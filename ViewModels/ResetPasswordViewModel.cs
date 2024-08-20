using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ResetPasswordViewModel
	{

		[Required(ErrorMessage = "password is required ")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "password is required ")]
		[Compare(nameof(NewPassword), ErrorMessage = "confirm password not match password ")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}
