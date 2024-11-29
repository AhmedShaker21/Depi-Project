using ELearningPlatform.Models;
using ELearningPlatForm.Repositery;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace ELearningPlatform.Repositery
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ELearningContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudentRepo(ELearningContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IdentityResult> Add_Student(Student student, ApplicationUser studentAccount)
        {
            _context.Students.Add(student);

            // Create the student account
            var result = await _userManager.CreateAsync(studentAccount,studentAccount.PasswordHash);
            if (result.Succeeded)
            {
                student.ApplicationUser_Id=studentAccount.Id;
            }
            return result;
        }


        public async Task Delete_Student(Student student, ApplicationUser studentAccount)
        {
            // Retrieve the ApplicationUser entity from the database to ensure it's fully loaded
            var studentAccountEntity = await _userManager.FindByIdAsync(studentAccount.Id.ToString());
            if (studentAccountEntity == null)
            {
                throw new InvalidOperationException("Student account not found.");
            }
            // Delete the user account associated with the student
            var deleteResult = await _userManager.DeleteAsync(studentAccountEntity);
            if (!deleteResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to delete the student account.");
            }
            // Remove the student entity from the context
            _context.Students.Remove(student);
        }
        public List<Student> Get_AllStudents()
        {
            return _context.Students.ToList();
        }
        public List<Student> Get_AllStudentsByCourseID(int courseID)
        {
            return (from student in _context.Students
                   join crs in _context.Course_Students
                   on student.Id equals crs.Student_ID
                   join course in _context.Courses
                   on crs.Course_ID equals course.Id
                   select student).ToList();
        }

        public Student Get_StudentByID(int id)
        {

            return (from student in _context.Students
                   where student.Id == id
                   select student).FirstOrDefault();
        }
        public ApplicationUser Get_Account(int ?id)
        {
            return (from account in _userManager.Users
                    where account.Id == id
                    select account
                    ).FirstOrDefault();
        }
        public void Update_Student(int id, Student student,ApplicationUser StudentAccount)
        {
            Student old_Data = _context.Students.Where(s => s.Id == id).FirstOrDefault();
            if (old_Data != null)
            {
                old_Data.Name = student.Name;
                old_Data.Street = student.Street;
                old_Data.City = student.City;
                old_Data.Country = student.Country;
            }
            ApplicationUser old_Acount=(from _student in _context.Students
                                       join studentAcc in _context.Users
                                       on _student.ApplicationUser_Id equals studentAcc.Id
                                       select studentAcc).FirstOrDefault();
            if (old_Acount != null)
            {
                old_Acount.Email= StudentAccount.Email;
                old_Acount.PasswordHash= StudentAccount.PasswordHash;
                old_Acount.PhoneNumber= StudentAccount.PhoneNumber;
                old_Acount.UserName= StudentAccount.UserName;
            }
            
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
