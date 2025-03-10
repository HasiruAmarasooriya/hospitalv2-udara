using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Service.Admission
{
    public interface IAdmissionService
    {
        public HospitalMgrSystem.Model.Admission CreateAdmission(HospitalMgrSystem.Model.Admission admission);
        public List<Model.Admission> GetAllAdmission();
        public Model.Admission GetAdmissionByID(int id);
        public HospitalMgrSystem.Model.Admission DeleteAdmission(HospitalMgrSystem.Model.Admission admission);
        public List<Model.Admission> SearchAdmission(string value);
        public HospitalMgrSystem.Model.AdmissionDrugus CreateAdmissionDrugus(HospitalMgrSystem.Model.AdmissionDrugus admissionDrugus);
        public List<Model.AdmissionDrugus> GetAdmissionDrugus(int AdmissionId ,PaymentStatus PayStatus);
        public Model.AdmissionDrugus DeleteAdmissionDrugus(HospitalMgrSystem.Model.AdmissionDrugus admissionDrugus);
        public HospitalMgrSystem.Model.AdmissionInvestigation CreateAdmissionInvestigation(HospitalMgrSystem.Model.AdmissionInvestigation admissionInvestigation);
        public List<Model.AdmissionInvestigation> GetAdmissionInvestigation(int AdmissionId, PaymentStatus PayStatus);
        public Model.AdmissionInvestigation DeleteAdmissionInvestigation(HospitalMgrSystem.Model.AdmissionInvestigation admissionInvestigation);
        public HospitalMgrSystem.Model.AdmissionConsultant CreateAdmissionConsultant(HospitalMgrSystem.Model.AdmissionConsultant admissionConsultant);
        public List<Model.AdmissionConsultant> GetAdmissionConsultant(int AdmissionId, PaymentStatus PayStatus);
        public Model.AdmissionConsultant DeleteAdmissionConsultant(HospitalMgrSystem.Model.AdmissionConsultant admissionConsultant);
        public HospitalMgrSystem.Model.AdmissionItems CreateAdmissionItems(HospitalMgrSystem.Model.AdmissionItems admissionItems);
        public List<Model.AdmissionItems> GetAdmissionItems(int AdmissionId, PaymentStatus PayStatus);
        public Model.AdmissionItems DeleteAdmissionItems(HospitalMgrSystem.Model.AdmissionItems admissionItems);
    }
}
