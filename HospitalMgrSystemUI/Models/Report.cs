using HospitalMgrSystem.Model;

namespace HospitalMgrSystemUI.Models
{
    public class Report
    {
        public int paidStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime DateTime { get; set; }
        public int InvoicedType { get; set; }

        #region OPD DATA
        public (List<OPD>?, int?, decimal?) listopd { get; set; }
        public List<OPD>? listPartialPaidOPD { get; set; }
        public List<OPD>? listNeedToPayOPD { get; set; }
        public List<OPD>? listNightShiftOPD { get; set; }
        public List<OPD>? listNotPaidOPD { get; set; }
        public Payment? OPDPaymentData { get; set; }
        public List<OPDDrugus>? listopdGrugs { get; set; }

        #endregion

        #region CHANNELING DATA
        public List<OPD>? listChanneling { get; set; }
        public List<OPD>? listPartialPaidChanneling { get; set; }
        public List<OPD>? listNeedToPayChanneling { get; set; }
        public List<OPD>? listNightShiftChanneling { get; set; }
        public List<OPD>? listNotPaidChanneling { get; set; }
        public Payment? channelingPaymentData { get; set; }
        public List<OPDDrugus>? listChannelingGrugs { get; set; }
        #endregion

        #region XRAY DATA
        public (List<OPD>?, int?, decimal?) listXRAY { get; set; }
        public List<OPD>? listPartialPaidXRAY { get; set; }
        public List<OPD>? listNeedToPayXRAY { get; set; }
        public List<OPD>? listNightShiftXRAY { get; set; }
        public List<OPD>? listNotPaidXRAY { get; set; }
        public Payment? XRAYPaymentData { get; set; }
        public List<OPDDrugus>? listXRAYGrugs { get; set; }
        #endregion

        #region OTHER DATA
        public (List<OPD>?, int?, decimal?) listOTHER { get; set; }
        public List<OPD>? listPartialPaidOTHER { get; set; }
        public List<OPD>? listNeedToPayOTHER { get; set; }
        public List<OPD>? listNightShiftOTHER { get; set; }
        public List<OPD>? listNotPaidOTHER { get; set; }
        public Payment? OTHERPaymentData { get; set; }
        public List<OPDDrugus>? listOTHERGrugs { get; set; }
        #endregion

        #region CHANNELING SCHEDULE

        public List<ChannelingSchedule> ChannelingSchedules { get; set; }

        #endregion

    }
}
