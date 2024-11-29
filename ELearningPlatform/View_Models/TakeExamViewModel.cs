using ELearningPlatform.Models;

namespace ELearningPlatform.View_Models
{
    public class TakeExamViewModel
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public List<Exam_Questions> Questions { get; set; }
    }
}
