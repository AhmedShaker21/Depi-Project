using ELearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Repositery
{
    public class CourseRepositery : ICourseRepositery
    {
        ELearningContext context;
        IWebHostEnvironment env;
        public CourseRepositery(ELearningContext _context , IWebHostEnvironment env)
        {
            context = _context;
            this.env = env;
        }
        public List<Course>GetAllCourses()
        {
            return context.Courses.ToList();
        }
        public void AddCourse(Course course)
        {
            if(course != null)
            {
                if (course.Crs_Cover != null)
                {
                    string ImageFolder = Path.Combine(env.WebRootPath, "img");
                    string ImagePath = Path.Combine(ImageFolder, course.Crs_Cover.FileName);
                    course.Crs_Cover.CopyTo(new FileStream(ImagePath, FileMode.Create));
                    course.Crs_Cover_Path = course.Crs_Cover.FileName;
                }
                context.Courses.Add(course);
                context.SaveChanges();
            }
        }

        public void AddCoverPhotoToCourse()
        {
            throw new NotImplementedException();
        }

        public void AddDocumentToLecture(int id, Lecture_Documents documents)
        {
            // Check if the course exists in the database
            Course_Lectures lecture = context.Lectures.FirstOrDefault(c => c.Id == id);
            if (lecture == null)
            {
                throw new Exception("Course not found.");
            }

            // Check if the uploaded document is not null
            if (documents.Document != null && documents.Document.Length > 0)
            {
                // Define the directory to store the uploaded document
                string documentFolder = Path.Combine(env.WebRootPath, "documents");

                // Ensure the folder exists
                if (!Directory.Exists(documentFolder))
                {
                    Directory.CreateDirectory(documentFolder);
                }

                // Generate a unique file name to avoid overwriting
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(documents.Document.FileName);
                string documentPath = Path.Combine(documentFolder, uniqueFileName);

                // Save the document to the specified path
                using (var fileStream = new FileStream(documentPath, FileMode.Create))
                {
                    documents.Document.CopyTo(fileStream);
                }

                // Store the relative path in the database
                Lecture_Documents document = new Lecture_Documents
                {
                    LectureId = lecture.Id,
                    FilePath = Path.Combine("documents", uniqueFileName),
                    Title = documents.Title,
                };

                // Add document to the context
                context.Documents.Add(document);

                // Save changes to the database
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Document not uploaded.");
            }
        }

        public void AddVideoToLecture(int id, IFormFile videoFile, string title)
        {
            var lecture = context.Lectures.FirstOrDefault(c => c.Id == id);

            if (lecture != null && videoFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    videoFile.CopyTo(memoryStream);

                    var video = new Lecture_Videos
                    {
                        Title = title,
                        VideoData = memoryStream.ToArray(), // Store as byte array
                        ContentType = videoFile.ContentType, // Store MIME type
                        LectureId = lecture.Id
                    };

                    context.Videos.Add(video);
                    context.SaveChanges();
                }
            }
        }


        public void DeleteCourse(int id)
        {
            Course course = context.Courses.FirstOrDefault(c=>c.Id == id);
            context.Courses.Remove(course);
            context.SaveChanges();
        }

        public Course GetCourseById(int id)
        {
            return context.Courses.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateCourse(int id, Course course)
        {
            throw new NotImplementedException();
        }

        public void UpdateCoverPhotoToCourse()
        {
            throw new NotImplementedException();
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
            return context.Lectures.Include(l=>l.Documents).Include(l=>l.Videos).Include(l=>l.Exams).FirstOrDefault(l=>id == l.Id);

        }
        public List<Course_Lectures> GetAllLecturesByCourseByAdmin(int id)
        {
            Course course = context.Courses.FirstOrDefault(l=>l.Id == id);
            return context.Lectures.Where(c=>c.CourseId == course.Id).ToList();
        }
        public Lecture_Videos GetVideoById(int id)
        {
            return context.Videos.FirstOrDefault(v=>v.Id == id);
        }
        public void DeleteVideo(int id)
        {
            var video = GetVideoById(id);
            context.Videos.Remove(video);
            context.SaveChanges();
        }
        void ICourseRepositery.UpdateLecture(int id, Course_Lectures lecture)
        {
            throw new NotImplementedException();
        }

        public void DeleteLecture(int id)
        {
            var lecture = GetLectureById(id);
            context.Lectures.Remove(lecture);
            context.SaveChanges();
        }

        public void DeleteDocumentFromLecture(int id)
        {
            var document = GetDocumentById(id);
            context.Documents.Remove(document);
            context.SaveChanges();

        }
        public Lecture_Documents GetDocumentById(int id)
        {
            return context.Documents.FirstOrDefault(d => d.Id == id);
        }

        public void AddExamToLecture(int id, Lecture_Exams exam)
        {
            var lecture = context.Lectures.FirstOrDefault(l => l.Id == id);
            exam.LectureId = id;
            exam.Id = 0;
            exam.Date = DateTime.Now.Date;
            context.Exams.Add(exam);
            context.SaveChanges();
        }
        public Lecture_Exams GetExamById(int id)
        {
            return context.Exams.Include(l => l.ExamQuestions).FirstOrDefault(e => e.Id == id);
        }
        public void AddQuestionsToExam(int id , List<Exam_Questions> examQuestions)
        {
            var exam = context.Exams.FirstOrDefault(l => l.Id == id);
            foreach (var examQuestion in examQuestions)
            {
                examQuestion.ExamId = exam.Id;
            }
            context.Questions.AddRange(examQuestions);
            context.SaveChanges();
        }
        public void UpdateExam(int id, Lecture_Exams exam)
        {
            var OldExam = GetExamById(id);
            if (OldExam == null)
            {
                throw new Exception($"Exam with ID {id} not found.");
            }
            if (exam.Name != null)
            {
                OldExam.Name = exam.Name;
            }
            if (exam.Description != null)
            {
                OldExam.Description = exam.Description;
            }
            if (exam.ExamQuestions != null)
            {
                foreach (var newQuestion in exam.ExamQuestions)
                {
                    var existingQuestion = OldExam.ExamQuestions
                        .FirstOrDefault(q => q.Id == newQuestion.Id);

                    if (existingQuestion != null)
                    {
                        // Update existing question
                        existingQuestion.Title = newQuestion.Title;
                        existingQuestion.AnswerOne = newQuestion.AnswerOne;
                        existingQuestion.AnswerTwo = newQuestion.AnswerTwo;
                        existingQuestion.AnswerThree = newQuestion.AnswerThree;
                        existingQuestion.AnswerFour = newQuestion.AnswerFour;
                        existingQuestion.CorrectAnswer = newQuestion.CorrectAnswer;

                        // Mark the question as modified
                        context.Entry(existingQuestion).State = EntityState.Modified;
                    }
                }
            }
            context.SaveChanges();



        }


        public void DeleteExam(int id)
        {
            var exam = GetExamById(id);
            context.Exams.Remove(exam);
            context.SaveChanges();
        }
        public void RegisterCourse(int StudentId ,int CourseId)
        {
            var student = context.Students.FirstOrDefault(s=> s.Id == StudentId);
            var course = context.Courses.FirstOrDefault(c => c.Id == CourseId);
            Course_Students course_Student = new Course_Students
            {
                Student_ID = StudentId,
                Course_ID = CourseId
            };
            context.Course_Students.Add(course_Student);
            context.SaveChanges();
        }

    }
}
