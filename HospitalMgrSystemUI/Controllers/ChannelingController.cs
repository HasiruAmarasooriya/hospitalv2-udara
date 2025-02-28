using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.Channeling;
using HospitalMgrSystem.Service.ChannelingSchedule;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Default;
using HospitalMgrSystem.Service.Drugs;
using HospitalMgrSystem.Service.NightShiftSession;
using HospitalMgrSystem.Service.OPD;
using HospitalMgrSystem.Service.Patients;
using HospitalMgrSystem.Service.SMS;
using HospitalMgrSystem.Service.User;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers;

public class ChannelingController : Controller
{
	[BindProperty] public ChannelingDto channelingDto { get; set; }

	[BindProperty] public Channeling myChannelling { get; set; }

	[BindProperty] public OPDDto _OPDDto { get; set; }

	// private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

	public IActionResult Index()
	{
		var oPDDto = new OPDDto
		{
			StartTime = DateTime.Now,
			EndTime = DateTime.Now.AddDays(1),
			//listOPDTbDto = GetAllChannelingBySelectedSchedule(DateTime.Today),
			CHAppoinmentDTO = GetAllChannelingBySelectedSchedule(DateTime.Today),
			listSpecialists = new ChannelingService().GetAllSpecialists(),
			listConsultants = new ConsultantService().GetAllConsultantThatHaveSchedulingsByDate(DateTime.Today),
			paymentStatus = PaymentStatus.ALL,
			channellingScheduleStatus = ChannellingScheduleStatus.ALL
		};

		return View(oPDDto);
	}

	public IActionResult FilterForm()
	{
		var channelingDto = new OPDDto
		{
			StartTime = DateTime.Now,
			EndTime = DateTime.Now,
			listSpecialists = new ChannelingService().GetAllSpecialists()
		};

		//var oPDTbDto = new List<OPDTbDto>();
		var resultSet = new List<AppointmentDTO>();

		try
		{
			// When the user selects all the options
			//if (_OPDDto.SpecialistId != -2 && _OPDDto.paymentStatus != PaymentStatus.ALL &&
			//    _OPDDto.channellingScheduleStatus != ChannellingScheduleStatus.ALL)
			//    resultSet = new ChannelingService().GetAllChannelingByAllFilters(_OPDDto.StartTime, _OPDDto.EndTime,
			//        _OPDDto.SpecialistId, _OPDDto.paymentStatus, _OPDDto.channellingScheduleStatus);

			if (_OPDDto.channeling.ChannelingScheduleID != 0)
			{
				resultSet = new ChannelingService().ChannelingGetBySheduleIdSP(_OPDDto.channeling.ChannelingScheduleID);
				
			}

			if (resultSet != null)
			{
				channelingDto.CHAppoinmentDTO = resultSet;
				channelingDto.listConsultants = new ConsultantService().GetAllConsultantThatHaveSchedulings();
                channelingDto.channelingSchedule = new ChannelingScheduleService().SheduleGetById(_OPDDto.channeling.ChannelingScheduleID); ;


                return View("Index", channelingDto);
			}

			return RedirectToAction("Index");
		}
		catch (Exception ex)
		{
			return RedirectToAction("Index");
		}
	}
	//public IActionResult FilterForm()
	//{
	//    var channelingDto = new OPDDto()
	//    {
	//        StartTime = DateTime.Now,
	//        EndTime = DateTime.Now,
	//        listSpecialists = new ChannelingService().GetAllSpecialists()
	//    };

	//    var oPDTbDto = new List<OPDTbDto>();
	//    var resultSet = new List<OPD>();

	//    try
	//    {
	//        // When the user selects all the options
	//        if (_OPDDto.SpecialistId != -2 && _OPDDto.paymentStatus != PaymentStatus.ALL &&
	//            _OPDDto.channellingScheduleStatus != ChannellingScheduleStatus.ALL)
	//            resultSet = new ChannelingService().GetAllChannelingByAllFilters(_OPDDto.StartTime, _OPDDto.EndTime,
	//                _OPDDto.SpecialistId, _OPDDto.paymentStatus, _OPDDto.channellingScheduleStatus);

	//        // When the user selects the payment status, and the channeling schedule status
	//        if (_OPDDto.SpecialistId == -2 && _OPDDto.paymentStatus != PaymentStatus.ALL &&
	//            _OPDDto.channellingScheduleStatus != ChannellingScheduleStatus.ALL)
	//            resultSet = new ChannelingService().GetAllChannelingByPaymentStatusAndChannelingScheduleStatus(
	//                _OPDDto.StartTime, _OPDDto.EndTime, _OPDDto.paymentStatus, _OPDDto.channellingScheduleStatus);

	//        // When the user selects the specialist and the channeling schedule status
	//        if (_OPDDto.SpecialistId != -2 && _OPDDto.paymentStatus == PaymentStatus.ALL &&
	//            _OPDDto.channellingScheduleStatus != ChannellingScheduleStatus.ALL)
	//            resultSet = new ChannelingService().GetAllChannelingByDoctorSpecialityAndScheduleStatus(
	//                _OPDDto.StartTime, _OPDDto.EndTime, _OPDDto.SpecialistId, _OPDDto.channellingScheduleStatus);

	//        // When the user selects the specialist and the payment status
	//        if (_OPDDto.SpecialistId != -2 && _OPDDto.paymentStatus != PaymentStatus.ALL &&
	//            _OPDDto.channellingScheduleStatus == ChannellingScheduleStatus.ALL)
	//            resultSet = new ChannelingService().GetAllChannelingBySpecialistIdAndPaymentStatus(_OPDDto.StartTime,
	//                _OPDDto.EndTime, _OPDDto.paymentStatus, _OPDDto.SpecialistId);

	//        // When the user selects only the payment status
	//        if (_OPDDto.SpecialistId == -2 && _OPDDto.paymentStatus != PaymentStatus.ALL &&
	//            _OPDDto.channellingScheduleStatus == ChannellingScheduleStatus.ALL)
	//            resultSet = new ChannelingService().GetAllChannelingByPaymentStatus(_OPDDto.StartTime, _OPDDto.EndTime,
	//                _OPDDto.paymentStatus);

	//        // When the user selects only the channeling schedule status
	//        if (_OPDDto.SpecialistId == -2 && _OPDDto.paymentStatus == PaymentStatus.ALL &&
	//            _OPDDto.channellingScheduleStatus != ChannellingScheduleStatus.ALL)
	//            resultSet = new ChannelingService().GetAllChannelingByChannelingScheduleStatus(_OPDDto.StartTime,
	//                _OPDDto.EndTime, _OPDDto.channellingScheduleStatus);

