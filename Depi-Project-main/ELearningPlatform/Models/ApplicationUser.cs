using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    public class ApplicationUser:IdentityUser<int>
    {
        public Admin? Admin { get; set; }
        public Instructor? Instructor { get; set; }
        public Student? Student { get; set; }        
    }
}