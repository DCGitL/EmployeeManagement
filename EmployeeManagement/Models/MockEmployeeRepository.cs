using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        public MockEmployeeRepository()
        {
            _employees = new List<Employee>()
            {
                new Employee{Id=1, Name="Mary", Department = Dept.HR, Email="mary@hotmail.com"},
                 new Employee{Id=2, Name="John", Department = Dept.IT, Email="John@hotmail.com"},
                  new Employee{Id=3, Name="Sam", Department= Dept.IT, Email="Sam@hotmail.com"}
            };
        }
        private List<Employee> _employees;
        public Employee GetEmployee(int Id)
        {

            return  this._employees.FirstOrDefault(e=> e.Id ==Id);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return this._employees;
        }

        public Employee Add(Employee employee)
        {
            int newID = _employees.Select(e => e.Id).Max() + 1;
             employee.Id = newID;
            _employees.Add(employee);

            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee foundEmployee = _employees.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (foundEmployee != null)
            {
                foundEmployee.Department = employeeChanges.Department;
                foundEmployee.Email = employeeChanges.Email;
                foundEmployee.Name = employeeChanges.Name;

            }
            return foundEmployee;
        }

        public Employee Delete(int id)
        {
            Employee foundEmployee = _employees.FirstOrDefault(e => e.Id == id);
            if (foundEmployee != null)
            {
                _employees.Remove(foundEmployee);
            }
            return foundEmployee;
        }
    }
}
