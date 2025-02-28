using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;

namespace HospitalMgrSystemUI.Models
{
    public class WarehouseDto
    {
        public List<GRN> supplierList { get; set; }

      
        public List<DrugsCategory> DrugsCategory { get; set; }
        public List<DrugsSubCategory> DrugsSubCategory { get; set; }
        public List<Drug > ListDrogs { get; set; }
        public List <GRPVDetailsDto> grpvList { get; set; }
        public List<RequestDetailsDto> RqeuestList { get; set; }
        public List<RequestItemDetailsDto> RqeuestItem { get; set; }
        public List <MainStoresDto> MainStore{ get; set; }
        public List<OPDStoresDto> OPDStore { get; set; }
        public List<AddmisionStoresDto> AddmisionStore { get; set; }
        public List<AddmisionStoresDetailsDto> AddmissionTrans {  get; set; }
        public List<OPDStoresDetaisDto>OPDTrans { get; set; }
        public string? Batch { get; set; }
        public string? Serial { get; set; }
        public int Qty { get; set; }
        public int Price { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime ProductDate { get; set; }
        public GRN grn { get; set; }
       public GRPV gRPV { get; set; }
        public Drug Drug { get; set; }
        public DateTime PreviousDateTime { get; set; }
        public string SearchValue { get; set; }
    }
}
