using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace JwtDotnet8Prectice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [EnableRateLimiting("HitCoundPolicy")]
        //[EnableRateLimiting("fixed")]
        [Authorize]
        //[AllowAnonymous]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok("Autorized methode hitteed");
        }

        [Authorize(Roles ="Manager")]
        //[AllowAnonymous]
        [HttpGet("RoleBasedIndex")]
        public IActionResult RoleBasedIndex()
        {
            return Ok("Autorized Role BAsed methode hitteed");
        }
    }
}
