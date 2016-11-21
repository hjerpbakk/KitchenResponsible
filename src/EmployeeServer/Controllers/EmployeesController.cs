using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KitchenResponsible.Model;
using KitchenResponsible.Services;
using System.Collections;
using KitchenResponsible.Data;

namespace KitchenResponsible.Controllers {
    [Route("api/[controller]")]
    public class EmployeesController : Controller {
        readonly IEmployeeService employeeService;
        readonly IServiceCache serviceCache;      

        public EmployeesController(IEmployeeService employeeService, IServiceCache serviceCache) {
            this.employeeService = employeeService;
            this.serviceCache = serviceCache;
        }

        // GET api/employees
        [HttpGet]
        public IActionResult Get() {
            // TODO: Use GraphQL for this
            // https://github.com/graphql-dotnet/graphql-dotnet
            var cachedResponse = serviceCache.Get();
            if (cachedResponse == null) {
                var response = Ok(employeeService.Get());
                serviceCache.Add(response);
                return response; 
            }
            
            return cachedResponse;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cachedResponse = serviceCache.Get(id);
            if (cachedResponse != null) {
                return cachedResponse;
            }

            var response = Ok(employeeService.Get(id));
            serviceCache.Add(id, response);
            return response;
        }
    }
}