using System.ComponentModel.DataAnnotations.Schema;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model;

public class OPD
{
	public int Id { get; set; }

	//OPD Shedular need to be add here
	//Payment Status
	public string? Description { get; set; }
	public PaymentStatus paymentStatus { get; set; }
	public int PatientID { get; set; }
	[ForeignKey("PatientID")] public Patient? patient { get; set; }
	public int ConsultantID { get; set; }
	[ForeignKey("ConsultantID")] public Consultant? consultant { get; set; }
	public int RoomID { get; set; }
	[ForeignKey("RoomID")] public Room? room { get; set; }

	public decimal ConsultantFee { get; set; }
	public decimal HospitalFee { get; set; }
	public decimal OtherFee { get; set; }

	public int AppoimentNo { get; set; }
	public int isOnOPD { get; set; }
	public DateTime DateTime { get; set; }
	public DateTime CreateDate { get; set; }
	public DateTime ModifiedDate { get; set; }
	public CommonStatus Status { get; set; }
	public int CreatedUser { get; set; }
	public int ModifiedUser { get; set; }
	public int shiftID { get; set; }

	[ForeignKey("shiftID")] public NightShiftSession? nightShiftSession { get; set; }
	public InvoiceType invoiceType { get; set; }
	public int schedularId { get; set; }

	[NotMapped] public decimal? TotalRefund { get; set; }
	[NotMapped] public decimal? TotalNeedToRefund { get; set; }
	[NotMapped] public decimal? TotalOldAmount { get; set; }
	[NotMapped] public decimal? TotalAmount { get; set; }
	[NotMapped] public decimal? TotalPaidAmount { get; set; }
	[NotMapped] public decimal? deviation { get; set; }

	[NotMapped] public User? issuedUser { get; set; }
	[NotMapped] public User? cashier { get; set; }

	[NotMapped] public int? invoiceID { get; set; }
	[NotMapped] public int OpdType { get; set; }
	[NotMapped] public ChannelingSchedule channelingScheduleData { get; set; }
	[NotMapped] public int? isRefund { get; set; }
	[NotMapped] public string? refundedItem { get; set; }
	[NotMapped] public OPDDrugus? OpdDrugus { get; set; }

	[NotMapped] public decimal? BillCount { get; set; }
	[NotMapped] public decimal? DoctorAmount { get; set; }
	[NotMapped] public decimal? HospitalAmount { get; set; }
}