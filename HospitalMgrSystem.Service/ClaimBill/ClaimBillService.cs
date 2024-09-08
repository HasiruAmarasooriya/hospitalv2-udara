using HospitalMgrSystem.DataAccess;
using HospitalMgrSystem.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Service.ClaimBill
{
	public class ClaimBillService : IClaimBill
	{

		public List<ClaimBillDto> GetAllClaimBillsSP()
		{
			var mtList = new List<ClaimBillDto>();
			using (var context = new HospitalDBContext())
			{
				mtList = context.Set<ClaimBillDto>()
					.FromSqlRaw("EXEC GetAllClaimBillHistory")
					.ToList();

			}
			return mtList;
		}

		public ClaimBillDto? GetAllClaimBillsSP(int id)
		{
			ClaimBillDto? claimBill;

			using (var context = new HospitalDBContext())
			{
				claimBill = context.Set<ClaimBillDto>()
					.FromSqlRaw("EXEC GetAllClaimBillHistoryById @Id = {0}", id)
					.FirstOrDefault();

			}
			return claimBill;
		}

		public List<Model.ClaimBillItems> GetChannelingItemsNames(List<Model.ClaimBillItems> billItemsList)
		{
			var dBContext = new HospitalDBContext();

			try
			{
				foreach (var item in billItemsList)
				{
					item.ItemName = item.ItemType switch
					{
						"CHE" => dBContext.ChannelingItems.Where(x => x.Id == item.ScanItemId)
							.Select(x => x.ItemName)
							.SingleOrDefault(),
						"OPD" => dBContext.Drugs.Where(x => x.Id == item.ScanItemId)
							.Select(x => x.DrugName)
							.SingleOrDefault(),
						_ => item.ItemName
					};
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return billItemsList;
		}


		public List<Model.ClaimBillItems> CreateClaimBillItemsList(List<Model.ClaimBillItems> billItemsList)
		{
			var dBContext = new HospitalDBContext();

			try
			{
				dBContext.ClaimBillItemsData.AddRange(billItemsList);
				dBContext.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return billItemsList;
		}

		public Model.ClaimBill CreateClaimBill(Model.ClaimBill claimBill)
		{
			var dBContext = new HospitalDBContext();

			try
			{
				if (claimBill != null && claimBill.Id == 0)
				{
					dBContext.ClaimBills.Add(claimBill);
					dBContext.SaveChanges();
				}
				else
				{
					var claimBillToUpdate = dBContext.ClaimBills.Where(x => x.Id == claimBill.Id).SingleOrDefault();

					claimBillToUpdate.PatientID = claimBill.PatientID;
					claimBillToUpdate.ConsultantId = claimBill.ConsultantId;
					claimBillToUpdate.RefNo = claimBill.RefNo;
					claimBillToUpdate.SubTotal = claimBill.SubTotal;
					claimBillToUpdate.Discount = claimBill.Discount;
					claimBillToUpdate.TotalAmount = claimBill.TotalAmount;
					claimBillToUpdate.CashAmount = claimBill.CashAmount;
					claimBillToUpdate.Balance = claimBill.Balance;
					claimBillToUpdate.CreateDate = claimBill.CreateDate;
					claimBillToUpdate.ModifiedDate = DateTime.Now;

					dBContext.SaveChanges();
				}

				return dBContext.ClaimBills
					.Include(x => x.Patient)
					.Include(x => x.Consultant)
					.Where(x => x.Id == claimBill.Id)
					.SingleOrDefault();

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Model.ClaimBill DeleteClaimBill(int id)
		{
			throw new NotImplementedException();
		}

		public Model.ClaimBill GetClaimBillById(int id)
		{
			throw new NotImplementedException();
		}

		public List<Model.ClaimBill> GetClaimBillByPatientId(int id)
		{
			throw new NotImplementedException();
		}
	}
}
