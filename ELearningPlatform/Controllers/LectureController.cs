using ELearningPlatform.Models;
using ELearningPlatform.Repositery;
using Microsoft.AspNetCore.Mvc;

namespace ELearningPlatform.Controllers
{
    public class LectureController : Controller
    {
        ICourseRepositery courseRepositery;
        ILectureRepositery lectureRepositery;
        public LectureController(ICourseRepositery courseRepositery, ILectureRepositery lectureRepositery)
        {
            this.courseRepositery = courseRepositery;
            this.lectureRepositery = lectureRepositery;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddLectureToCourse(int id)
        {
            Course course = courseRepositery.GetCourseById(id);
            var LectureModel = new Course_Lectures
            {
                Id = course.Id // Setting the CourseId in the video model
            };
            return View(LectureModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddLectureToCourse(int id, Course_Lectures lecture)
        {
            lectureRepositery.AddLectureToCourse(id, lecture);
            return RedirectToAction("ViewLecture", new { id = lecture.Id });
        }
        public IActionResult ViewLecture(int id)
        {
            Course_Lectures lecture = lectureRepositery.GetLectureById(id);
            return View(lecture);
        }
        public IActionResult GetAllLecturesByCourseByAdmin(int id)
        {
            List<Course_Lectures> lectures = lectureRepositery.GetAllLecturesByCourseByAdmin(id);
            return View(lectures);
        }
        public IActionResult ViewLectureByStudent(int id)
        {
            Course_Lectures lecture = lectureRepositery.GetLectureById(id);
            return View(lecture);
        }

    }
}
