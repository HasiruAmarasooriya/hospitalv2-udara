using HospitalMgrSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace HospitalMgrSystemUI.Models
{
    public class AdmissionDto
    {
        public List<Room> Rooms { get; set; }
        public List<Consultant> Consultants { get; set; }
        public List<Patient> Patients { get; set; }
        public Admission Admissions { get; set; }
        public List<Admission> listAdmission { get; set; }
        public AdmissionDrugus AdmissionDrugus { get; set; }
        public AdmissionInvestigation AdmissionInvestigations { get; set; }
        public List<AdmissionDrugus> AdmissionDrugusList { get; set; }
        public List<Drug> Drugs { get; set; }
        public Drug Drug { get; set; }
        public List<Investigation> Investigations { get; set; }
        public List<AdmissionInvestigation> AdmissionInvestigationList { get; set; }
        public string SearchValue { get; set; }
        public int AdmissionsId { get; set; }
        public List<Consultant> consultants  { get; set; }
        public AdmissionConsultant AdmissionConsultants { get; set; }
        public List<AdmissionConsultant> AdmissionConsultantList { get; set; }
        public List<Item> Items { get; set; }
        public AdmissionItems AdmissionItem { get; set; }
        public List<AdmissionItems> AdmissionItemsList { get; set; }
    }
}