	//        // When the user selects only the specialist
	//        if (_OPDDto.SpecialistId != -2 && _OPDDto.paymentStatus == PaymentStatus.ALL &&
	//            _OPDDto.channellingScheduleStatus == ChannellingScheduleStatus.ALL)
	//            resultSet = new ChannelingService().GetAllChannelingByDoctorSpeciality(_OPDDto.StartTime,
	//                _OPDDto.EndTime, _OPDDto.SpecialistId);

	//        // When the user selects only the date
	//        if (_OPDDto.SpecialistId == -2 && _OPDDto.paymentStatus == PaymentStatus.ALL &&
	//            _OPDDto.channellingScheduleStatus == ChannellingScheduleStatus.ALL)
	//            resultSet = new ChannelingService().GetAllChannelingByDateTime(_OPDDto.StartTime, _OPDDto.EndTime);

	//        foreach (var item in resultSet)
	//            oPDTbDto.Add(new OPDTbDto()
	//            {
	//                Id = item.Id,
	//                roomName = item.room.Name,
	//                Description = item.Description,
	//                consaltantName = item.consultant.Name,
	//                FullName = item.patient.FullName,
	//                MobileNumber = item.patient.MobileNumber,
	//                DateTime = item.DateTime,
	//                Sex = (SexStatus)item.patient.Sex,
	//                Status = item.Status,
	//                AppoimentNo = item.AppoimentNo,
	//                paymentStatus = item.paymentStatus,
	//                schedularId = item.schedularId,
	//                specialistData = item.consultant.Specialist,
	//                consaltantId = item.ConsultantID,
	//                channelingScheduleData = item.channelingScheduleData
	//            });

	//        channelingDto.listOPDTbDto = oPDTbDto;

	//        return View("Index", channelingDto);
	//    }
	//    catch (Exception ex)
	//    {
	//        return RedirectToAction("Index");
	//    }
	//}

	public ActionResult CreateChannelingAddOn(int Id)
	{
		using var httpClient = new HttpClient();
		try
		{
			if (Id == 0 || Id == null) return RedirectToAction("Index");

			var oPDChannelingDto = new OPDDto
			{
				opd = LoadChannelingByID(Id),
				opdId = Id
			};

			oPDChannelingDto.channelingSchedule = new ChannelingScheduleService().SheduleGetById(oPDChannelingDto.opd.schedularId);

			oPDChannelingDto.scanList = oPDChannelingDto.channelingSchedule.Consultant?.Specialist?.Id switch
			{
				12 => new DefaultService().GetAllScanChannelingFee(4), // Radio
				13 => new DefaultService().GetAllScanChannelingFee(3), // Cardio
                35 => new DefaultService().GetAllScanChannelingFee(3), // Ex-EEG
                                                                       // 44 => new DefaultService().GetAllScanChannelingFee(2), // VOG
                _ => new DefaultService().getProceduresByConsultantId(oPDChannelingDto.channelingSchedule.ConsultantId)
			};

			

			oPDChannelingDto.scanList?.Add(new Scan
			{
				Id = 100,
				ItemName = "Channeling",
			});

			var defaultService = new DefaultService();

			oPDChannelingDto.exerciseBook = defaultService.GetExerciseBookFee();
			oPDChannelingDto.clinicBook = defaultService.GetClinicBookFee();
			oPDChannelingDto.channelingSchedule.ConsultantFee = oPDChannelingDto.opd.ConsultantFee;
			oPDChannelingDto.channelingSchedule.HospitalFee = oPDChannelingDto.opd.HospitalFee;

			return PartialView("_PartialAddOn", oPDChannelingDto);
		}
		catch (Exception ex)
		{
			return RedirectToAction("Index");
		}
	}
	public ActionResult CreateChanneling(int Id)
	{
		var oPDChannelingDto = new OPDDto
		{
			listConsultants = LoadConsultant(),
			// oPDChannelingDto.PatientList = LoadPatient();
			listChannelingSchedule = LoadChannelingShedule(),
			Drugs = DrugsSearch(),
			vogScan = new DefaultService().GetScanChannelingFee(2),
			echoScan = new DefaultService().GetScanChannelingFee(3),
			exerciseBook = new DefaultService().GetExerciseBookFee(),
			listChannelingItems = LoadChannelingItems()
		};

		var consaltantFee = new DefaultService().GetDefailtConsaltantPrice();
		var hospitalFee = new DefaultService().GetDefailtHospitalPrice();

		var opdObject = new OPD
		{
			ConsultantFee = consaltantFee,
			HospitalFee = hospitalFee
		};


		if (Id > 0)
		{
			using var httpClient = new HttpClient();
			try
			{
				oPDChannelingDto.opd = LoadChannelingByID(Id);
				oPDChannelingDto.OPDDrugusList = GetChannelingDrugus(Id);
				oPDChannelingDto.opdId = Id;
				oPDChannelingDto.channelingSchedule =
					new ChannelingScheduleService().SheduleGetById(oPDChannelingDto.opd.schedularId);
				oPDChannelingDto.channelingSchedule.ConsultantFee = oPDChannelingDto.opd.ConsultantFee;
				oPDChannelingDto.channelingSchedule.HospitalFee = oPDChannelingDto.opd.HospitalFee;
				return PartialView("_PartialAddChanneling", oPDChannelingDto);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index");
			}
		}

		oPDChannelingDto.opd = opdObject;
		return PartialView("_PartialAddChanneling", oPDChannelingDto);
	}

	private List<OPDDrugus> GetChannelingDrugus(int id)
	{
		var oPDDrugus = new List<OPDDrugus>();

		using (var httpClient = new HttpClient())
		{
			try
			{
				oPDDrugus = new OPDService().GetOPDDrugus(id);
			}
			catch (Exception ex)
			{
			}
		}

		return oPDDrugus;
	}

	public List<Drug> DrugsSearch()
	{
		var drugs = new List<Drug>();
		using (var httpClient = new HttpClient())
		{
			try
			{
				drugs = new DrugsService().GetAllDrugsByStatus();
			}
			catch (Exception ex)
			{
			}
		}

		return drugs;
	}

