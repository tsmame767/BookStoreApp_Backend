using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;

namespace BookStoreApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userService;

        public UserController(IUserBL userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> UserRegisteration(UserRegisteration Credentials)
        {
            try
            {
                var res = await _userService.UserRegister(Credentials);
                if (res)
                {
                    return Ok("User Registered Successfully");
                }
                return BadRequest("User Not Registered");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin(UserLoginModel Credentials)
        {
            try
            {
                var res = await _userService.UserLogin(Credentials);
                if (res != null)
                {
                    return Ok(res);
                }
                return BadRequest(res);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
