using ELearningPlatform.Models;
using ELearningPlatform.Repositery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELearningPlatform.Controllers
{
    public class CodeController : Controller
    {
        ICodeRepositery codeRepositery;
        ICourseRepositery courseRepositery;
        public CodeController(ICodeRepositery codeRepositery, ICourseRepositery courseRepositery) {
            this.codeRepositery = codeRepositery;
            this.courseRepositery = courseRepositery;
        }
        public IActionResult Index()
        {
            var codes = codeRepositery.GetAllCodes();
            return View();
        }
        public IActionResult GetCourseById(int id)
        {
            var code = codeRepositery.GetCodeById(id);
            return Ok(new { codeId = code.Id });
        }
        // GET: Display the AddCode form
        public IActionResult AddCode()
        {
            // Ensure the repository returns a valid list of courses
            var courses = courseRepositery.GetAllCourses();
            if (courses == null || !courses.Any())
            {
                // Handle the scenario where no courses are found
                ViewBag.Courses = new List<Course>(); // Empty list instead of null
            }
            else
            {
                ViewBag.Courses = courses;
            }

            return View();
        }

        // POST: Handle the form submission
        [HttpPost]
        public IActionResult AddCode(Course_Codes code)
        {
            if (!ModelState.IsValid)
            {
                // If model validation fails, return the form with validation messages
                ViewBag.Courses = new SelectList(courseRepositery.GetAllCourses(), "Id", "Crs_Name");
                return View(code);
            }

            try
            {
                // Make sure the 'Code' property is not empty
                if (string.IsNullOrWhiteSpace(code.Code))
                {
                    ModelState.AddModelError("Code", "Code is required.");
                    ViewBag.Courses = new SelectList(courseRepositery.GetAllCourses(), "Id", "Crs_Name");
                    return View(code);
                }

                // Save the code to the repository
                codeRepositery.AddCode(code);
                return RedirectToAction("Index"); // Or any other action after saving
            }
            catch (Exception ex)
            {
                // Log the error and return an error message to the user
                ViewBag.ErrorMessage = "An error occurred while saving the code.";
                ViewBag.Courses = new SelectList(courseRepositery.GetAllCourses(), "Id", "Crs_Name");
                return View(code);
            }
        }




    }
}
