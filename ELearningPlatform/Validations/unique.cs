using ELearningPlatform.Models;
using ELearningPlatform.View_Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.Validations
{
	public class UniqueStudentNameAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			// Retrieve the UserManager<ApplicationUser> using the IServiceProvider from the ValidationContext
			var _userManager = (UserManager<ApplicationUser>)validationContext.GetService(typeof(UserManager<ApplicationUser>));

			string name = value?.ToString();

			// Check if a user exists with the same name
			ApplicationUser user = _userManager.Users.FirstOrDefault(u => u.UserName == name);

			// Get the current student account from the validation context
			StudentAccount stufromreq = validationContext.ObjectInstance as StudentAccount;

			if (user == null || (stufromreq != null && user.Id == stufromreq.Id))
			{
				return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult("Name is already in use.");
			}
		}
	}
	public class uniqueInstructorName : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			// Retrieve the UserManager<ApplicationUser> using the IServiceProvider from the ValidationContext
			var _userManager = (UserManager<ApplicationUser>)validationContext.GetService(typeof(UserManager<ApplicationUser>));

			string name = value?.ToString();

			// Check if a user exists with the same name
			ApplicationUser user = _userManager.Users.FirstOrDefault(u => u.UserName == name);

			// Get the current student account from the validation context
			InstructorAccount Insfromreq = validationContext.ObjectInstance as InstructorAccount;

			if (user == null || (Insfromreq != null && user.Id == Insfromreq.Id))
			{
				return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult("Name is already in use.");
			}
		}
	}
}
