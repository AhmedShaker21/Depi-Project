using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningPlatform.Models
{
    public class Lecture_Videos
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Lecture")]
        public int LectureId { get; set; }

        public string Title { get; set; }
        public byte[] VideoData { get; set; }
        public string ContentType {  get; set; }

        public Course_Lectures Lecture { get; set; }
    }
}

