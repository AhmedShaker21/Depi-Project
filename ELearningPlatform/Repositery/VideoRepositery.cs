using ELearningPlatform.Models;

namespace ELearningPlatform.Repositery
{
    public class VideoRepositery : IVideoRepositery
    {
        ELearningContext context;
        public VideoRepositery(ELearningContext context)
        {
            this.context = context;
        }
        public void AddVideoToLecture(int id, IFormFile videoFile, string title)
        {
            var lecture = context.Lectures.FirstOrDefault(c => c.Id == id);

            if (lecture != null && videoFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    videoFile.CopyTo(memoryStream);

                    var video = new Lecture_Videos
                    {
                        Title = title,
                        VideoData = memoryStream.ToArray(), // Store as byte array
                        ContentType = videoFile.ContentType, // Store MIME type
                        LectureId = lecture.Id
                    };

                    context.Videos.Add(video);
                    context.SaveChanges();
                }
            }
        }
        public Lecture_Videos GetVideoById(int id)
        {
            return context.Videos.FirstOrDefault(v => v.Id == id);
        }
        public void DeleteVideo(int id)
        {
            var video = GetVideoById(id);
            context.Videos.Remove(video);
            context.SaveChanges();
        }
    }
}
