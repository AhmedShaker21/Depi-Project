using ELearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Repositery
{
    public class LectureRepositery : ILectureRepositery
    {
        ELearningContext context;
        public LectureRepositery(ELearningContext context) {
            this.context = context;
        }
        public void AddLectureToCourse(int id, Course_Lectures lecture)
        {
            var course = context.Courses.FirstOrDefault(c => c.Id == id);
            lecture.CourseId = course.Id;
            lecture.Id = 0;  // Ensure Id is not explicitly set
            lecture.Date = DateTime.Now.Date;  // Set the date to the current date

            context.Lectures.Add(lecture);  // Add the lecture to the database
            context.SaveChanges();
        }
        public Course_Lectures GetLectureById(int id)
        {
            return context.Lectures.Include(l => l.Documents).Include(l => l.Videos).Include(l => l.Exams).FirstOrDefault(l => id == l.Id);

        }
        public List<Course_Lectures> GetAllLecturesByCourseByAdmin(int id)
        {
            Course course = context.Courses.FirstOrDefault(l => l.Id == id);
            return context.Lectures.Where(c => c.CourseId == course.Id).ToList();
        }
        public void UpdateLecture(int id, Course_Lectures lecture)
        {
            throw new NotImplementedException();
        }
        public void DeleteLecture(int id)
        {
            var lecture = GetLectureById(id);
            context.Lectures.Remove(lecture);
            context.SaveChanges();
        }

    }
}
