using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [Route("api/healthz")]
    public class HealthCheckController : Controller
    {
        [HttpGet()]
        public IActionResult Status() => Ok();

    }
}
