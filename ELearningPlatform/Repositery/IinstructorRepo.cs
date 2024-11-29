using ELearningPlatform.Models;
using Microsoft.AspNetCore.Identity;

namespace ELearningPlatform.Repositery
{
    public interface IinstructorRepo
    {
       public  Task<IdentityResult>Add_Instructor(Instructor INstructor, ApplicationUser InstructorAccount);
        public void Update_Instructor(int id, Instructor Instructor, ApplicationUser InstructorAccount);
        public Task Delete_Instructor(Instructor instructor, ApplicationUser IsntructorAccount);
        public Instructor Get_InstructorByID(int id);
        public ApplicationUser Get_Account(int? id);
        public List<Instructor> Get_AllInstructors();
        public Task Save();
    }
}
