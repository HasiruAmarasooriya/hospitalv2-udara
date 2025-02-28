using HospitalMgrSystem.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using HospitalMgrSystem.DataAccess;
using HospitalMgrSystem.Model.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Service.Stock
{
    public class StockService : IStockService
    {
        public Model.GRN AddGRN(Model.GRN stockDto)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {

                var existingSupplier = dbContext.GRN
                    .FirstOrDefault(s => s.SupplierName.ToLower() == stockDto.SupplierName.ToLower());


                if (existingSupplier != null)
                {
                    existingSupplier.SupplierContact = stockDto.SupplierContact;
                    existingSupplier.SupplierAddress = stockDto.SupplierAddress;
                    existingSupplier.ModifiedUser = stockDto.ModifiedUser;
                    existingSupplier.ModifiedDate = DateTime.Now;

                    dbContext.GRN.Update(existingSupplier);
                }
                else
                {
                    var grn = new GRN
                    {
                        SupplierName = stockDto.SupplierName,
                        SupplierContact = stockDto.SupplierContact,
                        SupplierAddress = stockDto.SupplierAddress,
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreateUser = stockDto.CreateUser,
                        ModifiedUser = stockDto.ModifiedUser
                    };

                    dbContext.GRN.Add(grn);
                }

                dbContext.SaveChanges();
                return existingSupplier ?? stockDto;
            }

        }
        public List<Model.GRN> GetAllGRN()
        {
            List<Model.GRN> mtList = new List<Model.GRN>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.GRN.ToList();
            }
            return mtList;
        }
        public Model.GRPV AddGRPV(Model.GRPV stockDto)
        {
            using (var dbContext = new DataAccess.HospitalDBContext())
            {
                dbContext.GRPV.Add(stockDto);
                dbContext.SaveChanges();


                var LogTran = new stockTransaction
                {
                    GrpvId = stockDto.Id,
                    DrugIdRef = stockDto.DrugId,
                    Qty = stockDto.Qty,
                    TranType = StoreTranMethod.Main_In,
                    RefNumber = $"GRN_{stockDto.GRNId}",
                    Remark = "Main_Stock_IN",
                    BatchNumber = stockDto.BatchNumber,
                    CreateUser = stockDto.CreateUser,
                    CreateDate = stockDto.CreateDate,
                    ModifiedUser = stockDto.ModifiedUser,
                    ModifiedDate = DateTime.Now
                };
                LogTransaction(LogTran);
                return stockDto;
            }
        }
        public Model.Warehouse AddStore(Model.Warehouse warehouse)
        {
            using (var dbContext = new DataAccess.HospitalDBContext())
            {
                dbContext.Warehouse.Add(warehouse);
                dbContext.SaveChanges();
                return warehouse;


            }
        }

        public List<GRPVDetailsDto> GetAllGRPV()
        {
            using var context = new HospitalDBContext();

            var grpvDetails = context.Set<GRPVDetailsDto>()
                .FromSqlRaw("EXEC GetAllGRPVDetails")
                .ToList();

            return grpvDetails;
        }
        public List<GRPVDetailsDto> GetBatchbyDrugID(int DrugID)
        {
            using var context = new HospitalDBContext();

            var grpvDetails = context.Set<GRPVDetailsDto>()
                .FromSqlRaw("EXEC GetStockDetailsByDrugId @DrugId = {0}", DrugID)
                .ToList();

            return grpvDetails;
        }
        public List<Model.Warehouse> GetAllStore()
        {
            List<Model.Warehouse> mtList = new List<Model.Warehouse>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Warehouse.ToList();
            }
            return mtList;
        }


        public Model.stockTransaction LogTransaction(Model.stockTransaction stockTran)
        {
            using (var dbContext = new DataAccess.HospitalDBContext())
            {
                dbContext.stockTransaction.Add(stockTran);
                dbContext.SaveChanges();
                return stockTran;


            }
        }
        private Type GetWarehouseType(int warehouseId)
        {
            return warehouseId switch
            {
                1 => typeof(GRPV),
                2 => typeof(OPDDrugus),
                3 => typeof(AdmissionDrugus),
                _ => throw new ArgumentException("Invalid warehouse ID")
            };
        }
        public void AddTransaction(int DrugId, decimal Qty, int FromWarehouse, int ToWarehouse, int UserId, int grpv, int grn, string batch_no)
        {
            using (var dbContext = new HospitalDBContext())
            {


                if (FromWarehouse == 1) // Main Warehouse stock handling
                {
                    HandleMainWarehouseStockReduction(dbContext, DrugId, Qty, FromWarehouse, ToWarehouse, UserId, grpv, grn, batch_no);
                    
                }
                else
                {
                    UpdateWarehouseStock(dbContext, DrugId, -Qty, FromWarehouse, ToWarehouse, UserId, grpv, grn, batch_no);
                    //UpdateWarehouseStock(dbContext, DrugId, Qty, ToWarehouse, UserId, grpv, grn, batch_no);
                }
                dbContext.SaveChanges();
            }
        }


        private void HandleMainWarehouseStockReduction(HospitalDBContext dbContext, int DrugId, decimal Qty, int FromWarehouse, int ToWarehouse, int UserId, int grpv, int grn, string batch_no)
        {

            // Log In transaction for the receiving warehouse
            if (FromWarehouse != 0)
            {
                switch (FromWarehouse)
                {
                    case 1:
                        int TranIn = (int)StoreTranMethod.Main_In;
                        int Tranout = (int)StoreTranMethod.Main_Out;
                        var qtycheck = GetStoresDetailsByTranType(TranIn, Tranout, DrugId).OrderBy(stock => stock.BatchNumber) .ToList();

                        if (qtycheck == null || !qtycheck.Any() || qtycheck.Sum(x => x.AvailableQty) < Qty)
                        {
                            throw new Exception("Insufficient stock.");
                        }

                        decimal remainingQty = Qty;

                        foreach (var batch in qtycheck)
                        {
                            if (remainingQty <= 0) break;
                            decimal toTransfer = Math.Min(remainingQty, batch.AvailableQty);
                            //batch.Qty -= toTransfer;
                            var LogTran = new stockTransaction
                            {
                                GrpvId = grpv,
                                DrugIdRef = DrugId,
                                Qty = -toTransfer,
                                TranType = StoreTranMethod.Main_Out,
                                RefNumber = $"GRN_{grn}",
                                Remark = "Main_Stock_Out",
                                BatchNumber = batch.BatchNumber,
                                CreateUser = UserId,
                                CreateDate = DateTime.Now,
                                ModifiedUser = UserId,
                                ModifiedDate = DateTime.Now
                            };
                            LogTransaction(LogTran);
                            UpdateWarehouseStock(dbContext, DrugId, toTransfer, ToWarehouse, FromWarehouse, UserId, grpv, grn, batch.BatchNumber);
                            // Log Main Out transaction
                            remainingQty -= toTransfer;
                            
                        }
                        break;

                    case 2:
                        var LogTranOPD = new stockTransaction
                        {
                            GrpvId = grpv,
                            DrugIdRef = DrugId,
                            Qty = -Qty,
                            TranType = StoreTranMethod.OPD_Out,
                            RefNumber = $"GRN_{grn}",
                            Remark = "OPD_Stock_IN",
                            BatchNumber = batch_no,
                            CreateUser = UserId,
                            CreateDate = DateTime.Now,
                            ModifiedUser = UserId,
                            ModifiedDate = DateTime.Now
                        };
                        LogTransaction(LogTranOPD);
                        break;
                    case 3:
                        var LogTranWard = new stockTransaction
                        {
                            GrpvId = grpv,
                            DrugIdRef = DrugId,
                            Qty = -Qty,
                            TranType = StoreTranMethod.Addmission_In,
                            RefNumber = $"GRN_{grn}",
                            Remark = "Addmission_Stock_IN",
                            BatchNumber = batch_no,
                            CreateUser = UserId,
                            CreateDate = DateTime.Now,
                            ModifiedUser = UserId,
                            ModifiedDate = DateTime.Now
                        };
                        LogTransaction(LogTranWard);
                        break;
                }
            }



        }

        private void UpdateWarehouseStock(HospitalDBContext dbContext, int DrugId, decimal Qty, int FromWarehouse,int ToWarehouse, int UserId, int grpv, int grn, string batch_no)
        {
            switch (FromWarehouse)
            {
                case 1:
                    if (Qty < 0)
                    {
                        var LogTranmain = new stockTransaction
                        {
                            GrpvId = grpv,
                            DrugIdRef = DrugId,
                            Qty = Qty,
                            TranType = StoreTranMethod.Main_In,
                            RefNumber = $"GRN_{grn}",
                            Remark = "Main_IN_Other",
                            BatchNumber = batch_no,
                            CreateUser = UserId,
                            CreateDate = DateTime.Now,
                            ModifiedUser = UserId,
                            ModifiedDate = DateTime.Now
                        };
                        LogTransaction(LogTranmain);
                        break;
                    }
                    else if (Qty > 0)
                    {
                      
                        var LogTranMain = new stockTransaction
                        {
                            GrpvId = grpv,
                            DrugIdRef = DrugId,
                            Qty = Qty,
                            TranType = StoreTranMethod.Main_In,
                            RefNumber = $"GRN_{grn}",
                            Remark = "Main_IN_Other",
                            BatchNumber = batch_no,
                            CreateUser = UserId,
                            CreateDate = DateTime.Now,
                            ModifiedUser = UserId,
                            ModifiedDate = DateTime.Now
                        };
                        LogTransaction(LogTranMain);
                        break;

                    }

                    break;
                case 2:
                    if (Qty < 0)
                    {
                        decimal requiredQty = Math.Abs(Qty);

                        int TranIn = (int)StoreTranMethod.OPD_IN;
                        int Tranout = (int)StoreTranMethod.OPD_Out;
                        var qtycheck = GetStoresDetailsByTranType(TranIn, Tranout, DrugId).OrderBy(stock => stock.BatchNumber).ToList();

                        if (qtycheck == null || !qtycheck.Any() || qtycheck.Sum(x => x.AvailableQty) < Qty)
                        {
                            throw new Exception("Insufficient stock.");
                        }

                        decimal remainingQty = requiredQty;
                        foreach (var batch in qtycheck)
                        {
                            if (remainingQty <= 0) break;
                            decimal toTransfer = Math.Min(remainingQty, batch.AvailableQty);
                            var LogTranOPD = new stockTransaction
                            {
                                GrpvId = grpv,
                                DrugIdRef = DrugId,
                                Qty = -toTransfer,
                                TranType = StoreTranMethod.OPD_Out,
                                RefNumber = $"GRN_{grn}",
                                Remark = "OPD_Stock_OUT",
                                BatchNumber = batch.BatchNumber,
                                CreateUser = UserId,
                                CreateDate = DateTime.Now,
                                ModifiedUser = UserId,
                                ModifiedDate = DateTime.Now
                            };
                            LogTransaction(LogTranOPD);
                            UpdateWarehouseStock(dbContext, DrugId, toTransfer, ToWarehouse, FromWarehouse, UserId, grpv, grn, batch.BatchNumber);
                            remainingQty -= toTransfer;
                        }
                    }
                    else
                    {
                        var LogTranOPD = new stockTransaction
                        {
                            GrpvId = grpv,
                            DrugIdRef = DrugId,
                            Qty = Qty,
                            TranType = StoreTranMethod.OPD_IN,
                            RefNumber = $"GRN_{grn}",
                            Remark = "OPD_Stock_IN",
                            BatchNumber = batch_no,
                            CreateUser = UserId,
                            CreateDate = DateTime.Now,
                            ModifiedUser = UserId,
                            ModifiedDate = DateTime.Now
                        };
                        LogTransaction(LogTranOPD);
                    }

                    break;
                case 3:
                    if (Qty < 0)
                    {
                        decimal requiredQty = Math.Abs(Qty);
                        int TranIn = (int)StoreTranMethod.Addmission_In;
                        int Tranout = (int)StoreTranMethod.Addmission_Out;
                        var qtycheck = GetStoresDetailsByTranType(TranIn, Tranout, DrugId).OrderBy(stock => stock.BatchNumber).ToList();

                        if (qtycheck == null || !qtycheck.Any() || qtycheck.Sum(x => x.AvailableQty) < Qty)
                        {
                            throw new Exception("Insufficient stock.");
                        }

                        decimal remainingQty = requiredQty;
                        foreach (var batch in qtycheck)
                        {
                            if (remainingQty <= 0) break;
                            decimal toTransfer = Math.Min(remainingQty, batch.AvailableQty);
                            var LogTranOPD = new stockTransaction
                            {
                                GrpvId = grpv,
                                DrugIdRef = DrugId,
                                Qty = -toTransfer,
                                TranType = StoreTranMethod.Addmission_Out,
                                RefNumber = $"GRN_{grn}",
                                Remark = "Admission_Stock_OUT",
                                BatchNumber = batch.BatchNumber,
                                CreateUser = UserId,
                                CreateDate = DateTime.Now,
                                ModifiedUser = UserId,
                                ModifiedDate = DateTime.Now
                            };
                            LogTransaction(LogTranOPD);
                            UpdateWarehouseStock(dbContext, DrugId, toTransfer, ToWarehouse, FromWarehouse, UserId, grpv, grn, batch.BatchNumber);
                            remainingQty -= toTransfer;
                        }
                    }
                    else
                    {
                        var LogTranOPD = new stockTransaction
                        {
                            GrpvId = grpv,
                            DrugIdRef = DrugId,
                            Qty = Qty,
                            TranType = StoreTranMethod.Addmission_In,
                            RefNumber = $"GRN_{grn}",
                            Remark = "Admission_Stock_IN",
                            BatchNumber = batch_no,
                            CreateUser = UserId,
                            CreateDate = DateTime.Now,
                            ModifiedUser = UserId,
                            ModifiedDate = DateTime.Now
                        };
                        LogTransaction(LogTranOPD);
                    }
                    break;
            }


        }

        public Model.StockRequest AddPurchaseRequest(Model.StockRequest stockDto)
        {
            try
            {
                using var dbContext = new DataAccess.HospitalDBContext();
                try
                {
                    stockDto.ModifiedDate = DateTime.Now;
                    dbContext.StockRequest.Add(stockDto);
                    dbContext.SaveChanges();
                    foreach (var item in stockDto.Items)
                    {
                        item.RequestID = stockDto.Id;
                        item.ModifiedUser = stockDto.ModifiedUser;
                        item.CreateUser = stockDto.CreateUser;
                        dbContext.StockRequestItem.Add(item);
                    }
                    dbContext.SaveChanges();
                    return stockDto;
                }
                catch
                {

                    throw;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider adding a logging service)
                Console.WriteLine($"Error adding purchase request: {ex.Message}");
                throw;
            }

        }
       
        public int PurchaseReqestSStatusUpdate(int requestId)
        {
            using var dbContext = new DataAccess.HospitalDBContext();

            var result = (from p in dbContext.StockRequest where p.Id == requestId select p).SingleOrDefault();

            if (result.Status == CommonStatus.Active)
            {
                result.Status = CommonStatus.Inactive;
            }

            else
            {
                result.Status = CommonStatus.Inactive;
            }
            dbContext.SaveChanges();

            return result.Id;
        }
        public List<RequestDetailsDto> GetStockRequestDetails(int RequestId)
        {
            using var context = new HospitalDBContext();

            var grpvDetails = context.Set<RequestDetailsDto>()
                .FromSqlRaw("EXEC GetStockRequestDetails @RequestID = {0}", RequestId)
                .ToList();

            return grpvDetails;
        }
        public List<DrugStoresDetailsDto> GetStoresDetailsByTranType(int TranIn,int TranOut)
        {
            using var context = new HospitalDBContext();

            var grpvDetails = context.Set<DrugStoresDetailsDto>()
                .FromSqlRaw("EXEC GetStoresDetailsByTranType @TranINType = {0},@TranOuttype = {1}", TranIn, TranOut)
                .ToList();

            return grpvDetails;
        }
        public List<StoresDetailsDto> GetStoresDetailsByTranTypeByDate(int TranIn, int TranOut,int Refund, DateTime StartDate,DateTime EndDate)
        {
            using var context = new HospitalDBContext();

            var grpvDetails = context.Set<StoresDetailsDto>()
                .FromSqlRaw("EXEC GetStoresDetailsByTranTypeByDate @TranINType = {0},@TranOuttype = {1},@TranRefndType = {2},@StartDate={3},@EndDate={4}", TranIn, TranOut, Refund, StartDate, EndDate)
                .ToList();

            return grpvDetails;
        }
        public List<RequestDetailsDto> GetAllStockRequestDetails()
        {
            using var context = new HospitalDBContext();

            var grpvDetails = context.Set<RequestDetailsDto>()
                .FromSqlRaw("EXEC GetAllStockRequestDetails")
                .ToList();

            return grpvDetails;
        }
        public List<RequestDetailsByIdDto> GetReqestDetailsByRequestID(int RequestId)
        {
            using var context = new HospitalDBContext();

            var grpvDetails = context.Set<RequestDetailsByIdDto>()
                .FromSqlRaw("EXEC GetReqestDetailsByRequestID @RequestID={0}", RequestId)
                .ToList();

            return grpvDetails;
        }
        public List<StockQuantityDto> GetStoresDetailsByTranType(int TranIn, int TranOut,int DrugId)
        {
            using var context = new HospitalDBContext();

            var grpvDetails = context.Set<StockQuantityDto>()
                .FromSqlRaw("EXEC GetQtyByTranTypeAndDrugId @TranINType = {0},@TranOuttype = {1},@DrugId = {2}", TranIn, TranOut, DrugId).ToList();
                

            return grpvDetails;
        }

    }


}
