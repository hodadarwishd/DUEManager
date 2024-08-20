using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser>userManager,SignInManager<ApplicationUser>signInManager ) 
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
        #region Register
        public IActionResult Register()
        {
            return View();
        }
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid) 
			{
				var user = new ApplicationUser()
				{
					UserName = model.Email.Split("@")[0],
					Email = model.Email,
					IsAgree = model.IsAgree,
					FName=model.FName,
					LName=model.LName
					//because i not have property isAgree i will custimize it 
				};
				  var result= await  _userManager.CreateAsync(user,model.Password);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(model);
		}

		#endregion

		#region Login

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user=await _userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					var flag=await _userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						var result=await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
						if (result.Succeeded)
						{
							return RedirectToAction(nameof(HomeController.Index),"Home");
						}
					}
				}
				ModelState.AddModelError(string.Empty, "invalid login");
			}
			return View(model);
		}

		#endregion

		#region Sign Out
		public async new Task<IActionResult> SignOutAsync()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		#endregion

		#region ForgetPassword
		public IActionResult ForgetPassword()
		{

			return View();
		}
	public async Task< IActionResult> SendResetPasswordURL( ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user =await  _userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					var token =await  _userManager.GeneratePasswordResetTokenAsync(user);// unique token for one user for one time 
					var ResetPasswordURL = Url.Action("ResetPassword", "Account", new {email= model.Email,token=token},Request.Scheme);
					var email = new Email
					{
						Subject = "Reset your password",
						Recipients = model.Email,
						Body = ResetPasswordURL
					};
					EmailSettings.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "invalid Email");

			}
			return View(model);
		}
		#endregion

		#region Reset Password 
		public IActionResult ResetPassword(string email, string token)
		{
			// i do this to send them to   ResetPassword(ResetPasswordViewModel model)
			TempData["email"]=email;
			TempData["token"]=token;
			return View();
		}
		[HttpPost]
		public async  Task< IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
		

			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;
				var user=await  _userManager.FindByEmailAsync(email);
				var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if ( result.Succeeded)
                {
					return RedirectToAction(nameof(Login)); 
                }
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
            }

			return View();
		}

		#endregion
		public IActionResult CheckYourInbox()
		{
			return View();
		}
	}
}
