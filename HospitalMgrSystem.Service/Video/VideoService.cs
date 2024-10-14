using System.Security.Cryptography;
using System.Text;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Service.User
{
    public class VideoService : IVideoService
    {
        public string GetLeastVideoId()
        {
            using (var dbContext = new DataAccess.HospitalDBContext())
            {
                return dbContext.Video
            .OrderByDescending(v => v.Id) 
            .Select(v => v.YoutubeId)
            .FirstOrDefault(); 
            }
        }

        public void AddVideo(string videoUrl)
        {
           
            string youtubeId = ExtractYouTubeId(videoUrl);
            if (string.IsNullOrEmpty(youtubeId))
            {
                throw new ArgumentException("Invalid YouTube URL.");
            }

            using (var dbContext = new DataAccess.HospitalDBContext())
            {
               
                var existingVideo = dbContext.Video.SingleOrDefault(v => v.YoutubeId == youtubeId);

                if (existingVideo != null)
                {
                  
                    existingVideo.ModifiedDate = DateTime.Now;

                   
                    dbContext.Video.Update(existingVideo);
                }
                else
                {
                    
                    var newVideo = new Video
                    {
                        YoutubeId = youtubeId,
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                   
                    dbContext.Video.Add(newVideo);
                }

                dbContext.SaveChanges();
            }
        }

        
        private string ExtractYouTubeId(string videoUrl)
        {
          
            var uri = new Uri(videoUrl);
            if (uri.Host.Contains("youtube.com"))
            {
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                return query["v"]; 
            }
            else if (uri.Host.Contains("youtu.be"))
            {
                return uri.Segments.Last(); 
            }

            return null; 
        }

    }
}
