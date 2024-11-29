using ELearningPlatform.Validations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.View_Models
{
    public class StudentAccount
    {
		public int Id { get; set; }
		public string Name { get; set; }

		[PersonalData]
		public string Gender { get; set; }
		[PersonalData]
		public string? Street { get; set; }
		[PersonalData]
		public string? City { get; set; }
		[PersonalData]
		public string? Country { get; set; }
		[Required]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }
		[Required]
		[PersonalData]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		[PersonalData]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		[PersonalData]
		[Compare("Password")]
		[DataType(DataType.PhoneNumber)]
		public string ConfirmPassword { get; set; }
		[UniqueStudentName]
		public string UserName { get; set; }
		public string? Action { get; set; }
	}
}