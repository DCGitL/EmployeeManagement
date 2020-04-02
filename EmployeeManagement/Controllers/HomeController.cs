using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger<HomeController> logger;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
           

            var viewModel = _employeeRepository.GetAllEmployees();
            return View(viewModel);
        }


        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            // throw new Exception("Detatil error exception ");
            //logger.LogTrace("Trace Log");
            //logger.LogDebug("Debug Log");
            //logger.LogInformation("Information Log");
            //logger.LogWarning("Warning Log");
            //logger.LogError("Error Log");
            //logger.LogCritical("Cretical Log");
           
            HomeDetailsViewModel viewModel = new HomeDetailsViewModel
            {
                Employee = _employeeRepository.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };

            return View(viewModel);
        }

   
        public ViewResult Create()
        {

            return View(new EmployeeCreateViewModel());
        }


        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFiles(viewModel);


                Employee employee = new Employee { Name = viewModel.Name, Department = viewModel.Department, Email = viewModel.Email, PhotoPath = uniqueFileName };
                _employeeRepository.Add(employee);
                return RedirectToAction("Details", new { id = employee.Id });
            }
            //  else
            return View(viewModel);


        }


 
     
        public IActionResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);

            var viewModel = new EmployeeEditViewModel
            {
                Id = id,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(viewModel);

        }
        
      
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Employee emp = _employeeRepository.GetEmployee(Id);
            if (!string.IsNullOrEmpty(emp.PhotoPath))
            {
                //delete the existing photo
                string existingPhoto = Path.Combine(hostingEnvironment.WebRootPath, "images", emp.PhotoPath);
                System.IO.File.Delete(existingPhoto);

            }

            _employeeRepository.Delete(Id);

            return RedirectToAction("Index");
            
        }

        [HttpPost]
        public IActionResult Update(EmployeeEditViewModel viewModel)
        {
            if(ModelState.IsValid)
            {               
                Employee emp = _employeeRepository.GetEmployee(viewModel.Id);
                if (emp != null)
                {
                    emp.Department = viewModel.Department;
                    emp.Name = viewModel.Name;
                    emp.Email = viewModel.Email;
                    emp.Id = viewModel.Id;
                    if (viewModel.Photos != null && viewModel.Photos.Count > 0)
                    {
                        if(viewModel.ExistingPhotoPath != null)
                        {
                            //delete the existing photo
                           string existingPhoto = Path.Combine(hostingEnvironment.WebRootPath, "images", viewModel.ExistingPhotoPath);
                            System.IO.File.Delete(existingPhoto);

                        }
                        emp.PhotoPath = ProcessUploadedFiles(viewModel);

                    }

                    _employeeRepository.Update(emp);

                    return RedirectToAction("Index");
                }
                //do update
            }
            return View(viewModel);
        }

        private string ProcessUploadedFiles(EmployeeCreateViewModel viewModel)
        {
            string uniqueFileName = null;

            if (viewModel.Photos != null && viewModel.Photos.Count > 0)
            {
                foreach (var photo in viewModel.Photos)
                {
                    string uploadFolder = Path.Combine(this.hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    using (FileStream file = new FileStream(filePath, FileMode.Create))
                    {
                         photo.CopyTo(file);
                    }
                       
                }

            }

            return uniqueFileName;
        }
    }
}
