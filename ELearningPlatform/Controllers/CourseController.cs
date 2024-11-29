using ELearningPlatform.Models;
using ELearningPlatform.Repositery;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    public class CourseController : Controller
    {
        ICourseRepositery courseRepositery;
        IinstructorRepo instructorRepositery;
        public CourseController(ICourseRepositery courseRepositery, IinstructorRepo instructorRepositery)
        {
            this.courseRepositery = courseRepositery;
            this.instructorRepositery = instructorRepositery;
        }
        public IActionResult Index()
        {
            List<Course> courses = courseRepositery.GetAllCourses();
            return View(courses);
        }
        public IActionResult AddCourse(int id, Course course)
        {
            var intructor = instructorRepositery.Get_InstructorByID(id);
            if (string.IsNullOrWhiteSpace(course.Crs_Name))
            {
                ModelState.AddModelError("Crs_Name", "Course name is required.");
                return View(course);
            }
            var coursee = new Course
            {
                Crs_Name = course.Crs_Name,
                Crs_Code = course.Crs_Code,
                Crs_Cover_Path = course.Crs_Cover_Path,
                Crs_Cover = course.Crs_Cover,

                Crs_Description = course.Crs_Description,
                Crs_Price = course.Crs_Price,
                InstructorId = id
            };
            courseRepositery.AddCourse(coursee);
            return RedirectToAction("Index");
        }
        public IActionResult AddCourseByInstructor(int id,Course course)
        {
            var intructor = instructorRepositery.Get_InstructorByID(id);
            if (string.IsNullOrWhiteSpace(course.Crs_Name))
            {
                ModelState.AddModelError("Crs_Name", "Course name is required.");
                return View(course);
            }
            var coursee = new Course
            {
                Crs_Name = course.Crs_Name,
                Crs_Code = course.Crs_Code,
                Crs_Cover_Path = course.Crs_Cover_Path,
                Crs_Cover = course.Crs_Cover,
                
                Crs_Description = course.Crs_Description,
                Crs_Price = course.Crs_Price,
                InstructorId = id
            };
            courseRepositery.AddCourse(coursee);
            return RedirectToAction("GetCoursesByInstructor");
        }




        public IActionResult GetCoursesByStudent()
        {
            List<Course> courses = courseRepositery.GetAllCourses();
            return View(courses);
        }




        public IActionResult ViewCoursesByStudent(int id)
        {
            ViewBag.StudentId = id;
            var courses = courseRepositery.GetAllCourses();
            return View(courses);
        }
        public IActionResult ViewCourseByStudent(int id)
        {
            var course = courseRepositery.GetCourseById(id);
            return View(course);
        }
        public IActionResult RegisterCourse(int StudentId,int CourseId)
        {
            var course = courseRepositery.GetCourseById(CourseId);
            ViewBag.StudentId = StudentId;
            return View(course);
        }
        [HttpPost]
        public IActionResult RegisterCourse(int StudentId, int CourseId , string codename)
        {
            courseRepositery.RegisterCourse(StudentId, CourseId, codename);
            return RedirectToAction("Index");
        }
        public IActionResult UpdateCourse(int id)
        {
            var course = courseRepositery.GetCourseById(id);
            return View(course);
        }
        [HttpPost]
        public IActionResult UpdateCourse(int id , Course course)
        {
            courseRepositery.UpdateCourse(id, course);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteCourse(int id) {  courseRepositery.DeleteCourse(id); return RedirectToAction("Index"); }
        public IActionResult GetCoursesByInstructor(int id)
        {
            var courses = courseRepositery.GetCoursesByInstructor(id);
            return View(courses);
        }

    }
}
