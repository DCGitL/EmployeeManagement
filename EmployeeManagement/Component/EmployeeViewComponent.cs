using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Component
{
    public class EmployeeViewComponent : ViewComponent
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeViewComponent(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }


        public IViewComponentResult Invoke(int id)
        {
            var result = employeeRepository.GetEmployee(id);

            return View(result);
        }
    }
}
