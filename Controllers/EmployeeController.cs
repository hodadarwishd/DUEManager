using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private  readonly IUnitOfWork _unitOfWork;

        //Inheritance : EmployeeController has a Controller
        // Aggregation : EmployeeController is a  DepartmentRepository 
        // private readonly IEmployeeRepository _employeeRepository; // attribute from DepartmentRepository
        // private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(  IMapper mapper,IUnitOfWork unitOfWork /* IEmployeeRepository employeeRepository*/ /*, IDepartmentRepository departmentRepository*/)// ask clr for creation object from class implementing interface  IDepartmentRepository
        {
            _mapper = mapper;
           _unitOfWork = unitOfWork;
            //   _employeeRepository = employeeRepository;
            // _departmentRepository = departmentRepository;
        }

        public async Task< IActionResult> Index()
        {
            var employees =await  _unitOfWork.EmployeeRepository.GetAll();
            var mappedEmp = _mapper.Map<IEnumerable< Employee>, IEnumerable< EmployeeViewModel> >(employees);

            return View(mappedEmp);
        }
        [HttpGet]
        public IActionResult Create()
        {
          //  ViewData["Departments"] = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(EmployeeViewModel employeeVM)
        {
            // employeeRepository take from me employee for this reson
            // i want to transfer from EmployeeViewModel to Employee
            //for this reason we will use mapping 
            //1= manuall mapping 
           

            if (ModelState.IsValid) // server side validation 
            {
                employeeVM.ImageName=await  DocumentSettings.UploadFile(employeeVM.Image,"images");
                //var MappVM = new Employee()
                //{
                //    Name = employeeVM.Name,
                //    Age = employeeVM.Age,
                //    Salary = employeeVM.Salary,
                //    HireDate = employeeVM.HireDate,
                //    CreationDate = employeeVM.CreationDate,
                //    Email = employeeVM.Email,
                //    PhoneNumber = employeeVM.PhoneNumber
                //};
                var mappedEmp=_mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Add(mappedEmp);
               await  _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task < IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var employee =await  _unitOfWork.EmployeeRepository.Get(id.Value);
            var mappedEmp = _mapper.Map<Employee,EmployeeViewModel>(employee);

            if (employee is null)
            {
                return NotFound();
            }
            return View(viewName, mappedEmp);
        }

        public async Task <IActionResult> Edit(int? id)
        {
          //  ViewData["Departments"] = _departmentRepository.GetAll();

            return await  Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]// this for not enable any one to do any thing out the application 
        public async Task <IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.EmployeeRepository.Update(mappedEmp);

               await  _unitOfWork.Complete();  

            }
            catch (Exception ex)
            {
                //log message 

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction(nameof(Index));
            return View();
        }


        public async Task<IActionResult> Delete(int? id)
        {
            return await  Details(id, "Delete");// to not enable repeating coding 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

               _unitOfWork.EmployeeRepository.Delete(mappedEmp);
               var count = await  _unitOfWork.Complete();
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "images");
                }
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
