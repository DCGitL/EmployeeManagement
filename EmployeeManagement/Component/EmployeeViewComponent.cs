using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Component
{
    public class EmployeeViewComponent(IEmployeeRepository employeeRepository) : ViewComponent
    {
        public IViewComponentResult Invoke(int id)
        {
            var result = employeeRepository.GetEmployee(id);

            return View(result);
        }
    }
}
