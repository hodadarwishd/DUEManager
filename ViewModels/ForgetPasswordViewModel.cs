using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ForgetPasswordViewModel
	{

		[Required(ErrorMessage = "email is required ")]
		[EmailAddress(ErrorMessage = "invalid Email")]
		public string Email { get; set; }
	}
}
