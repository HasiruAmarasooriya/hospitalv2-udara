using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class Video
    {
        public int Id { get; set; }
        public string? YoutubeId { get; set; }
       
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        
    }
}
