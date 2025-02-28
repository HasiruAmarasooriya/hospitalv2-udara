using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class stockTransaction
    {
        public int Id { get; set; }
        public int GrpvId { get; set; }
        public int BillId {  get; set; }

        [ForeignKey("Drug")]
        public int DrugIdRef { get; set; }
        public Drug? Drug { get; set; }

        public Decimal Qty { get; set; }
        public StoreTranMethod TranType { get; set; }
        public string? RefNumber { get; set; }
        public string? Remark { get; set; }
        public string? BatchNumber { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
