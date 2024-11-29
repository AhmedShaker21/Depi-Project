using Microsoft.AspNetCore.Mvc;
using ELearningPlatform.View_Models;
using ELearningPlatform.Models;
using ELearningPlatform.Repositery;
using Microsoft.AspNetCore.Identity;
using ELearningPlatForm.Repositery;

namespace ELearningPlatform.Controllers
{
    public class AuthanticationController : Controller
    {
        private readonly IStudentRepo _studentRepo;
        private readonly IinstructorRepo _instructorRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthanticationController(IinstructorRepo Instructor, IStudentRepo Student, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _studentRepo = Student;
            _instructorRepo = Instructor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult SignUp_Config() => View();

        [HttpPost]
        public IActionResult User_Kind(string role)
        {
            if (role == "Instructor")
                return View("Instructor_RegisterData", new InstructorAccount());

            return View("Student_RegisterData", new StudentAccount());
        }

        #region Instructor_SignUp
        [HttpPost]
        public IActionResult Instructor_RegisterData(InstructorAccount? instructor)
        {

            return View("InstructorSignUp", instructor);

        }

        [HttpPost]
        public async Task<IActionResult> Instructor_Register(InstructorAccount? instructor)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = instructor.UserName,
                    Email = instructor.Email,
                    PhoneNumber = instructor.PhoneNumber,
                    PasswordHash = instructor.Password
                };
                var newInstructor = new Instructor
                {
                    Name = instructor.Name,
                    Country = instructor.Country,
                    Gender = instructor.Gender,
                    City = instructor.City,
                    Street = instructor.Street
                };
                var result = await _instructorRepo.Add_Instructor(newInstructor, user);
                if (result.Succeeded)
                {
                    _instructorRepo.Save();
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var ItemError in result.Errors)
                    {
                        ModelState.AddModelError("Password", ItemError.Description);
                    }
                    return View("instructorSignUP", instructor);
                }


            }
            return View("instructorSignUP", instructor);
        }
        #endregion

        #region Student_SignUP
        [HttpPost]
        public IActionResult Student_Register_Data(StudentAccount? student)
        {
            return View("Student_signup", student);
        }
        //[HttpPost]
        //public async Task<IActionResult> Student_Register(StudentAccount? student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = student.UserName,
        //            Email = student.Email,
        //            PhoneNumber = student.PhoneNumber,
        //            PasswordHash = student.Password
        //        };
        //        var newStudent = new Student
        //        {
        //            Name = student.Name,
        //            Country = student.Country,
        //            Gender = student.Gender,
        //            City = student.City,
        //            Street = student.Street
        //        };
        //        var result = await _studentRepo.Add_Student(newStudent, user);
        //        if (result.Succeeded)
        //        {
        //            _instructorRepo.Save();
        //            await _signInManager.SignInAsync(user, false);
        //            await _userManager.AddToRoleAsync(user, "Instructor");
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            foreach (var ItemError in result.Errors)
        //            {
        //                ModelState.AddModelError("Password", ItemError.Description);
        //            }
        //            return View("Student_signup", student);
        //        }


        //    }
        //    return View("Student_signup", student);

        //}
        [HttpPost]
        public async Task<IActionResult> Student_Register(StudentAccount? student)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = student.UserName,
                    Email = student.Email,
                    PhoneNumber = student.PhoneNumber,
                    PasswordHash = student.Password
                };
                var newStudent = new Student
                {
                    Name = student.Name,
                    Country = student.Country,
                    Gender = student.Gender,
                    City = student.City,
                    Street = student.Street
                };
                var result = await _studentRepo.Add_Student(newStudent, user);
                if (result.Succeeded)
                {
                    _instructorRepo.Save();
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "Instructor");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var ItemError in result.Errors)
                    {
                        ModelState.AddModelError("Password", ItemError.Description);
                    }
                    return View("Student_signup", student);
                }


            }
            return View("Student_signup", student);

        }
        #endregion

        public IActionResult Login_View()
        {
            return View("User_Login");
        }

        [HttpPost]
        public async Task<IActionResult> User_Login(UserLogin userLoginVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(userLoginVM.UserName);
                if (user != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(user, userLoginVM.Password);
                    if (found)
                    {
                        await _signInManager.SignInAsync(user, userLoginVM.RememberMe);
                        return RedirectToAction("Index", "Home");   // Assuming you want to redirect to another view
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }
            return View("User_Login", userLoginVM);
        }

        public async Task<IActionResult> LogOut_View()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}