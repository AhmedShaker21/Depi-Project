using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    public class Student_Exams
    {
        [ForeignKey("Student")]
        public int StudentId {  get; set; }
        [ForeignKey("Exam")]
        public int ExamId {  get; set; }
        [ForeignKey("Course")]
        public int CourseId {  get; set; }
        [ForeignKey("Lecture")]
        public int LectureId { get; set; }
        public int? Grade { get; set; }
        public DateTime? Date { get; set; }
        public Student Student { get; set; }
        public Lecture_Exams Exam { get; set; }
        public Course Course { get; set; }
        public Course_Lectures Lecture { get; set; }

    }
}
