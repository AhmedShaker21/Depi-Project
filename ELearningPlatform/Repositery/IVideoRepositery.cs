using ELearningPlatform.Models;

namespace ELearningPlatform.Repositery
{
    public interface IVideoRepositery
    {
        Lecture_Videos GetVideoById(int id);
        void AddVideoToLecture(int id, IFormFile videoFile, string title);
        void DeleteVideo(int id);
    }
}
