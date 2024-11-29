using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.Models
{
    public class Lecture_Documents
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Lecture")]
        public int LectureId { get; set; }
        public string Title {  get; set; }
        public string FilePath { get; set; }
        [NotMapped]
        public IFormFile Document { get; set; }
        public Course_Lectures Lecture { get; set; }
    }
}
