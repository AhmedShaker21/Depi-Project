using ELearningPlatform.Models;
using ELearningPlatform.Repositery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Controllers
{
    public class DocumentController : Controller
    {
        ICourseRepositery courseRepositery;
        ILectureRepositery lectureRepositery;
        IDocumentRepositery documentRepositery;
        public DocumentController(ICourseRepositery courseRepositery, ILectureRepositery lectureRepositery, IDocumentRepositery documentRepositery)
        {
            this.courseRepositery = courseRepositery;
            this.lectureRepositery = lectureRepositery;
            this.documentRepositery = documentRepositery;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddDocumentToCourse(int id)
        {
            // Fetch the course using the course repository, if needed
            Course course = courseRepositery.GetCourseById(id);

            // Create a new Course_Videos object and initialize it with the CourseId
            var DocumentModel = new Lecture_Documents
            {
                LectureId = course.Id // Setting the CourseId in the video model
            };

            // Pass the Course_Videos object to the view
            return View(DocumentModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddDocumentToCourse(int id, Lecture_Documents documents)
        {
            var lecture = lectureRepositery.GetLectureById(id);
            documentRepositery.AddDocumentToLecture(id, documents);
            return RedirectToAction("ViewLecture", "Lecture", new { id = lecture.Id });

        }
        public IActionResult DeleteDocumentById(int id)
        {
            var doc = documentRepositery.GetDocumentById(id);
            var lecture = lectureRepositery.GetLectureById(doc.LectureId);
            documentRepositery.DeleteDocumentFromLecture(id); 
            return RedirectToAction("ViewLecture", "Lecture", new { id = lecture.Id });
        }
        public IActionResult GetDocumentById(int id)
        {
            var document = documentRepositery.GetDocumentById(id);

            if (document == null || string.IsNullOrEmpty(document.FilePath))
            {
                return NotFound("Document not found.");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", document.FilePath);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found on server.");
            }

            // Determine MIME type based on file extension
            var fileType = GetContentType(filePath);
            var fileName = Path.GetFileName(filePath);

            // Return the file for download or display
            return PhysicalFile(filePath, fileType, fileName);
        }

        // Utility method to determine content type based on file extension
        private string GetContentType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".txt" => "text/plain",
                _ => "application/octet-stream",
            };
        }
    }
}
