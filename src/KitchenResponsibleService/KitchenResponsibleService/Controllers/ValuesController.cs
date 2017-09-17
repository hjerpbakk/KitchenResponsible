using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenResponsibleService.Model;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsibleService.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<EmployeeWeek> Get()
        {
            return new EmployeeWeek[] { 
                new EmployeeWeek { WeekNumber = 41, SlackUser = "U1TBU8336" },
                new EmployeeWeek { WeekNumber = 42, SlackUser = "U1TBU8336" }};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
