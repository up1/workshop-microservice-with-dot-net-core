using System;
using EmployeeApi.Repositories;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [Route("api/employees")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository repository;
        public EmployeeController(EmployeeRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (repository.Employees != null)
                return Ok(repository.Employees);

            return NotFound();
        }

    }
}
