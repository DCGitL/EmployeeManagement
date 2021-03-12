using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.PartialRender
{
    public interface IPartialRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
