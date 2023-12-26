using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class Investigation
    {
        public int Id { get; set; }
        public string? SNo { get; set; }
        public string? InvestigationName { get; set; }
        public int InvestigationCategoryId { get; set; }
        [ForeignKey("InvestigationCategoryId")]
        public InvestigationCategory? InvestigationCategory { get; set; }
        public int InvestigationSubCategoryId { get; set; }
        [ForeignKey("InvestigationSubCategoryId")]
        public InvestigationSubCategory? InvestigationSubCategory { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
