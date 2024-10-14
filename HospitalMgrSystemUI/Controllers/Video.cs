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
                channellingScheduleStatus = ChannellingScheduleStatus.ALL,
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
                    // Extract and add the video using the service
                    var videoService = new VideoService();
                    videoService.AddVideo(videoUrl);

                    return View("Index");  // Redirect to a suitable page after success
                }
                catch (Exception ex)
                {
                    // Add error to the model state if an exception occurs
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // Return view again if validation fails or an error occurs
            return View();
        }
    }

}
