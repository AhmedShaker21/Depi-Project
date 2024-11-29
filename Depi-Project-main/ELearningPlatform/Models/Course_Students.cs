using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    [Table("Course Result")]
    [PrimaryKey(nameof(Course_ID), nameof(Student_ID))]
    public class Course_Students
    {
        public int Student_ID { get; set;}
        public int Course_ID { get; set;}
        public Student? Student { get; set;}
        public Course? Course { get; set;}
    }
}
