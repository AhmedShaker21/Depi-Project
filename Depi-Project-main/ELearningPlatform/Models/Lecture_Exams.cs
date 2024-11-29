using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    public class Lecture_Exams
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        [ForeignKey("Lecture")]
        public int LectureId { get; set; }
        public Course_Lectures Lecture { get; set; }
        public List<Exam_Questions> ExamQuestions { get; set; } = new List<Exam_Questions>();

    }
}
