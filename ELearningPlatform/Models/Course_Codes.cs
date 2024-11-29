using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    public class Course_Codes
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [ForeignKey("Course")]
        public int CourseId {  get; set; }
        public Course Course { get; set; }
    }
}
