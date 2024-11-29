using ELearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Repositery
{
    public class ExamRepositery : IExamRepositery
    {
        ELearningContext context;
        public ExamRepositery(ELearningContext context)
        {
            this.context = context;
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
        public void AddQuestionsToExam(int id, List<Exam_Questions> examQuestions)
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
        // Method to submit all answers for a student in an exam
        public void SubmitExam(int studentId, int examId, Dictionary<int, string> selectedAnswers)
        {
            // Get all questions for the exam
            var questions = context.Questions.Where(q => q.ExamId == examId).ToList();

            // Iterate over each question and evaluate the student's answers
            foreach (var question in questions)
            {
                // Check if the student provided an answer for this question
                if (selectedAnswers.TryGetValue(question.Id, out string selectedAnswer))
                {
                    // Determine if the selected answer is correct
                    bool isCorrect = question.CorrectAnswer.Equals(selectedAnswer, StringComparison.OrdinalIgnoreCase);

                    // Save the student's answer
                    var studentAnswer = new Students_QuestionsAnswers
                    {
                        StudentId = studentId,
                        ExamQuestionId = question.Id,
                        SelectedAnswer = selectedAnswer,
                        IsCorrect = isCorrect
                    };

                    context.Students_QuestionsAnswers.Add(studentAnswer);
                }
            }

            context.SaveChanges();
        }

        // Function to retrieve exam results for a student
        public List<Students_QuestionsAnswers> GetExamResults(int studentId, int examId)
        {
            var results = context.Students_QuestionsAnswers
                .Include(s => s.ExamQuestion) // Make sure to include the related entity
                .Where(r => r.StudentId == studentId && r.ExamQuestion.ExamId == examId) // Adjust based on your models
                .ToList();

            // Log the count of results retrieved
            Console.WriteLine($"Retrieved {results.Count} results for Student ID {studentId} and Exam ID {examId}");

            return results;
        }

        public Student_Exams? GetStudentExam(int studentId, int examId)
        {
            return context.Student_Exams
                          .FirstOrDefault(se => se.StudentId == studentId && se.ExamId == examId);
        }
        public void SaveStudentExam(Student_Exams studentExam)
        {
            // Find the existing record using the composite key
            var existingExam = context.Student_Exams
                .FirstOrDefault(se => se.StudentId == studentExam.StudentId &&
                                      se.ExamId == studentExam.ExamId &&
                                      se.CourseId == studentExam.CourseId &&
                                      se.LectureId == studentExam.LectureId);

            if (existingExam != null)
            {
                // Update the existing record if Grade or Date is not set
                if (existingExam.Grade == null || existingExam.Date == null)
                {
                    existingExam.Grade = studentExam.Grade;
                    existingExam.Date = DateTime.Now; // Update the date to current
                    context.SaveChanges();
                }
                else
                {
                    // If the record already has a Grade and Date, handle as needed (e.g., throw an exception or log)
                    throw new InvalidOperationException("This exam record already exists and has a grade/date. Overwrite is not allowed.");
                }
            }
            else
            {
                // If no existing record found, insert a new one
                context.Student_Exams.Add(studentExam);
                context.SaveChanges();
            }
        }





        public void SaveStudentGrade(int studentId, int examId, int grade)
        {
            var studentExam = GetStudentExam(studentId, examId);
            if (studentExam != null)
            {
                studentExam.Grade = grade;
                studentExam.Date = DateTime.Now;
                context.SaveChanges();
            }
        }


    }
}
