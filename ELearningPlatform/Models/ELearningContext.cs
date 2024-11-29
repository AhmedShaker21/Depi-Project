using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Models
{
    public class ELearningContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ELearningContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course_Students> Course_Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecture_Documents> Documents { get; set; }
        public DbSet<Lecture_Videos> Videos { get; set; }
        public DbSet<Lecture_Exams> Exams { get; set; }
        public DbSet<Course_Lectures> Lectures { get; set; }
        public DbSet<Exam_Questions> Questions { get; set; }
        public DbSet<Course_Codes> Codes { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Students_QuestionsAnswers> Students_QuestionsAnswers { get; set; }
        public DbSet<Student_Exams> Student_Exams { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging().UseSqlServer("Data Source=.;Initial Catalog=ELearningPlatform;Integrated Security=True; Trust Server Certificate =True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-one relationship between ApplicationUser and Admin
            modelBuilder.Entity<ApplicationUser>()
                 .HasOne(a => a.Admin)
                 .WithOne(admin => admin.User)
                 .HasForeignKey<Admin>(admin => admin.ApplicationUser_Id)
                 .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ApplicationUser>()
                 .HasOne(a => a.Student)
                 .WithOne(student => student.User)
                 .HasForeignKey<Student>(student => student.ApplicationUser_Id)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                 .HasOne(a => a.Instructor)
                 .WithOne(instructor => instructor.User)
                 .HasForeignKey<Instructor>(instructor => instructor.ApplicationUser_Id)
                 .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Course_Students>().HasKey("Student_ID", "Course_ID", "Code_ID");
            modelBuilder.Entity<Student_Exams>().HasKey("StudentId", "CourseId", "ExamId","LectureId");
            // Configure relationships for Student_Exams
            modelBuilder.Entity<Student_Exams>()
                .HasOne(se => se.Student)
                .WithMany(s => s.Student_Exams)
                .HasForeignKey(se => se.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict to avoid cycles

            modelBuilder.Entity<Student_Exams>()
                .HasOne(se => se.Exam)
                .WithMany(e => e.StudentExams)
                .HasForeignKey(se => se.ExamId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict to avoid cycles

            modelBuilder.Entity<Student_Exams>()
                .HasOne(se => se.Lecture)
                .WithMany(l => l.StudentExams)
                .HasForeignKey(se => se.LectureId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict to avoid cycles
            modelBuilder.Entity<Course>()
       .HasOne(c => c.Instructor)
       .WithMany(i => i.Courses)
       .HasForeignKey(c => c.InstructorId)
       .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction
            modelBuilder.Entity<Student_Exams>()
                .HasOne(se => se.Course)
                .WithMany(c => c.StudentExams)
                .HasForeignKey(se => se.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Course_Codes>()
    .HasOne(cc => cc.Course)
    .WithMany(c => c.Crs_Codes)
    .HasForeignKey(cc => cc.CourseId)
    .OnDelete(DeleteBehavior.Restrict);
        }



    }

}

