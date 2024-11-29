using ELearningPlatform.Models;

namespace ELearningPlatform.View_Models
{
    public class ExamResultViewModel
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public List<ExamResultItemViewModel> Results { get; set; } // Change here

    }
}
