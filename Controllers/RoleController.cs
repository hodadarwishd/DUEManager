using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager,IMapper mapper) {
           _roleManager = roleManager;
           _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    Name = R.Name
                    
                }).ToListAsync();
                return View(roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(name);
                // we willdomapping to convert from ApplicationUser to User
                // manual mapping
                if(role is not null)
                {
                    var MappedRole = new RoleViewModel()
                    {
                        Id = role.Id,
                        Name = role.Name

                    };
                    return View(new List<RoleViewModel>() { MappedRole });
                }
                else
                {
                    return View(Enumerable.Empty<RoleViewModel>());
                }
                
            }

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Create(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                var mappRole= _mapper.Map<RoleViewModel,IdentityRole>(roleVM);
              await  _roleManager.CreateAsync(mappRole);
                return RedirectToAction(nameof(Index));
            }
            return View(roleVM);
        }
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
            {
                return NotFound();
            }
            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);


            return View(viewName, mappedRole);
        }

        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]// this for not enable any one to do any thing out the application 
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel UpdatedRole)
        {
            if (id != UpdatedRole.Id)
            {
                return BadRequest();
            }
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                role.Name = UpdatedRole.Name;
               
                await _roleManager.UpdateAsync(role);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                //log message 

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(UpdatedRole);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");// to not enable repeating coding 
        }
        [HttpPost(Name = "Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {

            try
            {
                var user = await _roleManager.FindByIdAsync(id);
                 

                await _roleManager.DeleteAsync(user);

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
