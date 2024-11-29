using ELearningPlatform.Models;
using ELearningPlatform.Repositery;
using Microsoft.AspNetCore.Mvc;

namespace ELearningPlatform.Controllers
{
    public class VideoController : Controller
    {
        ICourseRepositery courseRepositery;
        IVideoRepositery videoRepositery;
        ILectureRepositery lectureRepositery;
        public VideoController(ICourseRepositery courseRepositery, IVideoRepositery videoRepositery, ILectureRepositery lectureRepositery)
        {
            this.courseRepositery = courseRepositery;
            this.videoRepositery = videoRepositery;
            this.lectureRepositery = lectureRepositery;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddVideoToLecture(int id)
        {
            Course_Lectures course = lectureRepositery.GetLectureById(id);

            var videoModel = new Lecture_Videos
            {
                LectureId = course.Id
            };

            return View(videoModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddVideoToLecture(int id, string title, IFormFile VideoFile)
        {
            var lecture = lectureRepositery.GetLectureById(id);
            if (VideoFile != null && VideoFile.Length > 0)
            {
                try
                {
                    // Call repository method to save the video
                    videoRepositery.AddVideoToLecture(id, VideoFile, title);
                    return RedirectToAction("ViewLecture", "Lecture", new { id = lecture.Id });
                }
                catch (Exception ex)
                {
                    ViewData["FileError"] = "An error occurred while uploading the video: " + ex.Message;
                }
            }
            else
            {
                ViewData["FileError"] = "Please select a valid video file.";
            }

            return View();
        }
        public IActionResult GetVideoById(int id)
        {
            var video = videoRepositery.GetVideoById(id);
            if (video != null)
            {
                // Create a memory stream from the byte array
                var stream = new MemoryStream(video.VideoData);

                // Return the video content as a FileStreamResult
                return new FileStreamResult(stream, video.ContentType);
            }
            return NotFound();
        }
        public IActionResult DeleteVideoById(int id)
        {
            var video = videoRepositery.GetVideoById(id);
            var lecture = lectureRepositery.GetLectureById(video.LectureId);
            videoRepositery.DeleteVideo(id);
            return RedirectToAction("ViewLecture", "Lecture", new { id = lecture.Id });
        }
    }
}
