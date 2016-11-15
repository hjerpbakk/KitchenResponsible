using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KitchenResponsible.Model;
using KitchenResponsible.Services;

namespace KitchenResponsible.Controllers {
    [Route("api/[controller]")]
    public class EmployeesController : Controller {
        readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService employeeService) {
            this.employeeService = employeeService;
        }

        // GET api/employees
        [HttpGet]
        public IActionResult Get() {
            // TODO: Use GraphQL for this
            // https://github.com/graphql-dotnet/graphql-dotnet
            return Ok(employeeService.Get());
        }
    }
}