using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //Inheritance : DepartmentController has a Controller
        // Aggregation : DepartmentController is a  DepartmentRepository 
       // private readonly IDepartmentRepository _departmentRepository; // attribute from DepartmentRepository
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(  IUnitOfWork unitOfWork  /*IDepartmentRepository departmentRepository*/)// ask clr for creation object from class implementing interface  IDepartmentRepository
        {
           // _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<IActionResult> Index()
        {
            var departments= await _unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public  async  Task<IActionResult>  Create( Department department)
        {
            if (ModelState.IsValid) // server side validation 
            {
                _unitOfWork.DepartmentRepository.Add(department);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task <IActionResult> Details(int? id,string viewName="Details")
        {
            if(id is null)
            {
                return BadRequest();
            }
            var department=await _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null)
            {
                return NotFound();
            }
            return View(viewName,department);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            return await  Details(id, "Edit");// this line to decrease repeating of code and the code of details is same the code of edit 
            //if (id is null)
            //{
            //    return BadRequest();
            //}
            //var department = _departmentRepository.Get(id.Value);
            //if (department is null)
            //{
            //    return NotFound();
            //}
            //return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]// this for not enable any one to do any thing out the application 
        public IActionResult Edit([FromRoute]int id,Department department)
        {
            if(id != department.Id)
            {
                return BadRequest();
            }
            try
            {
                _unitOfWork.DepartmentRepository.Add(department);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                //log message 

                ModelState.AddModelError(string.Empty, ex.Message);
            }
           
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");// to not enable repeating coding 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }
            try
            {
                _unitOfWork.DepartmentRepository.Delete(department);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                //log message 

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }

    }
}
