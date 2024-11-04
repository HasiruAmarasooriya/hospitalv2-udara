using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class ClaimBillItems
    {
        public int Id { get; set; }
        public int? ClaimBillId { get; set; }
        [ForeignKey(nameof(ClaimBillId))] public ClaimBill? ClaimBill { get; set; }
		public int? ScanItemId { get; set; }
        public string? RefId { get; set; }
        public string? ItemType { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public int? Qty { get; set; }


		public DateTime? CreateDate { get; set; }
        public int? CreateUser { get; set; }
        [ForeignKey(nameof(CreateUser))] public User? User { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedUser { get; set; }
        [ForeignKey(nameof(ModifiedUser))] public User? ModifiedBy { get; set; }

        [NotMapped]
        public string? ItemName { get; set; }
		[NotMapped]
		public decimal? HospitalFee { get; set; }

		[NotMapped]
		public decimal? DoctorFee { get; set; }

	}
}