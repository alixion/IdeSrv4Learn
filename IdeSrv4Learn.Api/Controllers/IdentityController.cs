using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdeSrv4Learn.Api.Controllers
{
    public class IdentityController : ControllerBase
    {
        [Authorize("ApiScope")]
        [HttpGet("identity")]
        public IActionResult Index()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}