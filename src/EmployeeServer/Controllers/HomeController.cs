using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenResponsible.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsible.Controllers
{
    public class HomeController : Controller
    {
        readonly IKitchenResponsibleService employeeService;

        public HomeController (IKitchenResponsibleService employeeService)
        {
            this.employeeService = employeeService;
        }

        public IActionResult Index()
        {
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

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
