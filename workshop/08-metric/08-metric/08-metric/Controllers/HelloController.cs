using System;
using Microsoft.AspNetCore.Mvc;

namespace metric.Controllers
{
    [ApiController]
    [Route("api/hello")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello";
        }
    }
}
