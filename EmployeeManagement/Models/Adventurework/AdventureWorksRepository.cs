
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models.Adventurework.DemoScrollingPaging
{
    public class AdventureWorksRepository : IAdventureWorksRepository
    {
        private readonly string connectionstr;
        private DataSource dataSource = null;

        public AdventureWorksRepository(string connectionstr)
        {
            this.connectionstr = connectionstr;
            if(string.IsNullOrEmpty(this.connectionstr))
            {
                throw new ArgumentNullException("connection string cannot be empty");
            }
            dataSource = new DataSource(this.connectionstr);
        }

        public async Task<IEnumerable<Employee>> GetAsyncEmployees(int pageNumber, int pageSize)
        {
            var result = await dataSource.GetAsyncPagingEmployees(pageNumber, pageSize);
            return result;
        }

        public IEnumerable<DemoScrollingPaging.Employee> GetEmployees(int pageNumber, int pageSize)
        {

          return  dataSource.GetPagingEmployees(pageNumber, pageSize);
        }
    }
}
