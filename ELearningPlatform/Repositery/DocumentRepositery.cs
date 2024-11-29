using ELearningPlatform.Models;

namespace ELearningPlatform.Repositery
{
    public class DocumentRepositery : IDocumentRepositery
    {
        ELearningContext context;
        IWebHostEnvironment env;
        public DocumentRepositery(ELearningContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
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
    }
}
