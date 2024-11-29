using System.ComponentModel.DataAnnotations.Schema;

namespace Faculty__MVC_.Models
{
    public class Course_Grade
    {
        public int Id { get; set; }
        public int CourseDegree { get; set; }
        [ForeignKey("Student")]
        public int Student_Id { get; set; }
        [ForeignKey("Course")]
        public int Course_Id { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
