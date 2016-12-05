using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenResponsible.Filters;
using KitchenResponsible.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsible.Controllers
{
    [TypeFilter(typeof(LessPoorButStillPoorCacheResourceFilterAttribute))]
    public class HomeController : Controller
    {
        readonly IKitchenResponsibleService employeeService;

        public HomeController (IKitchenResponsibleService employeeService)
        {
            this.employeeService = employeeService;
        }

        public IActionResult Index()
        {
            // TODO: What about different timezones? Now the server is king, but actually Trondheim office is king
            var employeeForWeek = employeeService.GetEmployeeForWeek();
            ViewData["Week"] = employeeForWeek.Week;
            ViewData["Responsible"] = employeeForWeek.Responsible;
            ViewData["OnDeck"] = employeeForWeek.OnDeck;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
