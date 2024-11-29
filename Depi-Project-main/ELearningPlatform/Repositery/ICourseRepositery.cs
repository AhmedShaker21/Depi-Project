
using ELearningPlatform.Models;

namespace ELearningPlatform.Repositery

{
    public interface ICourseRepositery
    {
        List<Course> GetAllCourses();
        Course GetCourseById(int id);
        void AddCourse(Course course);
        void UpdateCourse(int id,Course course);
        void DeleteCourse(int id);
        void ResgisterCourse(int StudentId , int CourseId);
        Course_Lectures GetLectureById(int id);
        List<Course_Lectures> GetAllLecturesByCourseByAdmin(int id);
        Lecture_Videos GetVideoById(int id);
        void AddDocumentToLecture(int id , Lecture_Documents documents);
        void DeleteDocumentFromLecture(int id);
        Lecture_Documents GetDocumentById(int id);
        void AddCoverPhotoToCourse();
        void UpdateCoverPhotoToCourse();
        void AddLectureToCourse(int id , Course_Lectures lecture);
        void DeleteLecture(int id);
        void UpdateLecture(int id, Course_Lectures lecture);
        void AddVideoToLecture(int id, IFormFile videoFile, string title);
        void DeleteVideo(int id);
        void AddExamToLecture(int id,Lecture_Exams exam);
        void UpdateExam(int id, Lecture_Exams exam);
        void DeleteExam(int id);
        Lecture_Exams GetExamById(int id);
        void AddQuestionsToExam(int id, List<Exam_Questions> examQuestions);


    }
}
