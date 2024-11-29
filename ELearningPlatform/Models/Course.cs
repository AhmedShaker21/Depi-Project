using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{

        [Table("Course")]
        public class Course
        {

            [Key]
            public int Id { get; set; }
            [Required(ErrorMessage = "Course name is required.")]
            public string Crs_Name { get; set; }
            public string? Crs_Description { get; set; }
            public decimal Crs_Price { get; set; }
            public string? Crs_Code { get; set; }
            public string? Crs_Catogery { get; set; }
            [NotMapped]
            public IFormFile? Crs_Cover { get; set; }
            public string? Crs_Cover_Path { get; set; }
            public List<Course_Lectures> Crs_Lectures { get; set; } = new List<Course_Lectures>();
            public List<Course_Students> Crs_Students { get; set; } = new List<Course_Students>();
            public List<Course_Codes> Crs_Codes { get; set;} = new List<Course_Codes>();
            [ForeignKey("Instructor")]
            public int InstructorId { get; set; }
            public Instructor Instructor {  get; set; } 
            public List<Student_Exams> StudentExams { get; set; } = new List<Student_Exams>();
    }
    }


