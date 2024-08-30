using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystemUI.Models
{
    public class OPDDto
    {
        public int opdId { get; set; }

        public int isPoP { get; set; }
        public int investigationID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int sessionType { get; set; }
        public int paidStatus { get; set; }
        public string? name { get; set; }
        public int age { get; set; }
        public int months { get; set; }
        public int days { get; set; }
        public int sex { get; set; }
        public string? phone { get; set; }
        public int OpdType { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? ConsultantName { get; set; }
        public int? RoomNumber { get; set; }
        public string? RoomName { get; set; }
        public string? CreatedUserName { get; set; }

		public OPD? opd { get; set; }
        public Patient? patient { get; set; }
        public Drug? Drug { get; set; }
        public Investigation? investigation { get; set; }
        public Shift isNightShift { get; set; }

        public OPDDrugus? opdDrugus { get; set; }
        public OPDInvestigation? opdInvestigation { get; set; }
        public OPDItem? opdItem { get; set; }

        public List<Patient>? patientsList { get; set; }
        public List<Consultant>? consultantList { get; set; }

        public List<OPD>? listopd { get; set; }

        public List<AppointmentDTO> CHAppoinmentDTO { get; set; }
        public List<OPDTbDto>? listOPDTbDto { get; set; }
        public List<OpdOtherXrayDataTableDto>? listOPDTbDtoSp { get; set; }
        public List<Drug>? Drugs { get; set; }
        public List<Investigation>? Investigations { get; set; }
        public List<Item>? Items { get; set; }

        public List<OPDDrugus>? OPDDrugusList { get; set; }
        public List<OPDInvestigation>? OPDInvestigationList { get; set; }
        public List<OPDItem>? OPDItemList { get; set; }

        #region ChannelingSchema

        public int chID { get; set; }
        public List<Patient> PatientList { get; set; }
        public List<Channeling> ChannelingList { get; set; }
        public List<Consultant> listConsultants { get; set; }
        public List<Specialist> listSpecialists { get; set; }
        public List<Scan> listChannelingItems { get; set; }

        public List<ChannelingSchedule> listChannelingSchedule { get; set; }
        public int PatientID { get; set; }
        public int ConsultantID { get; set; }
        public int SpecialistId { get; set; }
        public string? Description { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public ChannellingScheduleStatus channellingScheduleStatus { get; set; }
        public Channeling channeling { get; set; }
        public Scan channelingItem { get; set; }
        public int ChannelingScheduleID { get; set; }

        public ChannelingSchedule channelingSchedule { get; set; }

        public int isVOGScan { get; set; }
        public int isCardioScan { get; set; }
        public int isExerciseBook { get; set; }
        public int isClinicBook { get; set; }

        public int scanId { get; set; }

        public Scan? vogScan { get; set; }
        public Scan? echoScan { get; set; }
        public Drug? exerciseBook { get; set; }
        public Drug? clinicBook { get; set; }

        public List<Scan>? scanList { get; set; }
        public int appoinmentNo { get; set; }

        #endregion
    }
}