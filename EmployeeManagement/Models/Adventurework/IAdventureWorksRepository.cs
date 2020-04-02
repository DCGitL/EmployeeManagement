using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models.Adventurework.DemoScrollingPaging
{
    public interface IAdventureWorksRepository
    {
        IEnumerable<Employee> GetEmployees(int pageNumber, int pageSize);

        Task<IEnumerable<Employee>> GetAsyncEmployees(int pageNumber, int pageSize);
    }
}
