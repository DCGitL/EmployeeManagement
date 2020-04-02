using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<SQLEmployeeRepository> logger;

        public SQLEmployeeRepository(AppDbContext context, ILogger<SQLEmployeeRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        public Employee Add(Employee employee)
        {
            this.context.Add(employee);
            this.context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee foundEmployee = this.context.Employees.Find(id);
            if (foundEmployee != null)
            {
                this.context.Remove(foundEmployee);
                this.context.SaveChanges();
            }

            return foundEmployee;

        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return this.context.Employees;
        }

        public Employee GetEmployee(int Id)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return this.context.Employees.Find(Id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = this.context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return employeeChanges;
        }
    }
}
