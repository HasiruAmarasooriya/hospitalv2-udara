using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystemUI.Models
{
    public class AdmissionDto
    {
        public List<Room> Rooms { get; set; }
        public List<Consultant> Consultants { get; set; }
        public List<Patient> Patients { get; set; }
        public List <AdmissionHospitalFee> HospitalFees { get; set; }
        public List<HospitalFeeListDto> HospitalFeeList { get; set; }
        public List<AdmissionsCharges> Charges { get; set; }

        public Patient Patient { get; set; }
        public Admission Admissions { get; set; }
        public List<Admission> listAdmission { get; set; }
        public AdmissionDrugus AdmissionDrugus { get; set; }
        public AdmissionInvestigation AdmissionInvestigations { get; set; }
        public List<AdmissionDrugus> AdmissionDrugusList { get; set; }
        public List<Drug> Drugs { get; set; }
        public Drug Drug { get; set; }
        public List<Investigation> Investigations { get; set; }
        public List<AdmissionInvestigation> AdmissionInvestigationList { get; set; }
       
        public AdmissionConsultant AdmissionConsultants { get; set; }
        public List<AdmissionConsultant> AdmissionConsultantList { get; set; }
        public List<Item> Items { get; set; }
        public AdmissionItems AdmissionItem { get; set; }
        public List<AdmissionItems> AdmissionItemsList { get; set; }
        public string SearchValue { get; set; }
        public int AdmissionsId { get; set; }
        public string? name { get; set; }
        public int age { get; set; }
        public int sex { get; set; }
        public string? phone { get; set; }
        public decimal TotalAmount { get; set; }
        public int CreatedUserName { get; set; }
        public int ModifiedUserName { get; set; }
     

    }
}
