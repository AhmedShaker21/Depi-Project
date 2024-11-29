using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    public class Exam_Questions
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public string AnswerOne {  get; set; }
        public string AnswerTwo { get; set; }
        public string? AnswerThree { get; set; }
        public string? AnswerFour { get; set; }
        public string CorrectAnswer {  get; set; }
        [ForeignKey("Exam")]
        public int ExamId {  get; set; }
        public Lecture_Exams Exam { get; set; }
    }
}
