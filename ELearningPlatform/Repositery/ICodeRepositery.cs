using ELearningPlatform.Models;

namespace ELearningPlatform.Repositery
{
    public interface ICodeRepositery
    {
        List<Course_Codes> GetAllCodes();
        Course_Codes GetCodeById(int id);
        void AddCode(Course_Codes code);
        void UpdateCode(int id,Course_Codes code);
        void DeleteCode(int id);
    }
}