	private List<Consultant> LoadConsultant()
	{
		try
		{
			var ConsultantLists = new ConsultantService().GetAllConsultantThatHaveSchedulings();
			return ConsultantLists;
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	private List<Patient> LoadPatient()
	{
		try
		{
			var PatientLists = new PatientService().GetAllPatientByStatus();
			return PatientLists;
		}
		catch (Exception ex)
		{
		}

		return null;
	}

	private List<Scan> LoadChannelingItems()
	{
		using (var httpClient = new HttpClient())
		{
			try
			{
				var channelingItems = new DefaultService().GetAllScanChannelingFee(4);

				return channelingItems;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}

	private List<ChannelingSchedule> LoadChannelingShedule()
	{
		var channelingSchedule = new List<ChannelingSchedule>();

		using (var httpClient = new HttpClient())
		{
			try
			{
				channelingSchedule = new ChannelingScheduleService().SheduleGetByStatus();
			}
			catch (Exception ex)
			{
			}
		}

		return channelingSchedule;
	}

	private List<OPDTbDto> GetAllChannelingByStatus()
	{
		var opd = new List<OPD>();
		var oPDTbDto = new List<OPDTbDto>();
		using (var httpClient = new HttpClient())
		{
			try
			{
				opd = new ChannelingService().GetAllChannelingByStatus();
				var result = opd;

				foreach (var item in result)
					oPDTbDto.Add(new OPDTbDto
					{
						Id = item.Id,
						roomName = item.room.Name,
						Description = item.Description,
						consaltantName = item.consultant.Name,
						FullName = item.patient.FullName,
						MobileNumber = item.patient.MobileNumber,
						DateTime = item.DateTime,
						Sex = (SexStatus)item.patient.Sex,
						Status = item.Status,
						AppoimentNo = item.AppoimentNo,
						paymentStatus = item.paymentStatus,
						schedularId = item.schedularId,
						specialistData = item.consultant.Specialist,
						consaltantId = item.ConsultantID,
						isRefund = item.isRefund,
						refundedItem = item.refundedItem,
						channelingScheduleData = item.channelingScheduleData
					});
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		return oPDTbDto;
	}

	private List<AppointmentDTO> GetAllChannelingBySelectedSchedule(DateTime dateTime)
	{
		using (var httpClient = new HttpClient())
		{
			try
			{
				List<AppointmentDTO> AppointmentDTOList = new ChannelingService().GetAllChannelingBySelectedScheduleSP(dateTime);
				return AppointmentDTOList;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		return null;
	}

	[HttpGet("GetSheduleGetById")]
	private ActionResult<ChannelingSchedule> GetSheduleGetById(int Id)
	{
		var channelingSchedule = new ChannelingSchedule();

		using (var httpClient = new HttpClient())
		{
			try
			{
				channelingSchedule = new ChannelingScheduleService().SheduleGetById(Id);
			}
			catch (Exception ex)
			{
			}
		}

		return channelingSchedule;
	}

	public async Task<IActionResult> UpdatePatientAndSendMessageAsync([FromBody] OPDDto oPDDto)
	{
		var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

		try
		{
			var patient = new Patient();

			oPDDto.patient.CreateUser = Convert.ToInt32(userIdCookie);
			oPDDto.patient.ModifiedUser = Convert.ToInt32(userIdCookie);
			patient = CreatePatient(oPDDto.patient);

			if (patient != null)
			{
				var channelingSMS = new ChannelingSMS();

				channelingSMS.channelingForOnePatient = LoadChannelingByID(oPDDto.opd.Id);
				channelingSMS.channelingSchedule =
					ChannelingScheduleGetByID(channelingSMS.channelingForOnePatient.schedularId);
				channelingSMS.ChannellingScheduleStatus = channelingSMS.channelingSchedule.scheduleStatus;

				var sMSService = new SMSService();
				await sMSService.SendSMSTokenForNewChannel(channelingSMS);
			}

			return RedirectToAction("Index");
		}
		catch (Exception ex)
		{
			return RedirectToAction("Index");
		}
	}


	public async Task<IActionResult> AddOnAsync([FromBody] OPDDto oPDDto)
	{
		var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

		var NightShiftSessionList = new List<NightShiftSession>();
		NightShiftSessionList = GetActiveShiftSession();

		try
		{
			var channelingObj = new OPD();
			var channelingSchedule = new ChannelingSchedule();
			var ScanObj = new Scan();


			if (oPDDto == null || oPDDto.opd == null) return RedirectToAction("Index");

			channelingObj = LoadChannelingByID(oPDDto.opd.Id);

			if (channelingObj == null) return RedirectToAction("Index");

			channelingSchedule = ChannelingScheduleGetByID(channelingObj.schedularId);

			if (channelingSchedule == null) return RedirectToAction("Index");

			if (oPDDto.scanId != 0)
			{
				// If Exercise Book
				if (oPDDto.scanId == 564)
				{
					oPDDto.scanId = 0;
					oPDDto.opd.Description = "Exercise Book";
					oPDDto.isExerciseBook = 1;
					ScanObj.HospitalFee = 0;
					ScanObj.DoctorFee = 0;
				}
				// If Clinic Book
				else if (oPDDto.scanId == 948)
				{
					oPDDto.scanId = 0;
					oPDDto.opd.Description = "Clinic Book";
					oPDDto.isClinicBook = 1;
					ScanObj.HospitalFee = 0;
					ScanObj.DoctorFee = 0;
				}
				// Get the default doctor channeling fee and hospital fee
				else if (oPDDto.scanId == 100)
				{
					ScanObj.Id = 100;
					ScanObj.HospitalFee = channelingSchedule.HospitalFee;
					ScanObj.DoctorFee = channelingSchedule.ConsultantFee;
					ScanObj.ItemName = "Channeling";
				}
				else
				{
					ScanObj = ScanGetByID(oPDDto.scanId);
				}
			}

			var OPDobj = new OPD();
			oPDDto.opd.Id = 0;
			oPDDto.opd.PatientID = channelingObj.PatientID;
			oPDDto.opd.ConsultantID = channelingSchedule.ConsultantId;
			oPDDto.opd.DateTime = DateTime.Now;
			oPDDto.opd.RoomID = 1;
			oPDDto.opd.schedularId = channelingSchedule.Id;
			oPDDto.opd.AppoimentNo = channelingObj.AppoimentNo;
			oPDDto.opd.HospitalFee = ScanObj.HospitalFee;
			oPDDto.opd.ConsultantFee = ScanObj.DoctorFee;
			oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
			oPDDto.opd.invoiceType = InvoiceType.CHE;
			oPDDto.opd.shiftID = NightShiftSessionList[0].Id;
			oPDDto.opd.ModifiedUser = Convert.ToInt32(userIdCookie);
			oPDDto.opd.CreatedUser = Convert.ToInt32(userIdCookie);
			oPDDto.opd.CreateDate = DateTime.Now;
			oPDDto.opd.ModifiedDate = DateTime.Now;


			if (ScanObj != null && ScanObj.Id != 0)
			{
				oPDDto.opd.Description = ScanObj.ItemName;
			}

			OPDobj = new OPDService().CreateOPD(oPDDto.opd);

			if (oPDDto.isExerciseBook == 1)
			{
				var drugScan = new Drug();
				drugScan = new DefaultService().GetExerciseBookFee();
				var Exercise = new OPDDrugus();
				Exercise.opdId = OPDobj.Id;
				Exercise.itemInvoiceStatus = ItemInvoiceStatus.Add;
				Exercise.Amount = drugScan.Price;
				Exercise.IsRefund = 0;
				Exercise.DrugId = drugScan.Id;
				Exercise.Type = 0;
				Exercise.Qty = 1;
				Exercise.Price = drugScan.Price;
				Exercise.billingItemsType = BillingItemsType.Items;
				Exercise.Status = CommonStatus.Active;
				Exercise.CreateUser = Convert.ToInt32(userIdCookie);
				Exercise.ModifiedUser = Convert.ToInt32(userIdCookie);
				Exercise.CreateDate = DateTime.Now;
				Exercise.ModifiedDate = DateTime.Now;

				new OPDService().CreateOPDDrugus(Exercise);
			}

			if (oPDDto.isClinicBook == 1)
			{
				var drugScan = new Drug();
				drugScan = new DefaultService().GetClinicBookFee();
				var Clinic = new OPDDrugus();
				Clinic.opdId = OPDobj.Id;
				Clinic.itemInvoiceStatus = ItemInvoiceStatus.Add;
				Clinic.Amount = drugScan.Price;
				Clinic.IsRefund = 0;
				Clinic.DrugId = drugScan.Id;
				Clinic.Type = 0;
				Clinic.Qty = 1;
				Clinic.Price = drugScan.Price;
				Clinic.billingItemsType = BillingItemsType.Items;
				Clinic.Status = CommonStatus.Active;
				Clinic.CreateUser = Convert.ToInt32(userIdCookie);
				Clinic.ModifiedUser = Convert.ToInt32(userIdCookie);
				Clinic.CreateDate = DateTime.Now;
				Clinic.ModifiedDate = DateTime.Now;

				new OPDService().CreateOPDDrugus(Clinic);
			}


			return RedirectToAction("Index");
		}
		catch (Exception ex)
		{
			return RedirectToAction("Index");
		}
	}

	public async Task<IActionResult> AddOnWithQRAsync([FromBody] OPDDto oPDDto)
	{
		var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

		var NightShiftSessionList = new List<NightShiftSession>();
		NightShiftSessionList = GetActiveShiftSession();

		try
		{
			OPD channelingObj = new OPD();
			var channelingSchedule = new ChannelingSchedule();
			var ScanObj = new Scan();


			if (oPDDto == null || oPDDto.opd == null) return RedirectToAction("Index");

			channelingObj = LoadChannelingByID(oPDDto.opd.Id);
			if (channelingObj != null)
			{
				channelingSchedule = ChannelingScheduleGetByID(channelingObj.schedularId);



				if (channelingSchedule == null) return RedirectToAction("Index");


				decimal total = 0;

				if (oPDDto.scanId != 0)
				{
					//if Exercise Book
					if (oPDDto.scanId == 564)
					{
						oPDDto.scanId = 0;
						oPDDto.opd.Description = "Exercise Book";
						oPDDto.isExerciseBook = 1;
						ScanObj.HospitalFee = 0;
						ScanObj.DoctorFee = 0;
						_OPDDto.Description = oPDDto.opd.Description;
					}
					else
					{
						ScanObj = ScanGetByID(oPDDto.scanId);
						_OPDDto.Description = ScanObj.ItemName;
						total = ScanObj.HospitalFee + ScanObj.DoctorFee;
					}

				}


				var OPDobj = new OPD();
				oPDDto.opd.Id = 0;
				oPDDto.opd.PatientID = channelingObj.PatientID;
				oPDDto.opd.ConsultantID = channelingSchedule.ConsultantId;
				oPDDto.opd.DateTime = DateTime.Now;
				oPDDto.opd.RoomID = 1;
				oPDDto.opd.schedularId = channelingSchedule.Id;
				oPDDto.opd.AppoimentNo = channelingObj.AppoimentNo;
				oPDDto.opd.HospitalFee = ScanObj.HospitalFee;
				oPDDto.opd.ConsultantFee = ScanObj.DoctorFee;
				oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
				oPDDto.opd.invoiceType = InvoiceType.CHE;
				oPDDto.opd.shiftID = NightShiftSessionList[0].Id;
				oPDDto.opd.ModifiedUser = Convert.ToInt32(userIdCookie);
				oPDDto.opd.CreatedUser = Convert.ToInt32(userIdCookie);
				oPDDto.opd.CreateDate = DateTime.Now;
				oPDDto.opd.ModifiedDate = DateTime.Now;



				if (ScanObj != null && ScanObj.Id != 0)
				{
					oPDDto.opd.Description = ScanObj.ItemName;
				}

				OPDobj = new OPDService().CreateOPD(oPDDto.opd);

				if (oPDDto.isExerciseBook == 1)
				{
					var drugScan = new Drug();
					drugScan = new DefaultService().GetExerciseBookFee();
					var Exercise = new OPDDrugus();
					Exercise.opdId = OPDobj.Id;
					Exercise.itemInvoiceStatus = ItemInvoiceStatus.Add;
					Exercise.Amount = drugScan.Price;
					Exercise.IsRefund = 0;
					Exercise.DrugId = drugScan.Id;
					Exercise.Type = 0;
					Exercise.Qty = 1;
					Exercise.Price = drugScan.Price;
					Exercise.billingItemsType = BillingItemsType.Items;
					Exercise.Status = CommonStatus.Active;
					Exercise.CreateUser = Convert.ToInt32(userIdCookie);
					Exercise.ModifiedUser = Convert.ToInt32(userIdCookie);
					Exercise.CreateDate = DateTime.Now;
					Exercise.ModifiedDate = DateTime.Now;
					total = drugScan.Price;
					new OPDService().CreateOPDDrugus(Exercise);



					//var channelingSMS = new ChannelingSMS();

					//channelingSMS.channelingForOnePatient = LoadChannelingByID(OPDobj.Id);
					//channelingSMS.channelingSchedule = ChannelingScheduleGetByID(channelingSMS.channelingForOnePatient.schedularId);
					//channelingSMS.ChannellingScheduleStatus = channelingSMS.channelingSchedule.scheduleStatus;
					//var sMSService = new SMSService();
					//await sMSService.SendSMSTokenForNewChannel(channelingSMS);
				}

				if (channelingObj != null)
				{
					_OPDDto.opdId = channelingObj.Id;
					_OPDDto.Description = oPDDto.opd.Description;
					_OPDDto.appoinmentNo = oPDDto.opd.AppoimentNo;
					_OPDDto.name = channelingObj.patient.FullName;
					_OPDDto.age = channelingObj.patient.Age;
					_OPDDto.months = channelingObj.patient.Months;
					_OPDDto.days = channelingObj.patient.Days;
					_OPDDto.sex = channelingObj.patient.Sex;
					_OPDDto.phone = channelingObj.patient.MobileNumber;
					_OPDDto.TotalAmount = total;
					_OPDDto.RoomNumber = OPDobj.RoomID;
                    _OPDDto.CreatedUserName = new UserService().GetUserByID(OPDobj.CreatedUser).FullName;
                    _OPDDto.ConsultantName = new ConsultantService().GetAllConsultantByID(OPDobj.ConsultantID).Name;
				}

			}
			return PartialView("_PartialQR", _OPDDto);
		}
		catch (Exception ex)
		{
			return RedirectToAction("Index");
		}
	}


	public async Task<IActionResult> AddNewChannelAsync([FromBody] OPDDto oPDDto)
	{
		var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

		var NightShiftSessionList = new List<NightShiftSession>();
		NightShiftSessionList = GetActiveShiftSession();

		try
		{
			//check appoint numer already taken or not
			var channelingSchedule = new ChannelingSchedule();
			var ScanObj = new Scan();
			var patient = new Patient();

			if (oPDDto == null || oPDDto.opd == null || oPDDto.patient == null) return RedirectToAction("Index");

			channelingSchedule = ChannelingScheduleGetByID(oPDDto.opd.schedularId);
			if (channelingSchedule == null) return RedirectToAction("Index");

			if (CheckAppoinmentNumberTaken(oPDDto.opd.AppoimentNo, oPDDto.opd.schedularId))
				return BadRequest("The appointment number is already taken");

			if (oPDDto.scanId != 0) ScanObj = ScanGetByID(oPDDto.scanId);

			oPDDto.patient.CreateUser = Convert.ToInt32(userIdCookie);
			oPDDto.patient.ModifiedUser = Convert.ToInt32(userIdCookie);
			patient = CreatePatient(oPDDto.patient);

			if (oPDDto.patient.MobileNumber == null || oPDDto.patient.MobileNumber.Length != 10)
				return RedirectToAction("Index");

			// Validate phone number format and characters
			foreach (var c in oPDDto.patient.MobileNumber)
				if (!char.IsDigit(c))
					return RedirectToAction("Index");

			if (patient != null)
			{
				if (CheckAppoinmentNumberTaken(oPDDto.opd.AppoimentNo, oPDDto.opd.schedularId))
					return BadRequest("The appointment number is already taken");
				
				var hospitalFee = channelingSchedule.HospitalFee;
				var consultantFee = channelingSchedule.ConsultantFee;
				var OPDobj = new OPD();

				if (oPDDto.isChanneling == 1)
				{
					oPDDto.opd.PatientID = patient.Id;
					oPDDto.opd.DateTime = DateTime.Now;
					oPDDto.opd.RoomID = 1;
					oPDDto.opd.Description = "Channelling";
					oPDDto.opd.ModifiedUser = Convert.ToInt32(userIdCookie);
					oPDDto.opd.CreatedUser = Convert.ToInt32(userIdCookie);
					oPDDto.opd.AppoimentNo = oPDDto.opd.AppoimentNo;
					oPDDto.opd.CreateDate = DateTime.Now;
					oPDDto.opd.ModifiedDate = DateTime.Now;
					oPDDto.opd.HospitalFee = oPDDto.OpdType == 1 ? hospitalFee : 0;
					oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
					oPDDto.opd.invoiceType = InvoiceType.CHE;
					oPDDto.opd.ConsultantFee = consultantFee;
					oPDDto.opd.shiftID = NightShiftSessionList[0].Id;

					if (ScanObj != null && ScanObj.Id != 0)
					{
						oPDDto.opd.Description = ScanObj.ItemName;
						oPDDto.opd.HospitalFee = ScanObj.HospitalFee;
						oPDDto.opd.ConsultantFee = ScanObj.DoctorFee;
					}

					if (oPDDto.opd.Id > 0)
						OPDobj = new OPDService().UpdateOPDStatus(oPDDto.opd, oPDDto.OPDDrugusList);
					else
						OPDobj = new OPDService().CreateOPD(oPDDto.opd);
				}

				if (oPDDto.isVOGScan == 1)
				{
					var vogScan = new Scan();
					vogScan = new DefaultService().GetScanChannelingFee(2);
					var responseOPD = new OPD();
					var OPDobjScans = new OPD();
					OPDobjScans.PatientID = patient.Id;
					OPDobjScans.DateTime = DateTime.Now;
					OPDobjScans.RoomID = 1;
					OPDobjScans.Description = "VOG Scan";
					OPDobjScans.ModifiedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.CreatedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.AppoimentNo = oPDDto.opd.AppoimentNo;
					OPDobjScans.CreateDate = DateTime.Now;
					OPDobjScans.ModifiedDate = DateTime.Now;
					OPDobjScans.ConsultantFee = vogScan.DoctorFee;
					OPDobjScans.HospitalFee = vogScan.HospitalFee;
					OPDobjScans.paymentStatus = PaymentStatus.NOT_PAID;
					OPDobjScans.invoiceType = InvoiceType.CHE;
					OPDobjScans.ConsultantID = oPDDto.opd.ConsultantID;
					OPDobjScans.shiftID = NightShiftSessionList[0].Id;
					OPDobjScans.schedularId = oPDDto.opd.schedularId;
					OPDobj = new OPDService().CreateOPD(OPDobjScans);
				}

				if (oPDDto.isCardioScan == 1)
				{
					var ecoScan = new Scan();
					ecoScan = new DefaultService().GetScanChannelingFee(3);
					var responseOPD = new OPD();
					var OPDobjScans = new OPD();
					OPDobjScans.PatientID = patient.Id;
					OPDobjScans.DateTime = DateTime.Now;
					OPDobjScans.RoomID = 1;
					OPDobjScans.Description = "Echo Test";
					OPDobjScans.ModifiedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.CreatedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.AppoimentNo = oPDDto.opd.AppoimentNo;
					OPDobjScans.CreateDate = DateTime.Now;
					OPDobjScans.ModifiedDate = DateTime.Now;
					OPDobjScans.ConsultantFee = ecoScan.DoctorFee;
					OPDobjScans.HospitalFee = ecoScan.HospitalFee;
					OPDobjScans.paymentStatus = PaymentStatus.NOT_PAID;
					OPDobjScans.invoiceType = InvoiceType.CHE;
					OPDobjScans.ConsultantID = oPDDto.opd.ConsultantID;
					OPDobjScans.shiftID = NightShiftSessionList[0].Id;
					OPDobjScans.schedularId = oPDDto.opd.schedularId;
					OPDobj = new OPDService().CreateOPD(OPDobjScans);
				}

				if (oPDDto.isExerciseBook == 1)
				{
					var drugScan = new Drug();
					drugScan = new DefaultService().GetExerciseBookFee();
					var responseOPD = new OPD();
					var OPDobjScans = new OPD();
					OPDobjScans.PatientID = patient.Id;
					OPDobjScans.DateTime = DateTime.Now;
					OPDobjScans.RoomID = 1;
					OPDobjScans.Description = "Exercise Book";
					OPDobjScans.ModifiedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.CreatedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.AppoimentNo = oPDDto.opd.AppoimentNo;
					OPDobjScans.CreateDate = DateTime.Now;
					OPDobjScans.ModifiedDate = DateTime.Now;
					OPDobjScans.ConsultantFee = 0;
					OPDobjScans.HospitalFee = 0;
					OPDobjScans.paymentStatus = PaymentStatus.NOT_PAID;
					OPDobjScans.invoiceType = InvoiceType.CHE;
					OPDobjScans.ConsultantID = oPDDto.opd.ConsultantID;
					OPDobjScans.shiftID = NightShiftSessionList[0].Id;
					OPDobjScans.schedularId = oPDDto.opd.schedularId;
					responseOPD = new OPDService().CreateOPD(OPDobjScans);

					var Exercise = new OPDDrugus();
					Exercise.opdId = responseOPD.Id;
					Exercise.itemInvoiceStatus = ItemInvoiceStatus.Add;
					Exercise.Amount = drugScan.Price;
					Exercise.IsRefund = 0;
					Exercise.DrugId = drugScan.Id;
					Exercise.Type = 0;
					Exercise.Qty = 1;
					Exercise.Price = drugScan.Price;
					Exercise.billingItemsType = BillingItemsType.Items;
					Exercise.Status = CommonStatus.Active;
					Exercise.CreateUser = Convert.ToInt32(userIdCookie);
					Exercise.ModifiedUser = Convert.ToInt32(userIdCookie);
					Exercise.CreateDate = DateTime.Now;
					Exercise.ModifiedDate = DateTime.Now;

					new OPDService().CreateOPDDrugus(Exercise);
				}

				if (OPDobj != null || oPDDto.OPDDrugusList != null)
				{
					foreach (var drugusItem in oPDDto.OPDDrugusList)
					{
						var responseOPD = new OPD();
						var OPDobjScans = new OPD();
						OPDobjScans.PatientID = patient.Id;
						OPDobjScans.DateTime = DateTime.Now;
						OPDobjScans.RoomID = 1;
						OPDobjScans.ModifiedUser = Convert.ToInt32(userIdCookie);
						OPDobjScans.CreatedUser = Convert.ToInt32(userIdCookie);
						OPDobjScans.AppoimentNo = oPDDto.opd.AppoimentNo;
						OPDobjScans.CreateDate = DateTime.Now;
						OPDobjScans.ModifiedDate = DateTime.Now;
						OPDobjScans.HospitalFee = oPDDto.OpdType == 1 ? hospitalFee : 0;
						OPDobjScans.paymentStatus = PaymentStatus.NOT_PAID;
						OPDobjScans.invoiceType = InvoiceType.CHE;
						OPDobjScans.ConsultantFee = consultantFee;
						OPDobjScans.ConsultantID = oPDDto.opd.ConsultantID;
						OPDobjScans.shiftID = NightShiftSessionList[0].Id;
						OPDobjScans.schedularId = oPDDto.opd.schedularId;
						responseOPD = new OPDService().CreateOPD(OPDobjScans);

						drugusItem.opdId = responseOPD.Id;
						drugusItem.itemInvoiceStatus = ItemInvoiceStatus.Add;
						drugusItem.Amount = drugusItem.Qty * drugusItem.Price;
						drugusItem.IsRefund = 0;
						new OPDService().CreateOPDDrugus(drugusItem);
					}

					var channelingSMS = new ChannelingSMS();

					channelingSMS.channelingForOnePatient = LoadChannelingByID(OPDobj.Id);
					channelingSMS.channelingSchedule =
						ChannelingScheduleGetByID(channelingSMS.channelingForOnePatient.schedularId);
					channelingSMS.ChannellingScheduleStatus = channelingSMS.channelingSchedule.scheduleStatus;

					//var sMSService = new SMSService();
					//await sMSService.SendSMSTokenForNewChannel(channelingSMS);
					SMSService sMSService = new SMSService();
					SMSActivation sMSActivation = new SMSActivation();
					sMSActivation = sMSService.GetSMSServiceStatus();
					if (sMSActivation.isActivate == SMSStatus.Active)
					{
						try
						{

							await sMSService.SendSMSTokenForNewChannel(channelingSMS);


						}
						catch (Exception ex)
						{
							Console.WriteLine($"Error sending SMS token time change: {ex.Message}");
						}

					}
				}
			}
            //Response.Cookies.Append("printFlag", "true");
            //Response.Cookies.Append("preId", cashierDtoToPrint.PreID.ToString());
            return RedirectToAction("Index");
		}
		catch (Exception ex)
		{
			return RedirectToAction("Index");
		}

	}

	public async Task<IActionResult> AddNewChannelWithQRAsync([FromBody] OPDDto oPDDto)
	{
		var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

		var NightShiftSessionList = new List<NightShiftSession>();
		NightShiftSessionList = GetActiveShiftSession();

		try
		{
			var channelingSchedule = new ChannelingSchedule();
			var ScanObj = new Scan();
			var patient = new Patient();

			if (oPDDto == null || oPDDto.opd == null || oPDDto.patient == null) return RedirectToAction("Index");

			channelingSchedule = ChannelingScheduleGetByID(oPDDto.opd.schedularId);
			if (channelingSchedule == null) return RedirectToAction("Index");

			if (CheckAppoinmentNumberTaken(oPDDto.opd.AppoimentNo, oPDDto.opd.schedularId))
				return BadRequest("The appointment number is already taken");

			if (oPDDto.scanId != 0) ScanObj = ScanGetByID(oPDDto.scanId);

			oPDDto.patient.CreateUser = Convert.ToInt32(userIdCookie);
			oPDDto.patient.ModifiedUser = Convert.ToInt32(userIdCookie);
			patient = CreatePatient(oPDDto.patient);

			if (oPDDto.patient.MobileNumber == null || oPDDto.patient.MobileNumber.Length != 10)
				return RedirectToAction("Index");

			// Validate phone number format and characters
			foreach (var c in oPDDto.patient.MobileNumber)
				if (!char.IsDigit(c))
					return RedirectToAction("Index");

			var name = patient.FullName;
			var age = patient.Age;
			var months = patient.Months;
			var days = patient.Days;
			var phone = patient.MobileNumber;
			var sex = patient.Sex;

			if (patient != null)
			{
				var hospitalFee = channelingSchedule.HospitalFee;
				var consultantFee = channelingSchedule.ConsultantFee;
				var OPDobj = new OPD();
				oPDDto.opd.PatientID = patient.Id;
				oPDDto.opd.DateTime = DateTime.Now;
				oPDDto.opd.RoomID = 1;
				oPDDto.opd.Description = "Channelling";
				oPDDto.opd.ModifiedUser = Convert.ToInt32(userIdCookie);
				oPDDto.opd.CreatedUser = Convert.ToInt32(userIdCookie);
				oPDDto.opd.AppoimentNo = oPDDto.opd.AppoimentNo;
				oPDDto.opd.CreateDate = DateTime.Now;
				oPDDto.opd.ModifiedDate = DateTime.Now;
				oPDDto.opd.HospitalFee = oPDDto.OpdType == 1 ? hospitalFee : 0;
				oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
				oPDDto.opd.invoiceType = InvoiceType.CHE;
				oPDDto.opd.ConsultantFee = consultantFee;
				oPDDto.opd.shiftID = NightShiftSessionList[0].Id;

				if (ScanObj != null && ScanObj.Id != 0)
				{
					oPDDto.opd.Description = ScanObj.ItemName;
					oPDDto.opd.HospitalFee = ScanObj.HospitalFee;
					oPDDto.opd.ConsultantFee = ScanObj.DoctorFee;
				}

				if (oPDDto.opd.Id > 0)
					OPDobj = new OPDService().UpdateOPDStatus(oPDDto.opd, oPDDto.OPDDrugusList);
				else
					if (CheckAppoinmentNumberTaken(oPDDto.opd.AppoimentNo, oPDDto.opd.schedularId))
					return BadRequest("The appointment number is already taken");
				OPDobj = new OPDService().CreateOPD(oPDDto.opd);

				if (OPDobj != null)
				{
					_OPDDto.opdId = OPDobj.Id;
					_OPDDto.name = name;
					_OPDDto.age = age;
					_OPDDto.months = months;
					_OPDDto.days = days;
					_OPDDto.sex = sex;
					_OPDDto.phone = phone;
					_OPDDto.Description = oPDDto.opd.Description;
					_OPDDto.appoinmentNo = oPDDto.opd.AppoimentNo;
					_OPDDto.TotalAmount = OPDobj.HospitalFee + OPDobj.ConsultantFee;
					_OPDDto.RoomName = new ChannelingScheduleService().SheduleGetById(oPDDto.opd.schedularId).Room?.Name;
					_OPDDto.ConsultantName = new ConsultantService().GetAllConsultantByID(OPDobj.ConsultantID).Name;
				}

				if (oPDDto.isVOGScan == 1)
				{
					var vogScan = new Scan();
					vogScan = new DefaultService().GetScanChannelingFee(2);
					var responseOPD = new OPD();
					var OPDobjScans = new OPD();
					OPDobjScans.PatientID = patient.Id;
					OPDobjScans.DateTime = DateTime.Now;
					OPDobjScans.RoomID = 1;
					OPDobjScans.Description = "VOG Scan";
					OPDobjScans.ModifiedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.CreatedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.AppoimentNo = oPDDto.opd.AppoimentNo;
					OPDobjScans.CreateDate = DateTime.Now;
					OPDobjScans.ModifiedDate = DateTime.Now;
					OPDobjScans.ConsultantFee = vogScan.DoctorFee;
					OPDobjScans.HospitalFee = vogScan.HospitalFee;
					OPDobjScans.paymentStatus = PaymentStatus.NOT_PAID;
					OPDobjScans.invoiceType = InvoiceType.CHE;
					OPDobjScans.ConsultantID = oPDDto.opd.ConsultantID;
					OPDobjScans.shiftID = NightShiftSessionList[0].Id;
					OPDobjScans.schedularId = oPDDto.opd.schedularId;
					responseOPD = new OPDService().CreateOPD(OPDobjScans);
				}

				if (oPDDto.isCardioScan == 1)
				{
					var ecoScan = new Scan();
					ecoScan = new DefaultService().GetScanChannelingFee(3);
					var responseOPD = new OPD();
					var OPDobjScans = new OPD();
					OPDobjScans.PatientID = patient.Id;
					OPDobjScans.DateTime = DateTime.Now;
					OPDobjScans.RoomID = 1;
					OPDobjScans.Description = "Echo Test";
					OPDobjScans.ModifiedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.CreatedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.AppoimentNo = oPDDto.opd.AppoimentNo;
					OPDobjScans.CreateDate = DateTime.Now;
					OPDobjScans.ModifiedDate = DateTime.Now;
					OPDobjScans.ConsultantFee = ecoScan.DoctorFee;
					OPDobjScans.HospitalFee = ecoScan.HospitalFee;
					OPDobjScans.paymentStatus = PaymentStatus.NOT_PAID;
					OPDobjScans.invoiceType = InvoiceType.CHE;
					OPDobjScans.ConsultantID = oPDDto.opd.ConsultantID;
					OPDobjScans.shiftID = NightShiftSessionList[0].Id;
					OPDobjScans.schedularId = oPDDto.opd.schedularId;
					responseOPD = new OPDService().CreateOPD(OPDobjScans);
				}

				if (oPDDto.isExerciseBook == 1)
				{
					var drugScan = new Drug();
					drugScan = new DefaultService().GetExerciseBookFee();
					var responseOPD = new OPD();
					var OPDobjScans = new OPD();
					OPDobjScans.PatientID = patient.Id;
					OPDobjScans.DateTime = DateTime.Now;
					OPDobjScans.RoomID = 1;
					OPDobjScans.Description = "Exercise Book";
					OPDobjScans.ModifiedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.CreatedUser = Convert.ToInt32(userIdCookie);
					OPDobjScans.AppoimentNo = oPDDto.opd.AppoimentNo;
					OPDobjScans.CreateDate = DateTime.Now;
					OPDobjScans.ModifiedDate = DateTime.Now;
					OPDobjScans.ConsultantFee = 0;
					OPDobjScans.HospitalFee = 0;
					OPDobjScans.paymentStatus = PaymentStatus.NOT_PAID;
					OPDobjScans.invoiceType = InvoiceType.CHE;
					OPDobjScans.ConsultantID = oPDDto.opd.ConsultantID;
					OPDobjScans.shiftID = NightShiftSessionList[0].Id;
					OPDobjScans.schedularId = oPDDto.opd.schedularId;
					responseOPD = new OPDService().CreateOPD(OPDobjScans);

					var Exercise = new OPDDrugus();
					Exercise.opdId = responseOPD.Id;
					Exercise.itemInvoiceStatus = ItemInvoiceStatus.Add;
					Exercise.Amount = drugScan.Price;
					Exercise.IsRefund = 0;
					Exercise.DrugId = drugScan.Id;
					Exercise.Type = 0;
					Exercise.Qty = 1;
					Exercise.Price = drugScan.Price;
					Exercise.billingItemsType = BillingItemsType.Items;
					Exercise.Status = CommonStatus.Active;
					Exercise.CreateUser = Convert.ToInt32(userIdCookie);
					Exercise.ModifiedUser = Convert.ToInt32(userIdCookie);
					Exercise.CreateDate = DateTime.Now;
					Exercise.ModifiedDate = DateTime.Now;

					new OPDService().CreateOPDDrugus(Exercise);
				}


				if (OPDobj != null || oPDDto.OPDDrugusList != null)
				{
					foreach (var drugusItem in oPDDto.OPDDrugusList)
					{
						var responseOPD = new OPD();
						var OPDobjScans = new OPD();
						OPDobjScans.PatientID = patient.Id;
						OPDobjScans.DateTime = DateTime.Now;
						OPDobjScans.RoomID = 1;
						OPDobjScans.ModifiedUser = Convert.ToInt32(userIdCookie);
						OPDobjScans.CreatedUser = Convert.ToInt32(userIdCookie);
						OPDobjScans.AppoimentNo = oPDDto.opd.AppoimentNo;
						OPDobjScans.CreateDate = DateTime.Now;
						OPDobjScans.ModifiedDate = DateTime.Now;
						OPDobjScans.HospitalFee = oPDDto.OpdType == 1 ? hospitalFee : 0;
						OPDobjScans.paymentStatus = PaymentStatus.NOT_PAID;
						OPDobjScans.invoiceType = InvoiceType.CHE;
						OPDobjScans.ConsultantFee = consultantFee;
						OPDobjScans.ConsultantID = oPDDto.opd.ConsultantID;
						OPDobjScans.shiftID = NightShiftSessionList[0].Id;
						OPDobjScans.schedularId = oPDDto.opd.schedularId;
						responseOPD = new OPDService().CreateOPD(OPDobjScans);

						drugusItem.opdId = responseOPD.Id;
						drugusItem.itemInvoiceStatus = ItemInvoiceStatus.Add;
						drugusItem.Amount = drugusItem.Qty * drugusItem.Price;
						drugusItem.IsRefund = 0;
						new OPDService().CreateOPDDrugus(drugusItem);
					}

					var channelingSMS = new ChannelingSMS();

					channelingSMS.channelingForOnePatient = LoadChannelingByID(OPDobj.Id);
					channelingSMS.channelingSchedule =
						ChannelingScheduleGetByID(channelingSMS.channelingForOnePatient.schedularId);
					channelingSMS.ChannellingScheduleStatus = channelingSMS.channelingSchedule.scheduleStatus;

					//var sMSService = new SMSService();
					//await sMSService.SendSMSTokenForNewChannel(channelingSMS);


					SMSService sMSService = new SMSService();
					SMSActivation sMSActivation = new SMSActivation();
					sMSActivation = sMSService.GetSMSServiceStatus();
					if (sMSActivation.isActivate == SMSStatus.Active)
					{
						try
						{

							await sMSService.SendSMSTokenForNewChannel(channelingSMS);


						}
						catch (Exception ex)
						{
							Console.WriteLine($"Error sending SMS token time change: {ex.Message}");
						}

					}
				}
			}

			return PartialView("_PartialQR", _OPDDto);
        }
		catch (Exception ex)
		{
			return RedirectToAction("Index");
		}

	}

	private List<NightShiftSession> GetActiveShiftSession()
	{
		var NightShiftSessionList = new List<NightShiftSession>();

		using (var httpClient = new HttpClient())
		{
			try
			{
				NightShiftSessionList = new NightShiftSessionService().GetACtiveNtShiftSessions();
			}
			catch (Exception ex)
			{
			}
		}

		return NightShiftSessionList;
	}

	public Patient CreatePatient(Patient patientObj)
	{
		try
		{
			patientObj.CreateDate = DateTime.Now;
			patientObj.ModifiedDate = DateTime.Now;
			if (patientObj != null)
			{
				var patientService = new PatientService();
				var resPatient = patientService.CreatePatient(patientObj);
				return resPatient;
			}

			return null;
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	public IActionResult DeleteChanneling()
	{
		using (var httpClient = new HttpClient())
		{
			try
			{
				new ChannelingService().DeleteChanneling(myChannelling.Id);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index");
			}
		}
	}

	private OPD LoadChannelingByID(int id)
	{
		var channeling = new OPD();

		using (var httpClient = new HttpClient())
		{
			try
			{
				channeling = new ChannelingService().GetChannelByIDNew(id);
			}
			catch (Exception ex)
			{
			}
		}

		return channeling;
	}
	private bool CheckAppoinmentNumberTaken(int apNo, int shID)
	{


		using (var httpClient = new HttpClient())
		{
			try
			{
				return new ChannelingService().GetChannelByAppointmentNoAndScheduleID(apNo, shID);
			}
			catch (Exception ex)
			{
				return false;
			}
		}


	}
	private ChannelingSchedule ChannelingScheduleGetByID(int id)
	{
		var channelingSchedule = new ChannelingSchedule();

		using (var httpClient = new HttpClient())
		{
			try
			{
				channelingSchedule = new ChannelingScheduleService().SheduleGetById(id);
			}
			catch (Exception ex)
			{
			}
		}

		return channelingSchedule;
	}

	private Scan ScanGetByID(int id)
	{
		var scanObj = new Scan();

		using (var httpClient = new HttpClient())
		{
			try
			{
				scanObj = new ChannelingScheduleService().GetChannelingItemById(id);
			}
			catch (Exception ex)
			{
			}
		}

		return scanObj;
	}

	public ActionResult OpenQR(int Id)
	{
		var opdDto = new OPDDto();
		opdDto.opd = new OPDService().GetAllOPDByID(Id);
		var channelingSchedule = new ChannelingScheduleService().SheduleGetById(opdDto.opd.schedularId);
		opdDto.opdId = Id;

		var name = opdDto.opd.patient?.FullName;
		var age = opdDto.opd.patient!.Age;
		var months = opdDto.opd.patient.Months;
		var days = opdDto.opd.patient.Days;
		var phone = opdDto.opd.patient.MobileNumber;
		var sex = opdDto.opd.patient.Sex;
		var totalAmount = opdDto.opd.TotalAmount;
		var consultant = new ConsultantService().GetAllConsultantByID(opdDto.opd.ConsultantID);
		var roomNumber = channelingSchedule.Room!.Name;
		var appointmentNumber = opdDto.opd.AppoimentNo;
		var description = opdDto.opd.Description;
        var createdUser = new UserService().GetUserByID(opdDto.opd.ModifiedUser).FullName;

        _OPDDto.opdId = Id;
		_OPDDto.name = name;
		_OPDDto.age = age;
		_OPDDto.months = months;
		_OPDDto.days = days;
		_OPDDto.sex = sex;
		_OPDDto.phone = phone;
		_OPDDto.TotalAmount = totalAmount;
		_OPDDto.ConsultantName = consultant.Name;
		_OPDDto.RoomName = roomNumber;
		_OPDDto.appoinmentNo = appointmentNumber;
		_OPDDto.Description = description;
        _OPDDto.CreatedUserId = opdDto.opd.ModifiedUser;
        _OPDDto.CreatedUserName = createdUser;

        return PartialView("_PartialQR", _OPDDto);
	}
}