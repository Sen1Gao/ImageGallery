using Backend.interfaces;
using BasicArgs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SigninController : ControllerBase
    {
        private readonly IDbManagement dbManagement;
        public SigninController(IDbManagement dbManagement) 
        {
            this.dbManagement = dbManagement;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] SigninInfoArgs signinInfoArgs)
        {
            if(signinInfoArgs.SigninType==SigninType.Admin)
            {
                var result = await dbManagement.QueryUserAsync(signinInfoArgs.Username, signinInfoArgs.Password);
                return Ok(result);

            }
            else if(signinInfoArgs.SigninType == SigninType.Guest)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SigninInfoArgs signinInfoArgs)
        {
            if (signinInfoArgs.SigninType == SigninType.Admin)
            {
                var result = await dbManagement.RegisterUserAsync(signinInfoArgs.Username, signinInfoArgs.Password);
                return Ok(result);
            }
            else
            {
                return Ok(false);
            }
        }
    }
}
