using Microsoft.AspNetCore.Mvc;

using ClientSWH.Core.Abstraction.Services;
using ClientSWH.Contracts;



namespace ClientSWH.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUsersService usersService
        //, IWebHostEnvironment appEnvironment
            ) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;

        [HttpPost("login")]
        
        public async Task<IActionResult> LoginUser(LoginUserRequest userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _usersService.Login(userLogin.passwordHash, userLogin.email);

            return Ok(result);
          
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest userRegistr)
        {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

              var result=  await _usersService.Register(userRegistr.UserName, userRegistr.PasswordHash, userRegistr.Email);
            return Ok(result);
        }
      
       

    }
}
