using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models.Adventurework.DemoScrollingPaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [AllowAnonymous]
    public class AdventureWorkController : Controller
    {
        private readonly IAdventureWorksRepository adventureWorksRepository;
        private const int PageSize = 50;
        public AdventureWorkController(IAdventureWorksRepository adventureWorksRepository)
        {
            this.adventureWorksRepository = adventureWorksRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = adventureWorksRepository.GetEmployees(1, PageSize);
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetScrollingPage(int pageNumber)
        {
            var model = await adventureWorksRepository.GetAsyncEmployees(pageNumber, PageSize);
            return Json(model);
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}