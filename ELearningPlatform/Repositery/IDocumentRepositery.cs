using ELearningPlatform.Models;

namespace ELearningPlatform.Repositery
{
    public interface IDocumentRepositery
    {
        void AddDocumentToLecture(int id, Lecture_Documents documents);
        void DeleteDocumentFromLecture(int id);
        Lecture_Documents GetDocumentById(int id);
    }
}
