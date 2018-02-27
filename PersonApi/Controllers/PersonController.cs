using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PersonApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Person")]
    [Authorize]
    public class PersonController : Controller
    {
        [HttpGet]
        public IActionResult GetIdentities()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}