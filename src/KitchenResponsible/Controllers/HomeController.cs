using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenResponsible.Controllers.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsible.Controllers
{
    public class HomeController : Controller
    {
        static readonly IEmployeeService s_employeeServiceRepo;

        static HomeController()
        {
            s_employeeServiceRepo = new FileBasedEmployeeService();
        }

        public IActionResult Index()
        {
            // TODO: Finn uke basert på denne dato
            var employeeForWeek = s_employeeServiceRepo.GetEmployeeForWeek(46);
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
