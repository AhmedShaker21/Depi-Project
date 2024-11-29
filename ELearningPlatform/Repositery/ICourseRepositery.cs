
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
        void RegisterCourse(int StudentId, int CourseId, string codename);
        void AddCoverPhotoToCourse();
        void UpdateCoverPhotoToCourse();
        List<Course> GetCoursesByInstructor(int instructorId);


    }
}
