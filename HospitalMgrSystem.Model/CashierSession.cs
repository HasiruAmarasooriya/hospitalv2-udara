using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class CashierSession
    {
        public int Id { get; set; }
        public int userID { get; set; }

        [ForeignKey("userID")]
        public User? User { get; set; }

        public decimal StartBalence { get; set; }
        public decimal EndBalence { get; set; }
        public decimal EndCardBalence { get; set; }
        public decimal Deviation { get; set; }

        public DateTime StartingTime { get; set; }
        public DateTime EndTime { get; set; }

        public CashierSessionStatus cashierSessionStatus { get; set; }

        public CommonStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public UserRole UserRole { get; set; }
        public int col1 { get; set; } // 1
        public int col2 { get; set; } // 2
        public int col3 { get; set; } // 5
        public int col4 { get; set; } // 10
        public int col5 { get; set; } // 20
        public int col6 { get; set; } // 50
        public int col7 { get; set; } // 100
        public int col8 { get; set; } // 500
        public int col9 { get; set; } // 1000
        public int col10 { get; set; }// 5000

        [NotMapped]
        public decimal col1Total { get; set; } // 1
        [NotMapped]
        public decimal col2Total { get; set; } // 2
        [NotMapped]
        public decimal col3Total { get; set; } // 5
        [NotMapped]
        public decimal col4Total { get; set; } // 10

        [NotMapped]
        public decimal col5Total { get; set; } // 20
        [NotMapped]
        public decimal col6Total { get; set; } // 50
        [NotMapped]
        public decimal col7Total { get; set; } // 100
        [NotMapped]
        public decimal col8Total { get; set; } // 500
        [NotMapped]
        public decimal col9Total { get; set; } // 1000
        [NotMapped]
        public decimal col10Total { get; set; } //5000
        [NotMapped]
        public decimal totalCashAmountHandover { get; set; } //cash handover
        [NotMapped]
        public decimal totalAmountHandover { get; set; } //cash & card

        [NotMapped]
        public decimal TotalAmount { get; set; }
		[NotMapped]
		public decimal DiscountAmount { get; set; }

        #region OPD Payment Data

        [NotMapped]
        public decimal OPDTotalAmount { get; set; }
        [NotMapped]
        public decimal OPDTotalPaidAmount { get; set; }
        [NotMapped]
        public decimal OPDTotalPaidCardAmount { get; set; }
        [NotMapped]
        public decimal OPDTotalRefund { get; set; }
        [NotMapped]
        public decimal OPDCashBalence { get; set; }
		[NotMapped]
		public decimal OPDTotalDiscount { get; set; }

        [NotMapped]
        public decimal ChannelingTotalAmount { get; set; }
        [NotMapped]
        public decimal ChannelingTotalPaidAmount { get; set; }
        [NotMapped]
        public decimal ChannelingTotalPaidCardAmount { get; set; }
        [NotMapped]
        public decimal ChannelingTotalDoctorPayment { get; set; }
        [NotMapped]
        public decimal ChannelingTotalRefund { get; set; }
        [NotMapped]
        public decimal ChannelingCashBalence { get; set; }
		[NotMapped]
		public decimal ChannelingDiscountAmount { get; set; }

        [NotMapped]
        public decimal XRAYTotalAmount { get; set; }
        [NotMapped]
        public decimal XRAYTotalPaidAmount { get; set; }
        [NotMapped]
        public decimal XRAYTotalPaidCardAmount { get; set; }
        [NotMapped]
        public decimal XRAYTotalRefund { get; set; }
        [NotMapped]
        public decimal XRAYCashBalence { get; set; }
		[NotMapped]
		public decimal XrayTotalDiscountAmount { get; set; }
		[NotMapped]
        public decimal OtherTotalAmount { get; set; }
        [NotMapped]
        public decimal OtherTotalPaidAmount { get; set; }
        [NotMapped]
        public decimal OtherTotalRefund { get; set; }
        [NotMapped]
        public decimal OtherTotalPaidCardAmount { get; set; }
        [NotMapped]
        public decimal OtherCashBalence { get; set; }
		[NotMapped]
		public decimal OtherTotalDiscountAmount { get; set; }

        [NotMapped]
        public decimal AllServiceTotalPaidAmount { get; set; }
        [NotMapped]
        public decimal AllServiceTotalRefund { get; set; }
        [NotMapped]
        public decimal AllServiceTotalPaidCardAmount { get; set; }
        [NotMapped]
        public decimal AllServiceCashBalence { get; set; }
        [NotMapped]
        public decimal AllServiceTotalAmount { get; set; }
		[NotMapped]
		public decimal AllServiceDiscountAmount { get; set; }
        #region Admission
        [NotMapped]
        public decimal AdmissionTotalAmount { get; set; }
        [NotMapped]
        public decimal AdmissionTotalPaidAmount { get; set; }
        [NotMapped]
        public decimal AdmissionTotalPaidCardAmount { get; set; }
        [NotMapped]
        public decimal AdmissionTotalDoctorPayment { get; set; }
        [NotMapped]
        public decimal AdmissionTotalRefund { get; set; }
        [NotMapped]
        public decimal AdmissionCashBalence { get; set; }
        [NotMapped]
        public decimal AdmissionDiscountAmount { get; set; }
        #endregion

        [NotMapped]
        public List<Consultant> notInSyestemConsultantList { get; set; }

        [NotMapped]
        public List<OtherTransactions> otherIncomeList { get; set; }

        [NotMapped]
        public decimal totalHospitaOtherIncome { get; set; }

        [NotMapped]
        public decimal totalGrandIncome { get; set; }
        [NotMapped]
        public decimal totalCashierTransferIn { get; set; }

        [NotMapped]
        public decimal totalCashierTransferOut{ get; set; }
        #endregion
    }
}
