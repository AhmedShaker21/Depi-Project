using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    public class Admin
    {
        [Key]
        public  int Id { get; set; }
        public string Name { get; set; }
        public string Phone_Number { get; set; }
        public string Email { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public string? country { get; set; }
        public string ?Salary { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public int ?ApplicationUser_Id { get; set; }
        public ApplicationUser? User { get; set; }
    }
}