using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.Channeling;
using HospitalMgrSystem.Service.ChannelingSchedule;
using HospitalMgrSystem.Service.User;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class Video : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Live()
        {
            ChannelingSheduleDto channelingSheduleDto = new ChannelingSheduleDto()
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(1),
                Specialists = new ChannelingService().GetAllSpecialists(),
                channellingScheduleStatus = ChannellingScheduleStatus.ACTIVE,
                VideoId = new VideoService().GetLeastVideoId()
            };

            using (var httpClient = new HttpClient())
            {
                try
                {

                    channelingSheduleDto.ChannelingScheduleList = new ChannelingScheduleService().SheduleGetBySelected(DateTime.Today);
                    return View(channelingSheduleDto);
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
        }
       

        public IActionResult Create(string videoUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                 
                    var videoService = new VideoService();
                    videoService.AddVideo(videoUrl);

                    return View("Index");  
                }
                catch (Exception ex)
                {
                   
                    ModelState.AddModelError("", ex.Message);
                }
            }

       
            return View();
        }
    }

}
