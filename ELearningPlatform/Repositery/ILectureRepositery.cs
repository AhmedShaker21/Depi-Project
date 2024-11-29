using ELearningPlatform.Models;

namespace ELearningPlatform.Repositery
{
    public interface ILectureRepositery
    {
        Course_Lectures GetLectureById(int id);
        List<Course_Lectures> GetAllLecturesByCourseByAdmin(int id);
        void AddLectureToCourse(int id, Course_Lectures lecture);
        void DeleteLecture(int id);
        void UpdateLecture(int id, Course_Lectures lecture);
    }
}
