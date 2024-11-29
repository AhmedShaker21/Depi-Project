using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    public class Course_Students
    {
        [ForeignKey("Student")]
        public int Student_ID { get; set;}
        [ForeignKey("Course")]
        public int Course_ID { get; set;}
        [ForeignKey("Code")]
        public int Code_ID {  get; set;}
        public Student Student { get; set;}
        public Course Course { get; set;}
        public Course_Codes Code { get; set;}
    }
}
