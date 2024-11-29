using ELearningPlatform.Models;
using ELearningPlatform.Repositery;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    public class CourseController : Controller
    {
        ICourseRepositery courseRepositery;
        public CourseController(ICourseRepositery courseRepositery) {
            this.courseRepositery = courseRepositery;
        }
        public IActionResult Index()
        {
            List<Course> courses = courseRepositery.GetAllCourses();
            return View(courses);
        }
        public IActionResult AddCourse(Course course)
        {
            if (string.IsNullOrWhiteSpace(course.Crs_Name))
            {
                ModelState.AddModelError("Crs_Name", "Course name is required.");
                return View(course);
            }

            courseRepositery.AddCourse(course);
            return RedirectToAction("Index");
        }
        public IActionResult AddVideoToCourse(int id)
        {
            Course course = courseRepositery.GetCourseById(id);

            var videoModel = new Lecture_Videos
            {
                LectureId = course.Id 
            };

            return View(videoModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddVideoToCourse(int id, string title, IFormFile VideoFile)
        {
            if (VideoFile != null && VideoFile.Length > 0)
            {
                try
                {
                    // Call repository method to save the video
                    courseRepositery.AddVideoToLecture(id, VideoFile, title);
                    return RedirectToAction("Index");
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


        public IActionResult AddDocumentToCourse(int id)
        {
            // Fetch the course using the course repository, if needed
            Course course = courseRepositery.GetCourseById(id);

            // Create a new Course_Videos object and initialize it with the CourseId
            var DocumentModel = new Lecture_Documents
            {
                LectureId = course.Id // Setting the CourseId in the video model
            };

            // Pass the Course_Videos object to the view
            return View(DocumentModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddDocumentToCourse(int id , Lecture_Documents documents)
        {
            var lecture = courseRepositery.GetLectureById(id);
            courseRepositery.AddDocumentToLecture(id, documents);
            return RedirectToAction("ViewLecture", new { id = lecture.Id });
        }
        public IActionResult GetCoursesByStudent()
        {
            List<Course> courses = courseRepositery.GetAllCourses();
            return View(courses);
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
                courseRepositery.AddLectureToCourse(id, lecture);
                return RedirectToAction("ViewLecture", new {id = lecture.Id});
        }
        public IActionResult ViewLecture(int id) 
        {
            Course_Lectures lecture = courseRepositery.GetLectureById(id);
            return View(lecture); 
        }
        public IActionResult GetAllLecturesByCourseByAdmin(int id) 
        {
            List<Course_Lectures> lectures = courseRepositery.GetAllLecturesByCourseByAdmin(id);
            return View(lectures);
        }
        public IActionResult GetVideoById(int id)
        {
            var video = courseRepositery.GetVideoById(id);
            if (video != null)
            {
                return File(video.VideoData, "video/mp4"); // Assuming VideoData is a byte array containing video content
            }
            return NotFound();
        }
        public IActionResult AddExamToLecture(int id)
        {
            var lecture = courseRepositery.GetLectureById(id);
            var exam = new Lecture_Exams
            {
                LectureId = lecture.Id
            }; 
            return View(exam);

    }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddExamToLecture(int id,Lecture_Exams exam) 
        {
            var lecture = courseRepositery.GetLectureById(id);
            courseRepositery.AddExamToLecture(id, exam);
            return RedirectToAction("AddQuestionsToExam", new { id = exam.Id });
        }
        public IActionResult AddQuestionsToExam(int id)
        {
            var exam = courseRepositery.GetExamById(id);
            var Questions = new List <Exam_Questions>
            {
                new Exam_Questions { ExamId = exam.Id }
            };
            return View(Questions);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddQuestionsToExam(int id , List<Exam_Questions> examQuestions)
        {
            var exam = courseRepositery.GetExamById(id);
            courseRepositery.AddQuestionsToExam(id, examQuestions);
            return RedirectToAction("index");
        }
        public IActionResult GetExamById(int id)
        {
            var exam = courseRepositery.GetExamById(id);
            if(exam.ExamQuestions.Count == 0)
            {
                return RedirectToAction("AddQuestionsToExam", new { id = exam.Id });
            }
            return View(exam);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateExam(int id , Lecture_Exams exam)
        {
            courseRepositery.UpdateExam(id, exam);
            return RedirectToAction("index");
        }
        public IActionResult DeleteExamById(int id) 
        {
            courseRepositery.DeleteExam(id);
            return RedirectToAction("index");
        }
        public IActionResult DeleteVideoById(int id)
        {
            courseRepositery.DeleteVideo(id);
            return RedirectToAction("index");
        }
        public IActionResult DeleteDocumentById(int id)
        {
            courseRepositery.DeleteDocumentFromLecture(id);
            return RedirectToAction("index");
        }
    }
}
