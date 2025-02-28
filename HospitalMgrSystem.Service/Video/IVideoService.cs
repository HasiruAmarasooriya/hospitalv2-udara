namespace HospitalMgrSystem.Service.User
{
    public interface IVideoService
    {
        string GetLeastVideoId();
        void AddVideo(string videoUrl);
    }
}
