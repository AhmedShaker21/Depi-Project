using ELearningPlatform.Models;
using ELearningPlatform.Repositery;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICourseRepositery, CourseRepositery>();

            builder.Services.AddDbContext<ELearningContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer("Data Source=.;Initial Catalog=ELearningPlatform;Integrated Security=True; Trust Server Certificate =True");
            });

            // Set the maximum file upload size to 1 GB (1 * 1024 * 1024 * 1024 bytes)
            long maxFileSize = 10L * 1024 * 1024 * 1024;


            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = maxFileSize; // 1 GB
            });

            // If you're using IIS, configure it in `IISServerOptions`
            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = maxFileSize; // 1 GB
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

