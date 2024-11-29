using ELearningPlatform.Models;

namespace ELearningPlatform.Repositery
{
    public class CodeRepositery:ICodeRepositery
    {
        ELearningContext context;
        public CodeRepositery(ELearningContext _context)
        {
            context = _context;
        }
        public List<Course_Codes> GetAllCodes()
        {
            return context.Codes.ToList();
        }
        public Course_Codes GetCodeById(int id)
        {
            return context.Codes.FirstOrDefault(c => c.Id == id);
        }
        public void AddCode(Course_Codes code)
        {
            if (code != null && !string.IsNullOrWhiteSpace(code.Code))
            {
                context.Codes.Add(code);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Code cannot be null or empty.");
            }
        }

        public void DeleteCode(int id)
        {
            var code = context.Codes.FirstOrDefault(c=>c.Id == id);
            context.Codes.Remove(code);
            context.SaveChanges();
        }
        public void UpdateCode(int id, Course_Codes code) {
            var oldCode = context.Codes.FirstOrDefault(c => c.Id == id);
            oldCode.Code = code.Code;
            context.SaveChanges();
        }
    }
}
