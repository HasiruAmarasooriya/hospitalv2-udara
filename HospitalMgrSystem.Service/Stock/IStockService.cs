using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;

namespace HospitalMgrSystem.Service.Stock
{
    public interface IStockService
    {
        public Model.GRN AddGRN(Model.GRN grn);
        public void AddTransaction(int DrugId, decimal Qty, int FromWarehouse, int ToWarehouse, int UserId, int grpv, int grn, string batch_no);
        public List<Model.GRN> GetAllGRN();
        public List<GRPVDetailsDto> GetAllGRPV();
        public List<Model.Warehouse> GetAllStore();
        public List<GRPVDetailsDto> GetBatchbyDrugID(int DrugID);
        public Model.stockTransaction LogTransaction(Model.stockTransaction stockTran);
        public Model.StockRequest AddPurchaseRequest(StockRequest stockDto);
    }
}