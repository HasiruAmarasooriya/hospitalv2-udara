using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class SMSCampaign
    {
        public int Id { get; set; }
        public int CampaignID { get; set; }
        public decimal CampaignCost { get; set; }
        public int duplicateNo { get; set; }
        public int invaliedNo { get; set; }
        public int maskBlockedUser { get; set; }
        public int sceduleID { get; set; }

        [ForeignKey("sceduleID")]
        public ChannelingSchedule? ChannelingSchedule { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
    }
}
