using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    public class Course_Lectures
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price {  get; set; }
        public bool? PrequestiesExam {  get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("Course")]
        public int CourseId {  get; set; }
        public Course Course { get; set; }
        public List<Lecture_Videos> Videos { get; set; } = new List<Lecture_Videos>();
        public List<Lecture_Documents> Documents { get; set; } = new List<Lecture_Documents>();
        public List<Lecture_Exams> Exams { get; set; } = new List<Lecture_Exams>();
        public List<Student_Exams> StudentExams { get; set; } = new List<Student_Exams>();
    }
}
