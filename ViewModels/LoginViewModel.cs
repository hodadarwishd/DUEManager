using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class LoginViewModel
	{

		[Required(ErrorMessage = "email is required ")]
		[EmailAddress(ErrorMessage = "invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "password is required ")]
		[MinLength(5, ErrorMessage = "min length is 5 ")]
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
