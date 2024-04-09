using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace C3P1.Net.WebApi.Misc
{
    [ApiController]
    [Route("api/misc/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Hello webApi");
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("user")]
        public ActionResult<IEnumerable<string>> GetUserData()
        {
            return Ok(new string[] { "Hello WebApi", "Hello User" });
        }

        [Authorize (Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("admin")]
        public ActionResult<IEnumerable<string>> GetAdminData()
        {
            return Ok(new string[] { "Hello WebApi", "Hello Admin" });
        }
    }
}
