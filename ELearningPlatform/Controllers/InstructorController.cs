using ELearningPlatform.Repositery;
using Microsoft.AspNetCore.Mvc;

namespace ELearningPlatform.Controllers
{
    public class InstructorController : Controller
    {
        IinstructorRepo instructorRepositery;
        public InstructorController(IinstructorRepo instructorRepositery)
        {
            this.instructorRepositery = instructorRepositery;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewDashboard(int id)
        {
            var instructor = instructorRepositery.Get_InstructorByID(id);
            return View(instructor);
        }
    }
}
