using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class RegisterViewModel

	{
		[Required(ErrorMessage = "first name  is required ")]
        public string FName { get; set; }

		[Required(ErrorMessage = "last name is required ")]
		public string LName { get; set; }

		[Required(ErrorMessage ="email is required ")]
		[EmailAddress(ErrorMessage ="invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "password is required ")]
		[MinLength(5,ErrorMessage ="min length is 5 ")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "password is required ")]
		[Compare(nameof(Password),ErrorMessage ="confirm password not match password ")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		public bool IsAgree { get; set; }
	}
}
