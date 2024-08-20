using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _mapper = mapper;
        }

		public async Task<IActionResult> Index(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				var users = await _userManager.Users.Select( U => new UserViewModel()
				{
					Email = U.Email,
					FName = U.FName,
					LName = U.LName,
					PhoneNumber = U.PhoneNumber,
					Roles =  _userManager.GetRolesAsync(U).Result
				}).ToListAsync();
				return View(users);
			}
			else
			{
				var user=await _userManager.FindByEmailAsync(email);
				// we willdomapping to convert from ApplicationUser to User
				// manual mapping
				var MappedUser = new UserViewModel()
				{
					Email = user.Email,
					FName = user.FName,
					LName = user.LName,
					PhoneNumber = user.PhoneNumber,
					Roles = _userManager.GetRolesAsync(user).Result

				};
				return View(new List<UserViewModel> (){ MappedUser });
			}
			
		}

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
			var user = await  _userManager.FindByEmailAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            var mappedUser =   _mapper.Map<ApplicationUser, UserViewModel>( user);

           
            return View(viewName, mappedUser);
        }

        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]// this for not enable any one to do any thing out the application 
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel UpdatedUser)
        {
            if (id != UpdatedUser.Id)
            {
                return BadRequest();
            }
            try
            {
                var user =await  _userManager.FindByIdAsync(id);
                user.FName= UpdatedUser.FName;
                user.LName= UpdatedUser.LName;
                user.PhoneNumber= UpdatedUser.PhoneNumber;

               await  _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                //log message 

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(UpdatedUser);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");// to not enable repeating coding 
        }
        [HttpPost(Name ="Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete( string id)
        {
            
            try
            {
               var user = await _userManager.FindByIdAsync(id);
               

                await _userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                //log message 

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }


    }
}
