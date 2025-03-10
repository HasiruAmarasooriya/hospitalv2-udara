using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;

namespace HospitalMgrSystemUI.Models
{
	public class Report
	{
		public int paidStatus { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public DateTime DateTime { get; set; }
		public int InvoicedType { get; set; }
        #region Admision DATA
        public (List<Admission>?, int?, decimal?) listadmission { get; set; }
        public List<Admission>? listPartialADM { get; set; }
        public List<Admission>? listNeedToPayADM { get; set; }
        public List<Admission>? listNightShiftADM { get; set; }
        public List<Admission>? listNotPaidADM { get; set; }
        public Payment? ADMPaymentData { get; set; }
        public PaymentSummaryOpdXrayOtherDTO? ADMPaymentDataDto { get; set; }
        public List<PaymentSummaryOfDoctorsOPDDTO>? ADMPaymentDataOfDoctorsDto { get; set; }
        public List<AdmissionDrugus>? listADMGrugs { get; set; }
        public List<ReportOpdXrayOtherDrugs> listADMDGrugsDto { get; set; }
        public List<ReportOpdXrayOtherPaidDto> ADMPaidDtos { get; set; }
        public List<ReportOpdXrayOtherRefundDTO> ADMRefundDtos { get; set; }

        #endregion

        #region OPD DATA
        public (List<OPD>?, int?, decimal?) listopd { get; set; }
		public List<OPD>? listPartialPaidOPD { get; set; }
		public List<OPD>? listNeedToPayOPD { get; set; }
		public List<OPD>? listNightShiftOPD { get; set; }
		public List<OPD>? listNotPaidOPD { get; set; }
		public Payment? OPDPaymentData { get; set; }
		public PaymentSummaryOpdXrayOtherDTO? OpdPaymentDataDto { get; set; }
		public List<PaymentSummaryOfDoctorsOPDDTO>? OpdPaymentDataOfDoctorsDto { get; set; }
		public List<OPDDrugus>? listopdGrugs { get; set; }
		public List<ReportOpdXrayOtherDrugs> listopdGrugsDto { get; set; }
		public List<ReportOpdXrayOtherPaidDto> OpdPaidDtos { get; set; }
		public List<ReportOpdXrayOtherRefundDTO> OpdRefundDtos { get; set; }

		#endregion

		#region CHANNELING DATA
		public List<OPD>? listChanneling { get; set; }
		public List<ChannelingPaidReport>? ChannelingPaidReports { get; set; }
		public List<OPD>? listPartialPaidChanneling { get; set; }
		public List<OPD>? listNeedToPayChanneling { get; set; }
		public List<ChannelingRefundReportDto>? ChannelingRefundReportDtos { get; set; }
		public List<OPD>? listNightShiftChanneling { get; set; }
		public List<OPD>? listNotPaidChanneling { get; set; }
		public List<ChannelingPaymentSummaryReportDto>? ChannelingPaymentSummaryReportDtos { get; set; }
		public Payment? channelingPaymentData { get; set; }
		public List<ForwardBookingDataTableDTO> ForwardBookingDataTableDtos { get; set; }
		public List<PreviousForwardBookingDataDto> PreviousForwardBookingDataDtos { get; set; }
		public List<DiscountTableReport> DiscountTableReportsDto { get; set; }
		public List<OPDDrugus>? listChannelingGrugs { get; set; }
		#endregion

		#region XRAY DATA
		public (List<OPD>?, int?, decimal?) listXRAY { get; set; }
		public List<OPD>? listPartialPaidXRAY { get; set; }
		public List<OPD>? listNeedToPayXRAY { get; set; }
		public List<OPD>? listNightShiftXRAY { get; set; }
		public List<OPD>? listNotPaidXRAY { get; set; }
		public Payment? XRAYPaymentData { get; set; }
		public PaymentSummaryOpdXrayOtherDTO? XrayPaymentDataDto { get; set; }
		public List<OPDDrugus>? listXRAYGrugs { get; set; }
		public List<ReportOpdXrayOtherDrugs>? listXRAYGrugsDto { get; set; }
		public List<ReportOpdXrayOtherPaidDto> XrayPaidDtos { get; set; }
		public List<ReportOpdXrayOtherRefundDTO> XrayRefundDtos { get; set; }
		#endregion

		#region OTHER DATA
		public (List<OPD>?, int?, decimal?) listOTHER { get; set; }
		public List<OPD>? listPartialPaidOTHER { get; set; }
		public List<OPD>? listNeedToPayOTHER { get; set; }
		public List<OPD>? listNightShiftOTHER { get; set; }
		public List<OPD>? listNotPaidOTHER { get; set; }
		public Payment? OTHERPaymentData { get; set; }
		public PaymentSummaryOpdXrayOtherDTO? OTHERPaymentDataDto { get; set; }
		public List<OPDDrugus>? listOTHERGrugs { get; set; }
		public List<ReportOpdXrayOtherDrugs>? listOTHERGrugsDto { get; set; }
		public List<ReportOpdXrayOtherPaidDto> OtherPaidDtos { get; set; }
		public List<ReportOpdXrayOtherRefundDTO> OtherRefundDtos { get; set; }
		#endregion

		#region CHANNELING SCHEDULE

		public List<ChannelingSchedule> ChannelingSchedules { get; set; }

		#endregion

	}
}
