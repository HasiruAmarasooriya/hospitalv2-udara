using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.CashierSession;
using HospitalMgrSystem.Service.User;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HospitalMgrSystemUI.Controllers
{
    public class CashierSessionsController : Controller
    {

        [BindProperty]
        public CashierSessionDto viewCashierSessionDto { get; set; }
        public IActionResult Index()
        {
            CashierSessionDto cashierSessionDto = new CashierSessionDto();
            cashierSessionDto.CashierSessions = GetAllCashierSession();
            return View(cashierSessionDto);
        }

        public ActionResult DownloadCashierPayment([FromBody] CashierSessionDto cashierSessionDtoData)
        {
            
            CashierSessionDto cashierSessionDto = new CashierSessionDto();
            CashierSessionService cashierSessionService = new CashierSessionService();

            cashierSessionDto.CashierPaymentData= cashierSessionService.GetCashierSessionPaymentData(cashierSessionDtoData.sessionId);
            cashierSessionDto.cashierSessionID = cashierSessionDtoData.sessionId;
            cashierSessionDto.sessionDate = DateTime.Now;

            return PartialView("_PartialViewSummary", cashierSessionDto);
        }


        public ActionResult CreateCashierSessionse(int id)
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            CashierSessionDto cashierSessionDto = new CashierSessionDto();
            int userID = Convert.ToInt32(userIdCookie);
            if (id > 0)
            {
                cashierSessionDto.cashierSession = GetCashierSessionById(id);
                cashierSessionDto.sessionDate = cashierSessionDto.cashierSession.StartingTime;
                cashierSessionDto.user = cashierSessionDto.cashierSession.User;
                return PartialView("_PartialAddCashierSession", cashierSessionDto);
            }
            else
            {
                CashierSession cashierSession= new CashierSession();
                cashierSessionDto.cashierSession = cashierSession;
                cashierSessionDto.cashierSession.cashierSessionStatus = CashierSessionStatus.START;
                cashierSessionDto.sessionDate = DateTime.Now;
                cashierSessionDto.user = GetUserById(userID);
                return PartialView("_PartialAddCashierSession", cashierSessionDto);
            }
        }

        public IActionResult CreateNewCashierSession()
        {
            CashierSession cashierSession = new CashierSession();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
                    if (viewCashierSessionDto != null)
                    {
                        if (viewCashierSessionDto.cashierSession.Id != 0)
                        {

                                viewCashierSessionDto.cashierSession.userID = Convert.ToInt32(userIdCookie);
                                viewCashierSessionDto.cashierSession.EndTime = DateTime.Now;
                                viewCashierSessionDto.cashierSession.ModifiedUser = Convert.ToInt32(userIdCookie);
                                viewCashierSessionDto.cashierSession.ModifiedDate = DateTime.Now;
                                viewCashierSessionDto.cashierSession.Deviation = 0;
                                viewCashierSessionDto.cashierSession.cashierSessionStatus = CashierSessionStatus.END;
                                viewCashierSessionDto.cashierSession.UserRole = UserRole.CASHIER;
                                 cashierSession = new CashierSessionService().CreateCashierSession(viewCashierSessionDto.cashierSession);
                           

                        }
                        else
                        {
                            List<CashierSession> mtList = new List<CashierSession>();
                            mtList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));
                            if (mtList.Count == 0)
                            {

                            viewCashierSessionDto.cashierSession.userID = Convert.ToInt32(userIdCookie);
                            viewCashierSessionDto.cashierSession.StartingTime = DateTime.Now;
                            viewCashierSessionDto.cashierSession.EndTime = DateTime.Now;
                            viewCashierSessionDto.cashierSession.CreateUser = Convert.ToInt32(userIdCookie);
                            viewCashierSessionDto.cashierSession.CreateDate = DateTime.Now;
                            viewCashierSessionDto.cashierSession.cashierSessionStatus = CashierSessionStatus.START;
                            viewCashierSessionDto.cashierSession.ModifiedUser = Convert.ToInt32(userIdCookie);
                            viewCashierSessionDto.cashierSession.ModifiedDate = DateTime.Now;
                            viewCashierSessionDto.cashierSession.UserRole = UserRole.CASHIER;
                            cashierSession = new CashierSessionService().CreateCashierSession(viewCashierSessionDto.cashierSession);

                            }
                        }

                       
                    }
                   
                }
                catch (Exception ex)
                {

                }
                return RedirectToAction("Index");

            }
        }

        private List<CashierSession> GetAllCashierSession()
        {
            List<CashierSession> CashierSessionList = new List<CashierSession>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    CashierSessionList = new CashierSessionService().GetAllCashierSession();

                }
                catch (Exception ex) { }
            }
            return CashierSessionList;
        }
        private List<CashierSession> GetActiveCashierSession(int id)
        {
            List<CashierSession> CashierSessionList = new List<CashierSession>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    CashierSessionList = new CashierSessionService().GetACtiveCashierSessions(id);

                }
                catch (Exception ex) { }
            }
            return CashierSessionList;
        }
        private CashierSession GetCashierSessionById(int id)
        {
            CashierSession CashierSession = new CashierSession();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    CashierSession = new CashierSessionService().GetCashierSessionByID(id);

                }
                catch (Exception ex) { }
            }
            return CashierSession;
        }

        private User GetUserById(int id)
        {
            User user = new User();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    user = new UserService().GetUserByID(id);

                }
                catch (Exception ex) { }
            }
            return user;
        }
    }
}
