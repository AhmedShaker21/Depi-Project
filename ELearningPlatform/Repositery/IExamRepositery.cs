using ELearningPlatform.Models;

namespace ELearningPlatform.Repositery
{
    public interface IExamRepositery
    {
        void AddExamToLecture(int id, Lecture_Exams exam);
        void UpdateExam(int id, Lecture_Exams exam);
        void DeleteExam(int id);
        Lecture_Exams GetExamById(int id);
        void AddQuestionsToExam(int id, List<Exam_Questions> examQuestions);
        void SubmitExam(int studentId, int examId, Dictionary<int, string> selectedAnswers);
        List<Students_QuestionsAnswers> GetExamResults(int studentId, int examId);
        void SaveStudentExam(Student_Exams studentExam);
        void SaveStudentGrade(int studentId, int examId, int grade);
        Student_Exams? GetStudentExam(int studentId, int examId);
    }
}
