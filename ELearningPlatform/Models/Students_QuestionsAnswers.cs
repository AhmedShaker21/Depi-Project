using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    public class Students_QuestionsAnswers
    {
        public int Id { get; set; }

        [ForeignKey("ExamQuestion")]
        public int ExamQuestionId { get; set; }
        public Exam_Questions ExamQuestion { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; } // Assuming you have a Student model

        public string SelectedAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
