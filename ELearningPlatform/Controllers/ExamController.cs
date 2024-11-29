using ELearningPlatform.Models;
using ELearningPlatform.View_Models;
using ELearningPlatform.Repositery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Controllers
{
    public class ExamController : Controller
    {
        ILectureRepositery lectureRepositery;
        IExamRepositery examRepositery;
        ICourseRepositery courseRepositery;
        public ExamController( ILectureRepositery lectureRepositery, IExamRepositery examRepositery , ICourseRepositery courseRepositery)
        {
            this.lectureRepositery = lectureRepositery;
            this.examRepositery = examRepositery;
            this.courseRepositery = courseRepositery;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddExamToLecture(int id)
        {
            var lecture = lectureRepositery.GetLectureById(id);
            var exam = new Lecture_Exams
            {
                LectureId = lecture.Id
            };
            return View(exam);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddExamToLecture(int id, Lecture_Exams exam)
        {
            var lecture = lectureRepositery.GetLectureById(id);
            examRepositery.AddExamToLecture(id, exam);
            return RedirectToAction("AddQuestionsToExam", new { id = exam.Id });
        }
        public IActionResult AddQuestionsToExam(int id)
        {
            var exam = examRepositery.GetExamById(id);
            var Questions = new List<Exam_Questions>
            {
                new Exam_Questions { ExamId = exam.Id }
            };
            return View(Questions);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddQuestionsToExam(int id, List<Exam_Questions> examQuestions)
        {
            var exam = examRepositery.GetExamById(id);
            examRepositery.AddQuestionsToExam(id, examQuestions);
            return RedirectToAction("index");
        }
        public IActionResult GetExamById(int id)
        {
            var exam = examRepositery.GetExamById(id);
            if (exam.ExamQuestions.Count == 0)
            {
                return RedirectToAction("AddQuestionsToExam", new { id = exam.Id });
            }
            return View(exam);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateExam(int id, Lecture_Exams exam)
        {
            examRepositery.UpdateExam(id, exam);
            return RedirectToAction("index");
        }
        public IActionResult DeleteExamById(int id)
        {
            examRepositery.DeleteExam(id);
            return RedirectToAction("index");
        }
        public IActionResult StartExam(int examId, int studentId)
        {
            // Check if the student has already taken the exam
            var existingEntry = examRepositery.GetStudentExam(studentId, examId);
            if (existingEntry != null)
            {
                return RedirectToAction("ExamResult", new { examId = examId, studentId = studentId });
            }

            var exam = examRepositery.GetExamById(examId);

            // Create a new entry in the Student_Exams table
            var studentExam = new Student_Exams
            {
                StudentId = studentId,
                ExamId = examId,
                LectureId = exam.LectureId // Ensure you provide the correct lecture ID
            };

            examRepositery.SaveStudentExam(studentExam);

            // Return a partial view for the exam content
            return PartialView("_Exam", exam); // Create a partial view named _Exam
        }




        public IActionResult TakeExam(int examId, int studentId)
        {
            var exam = examRepositery.GetExamById(examId);
            if (exam == null)
            {
                return NotFound();
            }

            var viewModel = new TakeExamViewModel
            {
                ExamId = examId,
                StudentId = studentId,
                Questions = exam.ExamQuestions // Ensure this is populated
            };

            return View(viewModel);
        }

        public IActionResult GetExamResults(int studentId, int examId)
        {
            // Fetch the results using the repository
            var results = examRepositery.GetExamResults(studentId, examId);

            // Check if results are empty
            if (!results.Any())
            {
                return PartialView("_NoResultsPartial"); // Create a partial view to display a message
            }

            // Map the results to the view model
            var resultViewModel = results.Select(r => new ExamResultItemViewModel
            {
                ExamQuestion = r.ExamQuestion.Title, // Assuming you have a property for the question text
                SelectedAnswer = r.SelectedAnswer, // The student's answer
                CorrectAnswer = r.ExamQuestion.CorrectAnswer, // Assuming you have a property for the correct answer
                IsCorrect = r.IsCorrect // Assuming you have a property indicating if the answer was correct
            }).ToList();

            // Return the partial view with the populated model
            return PartialView("_ExamResultPartial", resultViewModel);
        }
        // Action to handle exam submission
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SubmitExam(int examId, int studentId, Dictionary<int, string> selectedAnswers)
        {
            // Retrieve the exam and questions
            var exam = examRepositery.GetExamById(examId);
            if (exam == null)
            {
                return NotFound();
            }

            // Calculate the grade
            int totalQuestions = exam.ExamQuestions.Count;
            int correctAnswers = 0;

            // List to store each question result
            var questionResults = new List<ExamResultItemViewModel>();

            foreach (var question in exam.ExamQuestions)
            {
                // Check if the student's answer matches the correct answer
                string studentAnswer = selectedAnswers.ContainsKey(question.Id) ? selectedAnswers[question.Id] : string.Empty;
                bool isCorrect = string.Equals(studentAnswer, question.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

                // Count correct answers
                if (isCorrect)
                {
                    correctAnswers++;
                }

                // Add the question result to the list
                questionResults.Add(new ExamResultItemViewModel
                {
                    ExamQuestion = question.Title,
                    SelectedAnswer = studentAnswer,
                    CorrectAnswer = question.CorrectAnswer,
                    IsCorrect = isCorrect
                });
            }

            // Calculate the grade as a percentage
            int grade = (int)((double)correctAnswers / totalQuestions * 100);
            var lecture = lectureRepositery.GetLectureById(exam.LectureId);

            // Save the result in the Student_Exams table
            var studentExam = new Student_Exams
            {
                StudentId = studentId,
                ExamId = examId,
                CourseId = lecture.CourseId,
                LectureId = exam.LectureId,
                Grade = grade,
                Date = DateTime.Now
            };

            // Save to the database (implement the method in the repository)
            examRepositery.SaveStudentExam(studentExam);

            // Prepare the view model for the exam result
            var examResultViewModel = new ExamResultViewModel
            {
                ExamId = examId,
                StudentId = studentId,
                CorrectAnswers = correctAnswers,
                TotalQuestions = totalQuestions,
                Results = questionResults // Use the updated question results
            };

            // Return the partial view with the exam result data
            return PartialView("_ExamResultPartial", examResultViewModel);
        }







        // Action to show exam results
        public IActionResult ExamResult(int examId, int studentId)
        {
            // Get exam results from the repository
            var results = examRepositery.GetExamResults(studentId, examId);

            // Check if results are empty
            if (results == null || !results.Any())
            {
                // Log or handle the case when no results are found
                Console.WriteLine($"No results found for Student ID {studentId} and Exam ID {examId}");
            }

            // Calculate total questions and correct answers
            var totalQuestions = results.Count;
            var correctAnswers = results.Count(r => r.IsCorrect);

            // Create the view model
            var resultViewModel = new ExamResultViewModel
            {
                StudentId = studentId,
                ExamId = examId,
                Results = results.Select(r => new ExamResultItemViewModel
                {
                    ExamQuestion = r.ExamQuestion.Title, // Make sure ExamQuestion has Title
                    SelectedAnswer = r.SelectedAnswer,
                    CorrectAnswer = r.ExamQuestion.CorrectAnswer, // Make sure ExamQuestion has CorrectAnswer
                    IsCorrect = r.IsCorrect
                }).ToList(),
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers
            };

            // Return the view with the populated model
            return View(resultViewModel);
        }






    }
}


