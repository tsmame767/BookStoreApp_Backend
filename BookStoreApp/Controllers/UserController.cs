using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;

namespace BookStoreApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userService;
        private readonly IEmailServiceBL _emailService;
        private readonly ICacheServiceBL _cacheService;

        public UserController(IUserBL userService, IEmailServiceBL emailService, ICacheServiceBL cacheService)
        {
            _userService = userService;
            _emailService = emailService;
            _cacheService = cacheService;
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
                return Ok("User Not Registered");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                    return Ok("User Login SuccessFull\nToken: "+res);
                }
                return Ok("User Does Not Exist!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            try
            {
                var res = await _userService.ForgotPassword(Email); // Assuming this returns the OTP string
                var htmlBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>BookStore Validation</title>
            </head>
            <body>
                <h1>Your OTP for Password Reset</h1>
                <p>Hello,</p>
                <p>You have requested to reset your password. Please use the following One-Time Password (OTP) to proceed with resetting your password. This OTP is only valid for the next 5 minutes:</p>
                <h2 style='color: red;'>{res}</h2> <!-- Here the OTP is dynamically inserted -->
                <p>Best regards,<br>Your Application Team</p>
            </body>
            </html>";

                var SendMail = await _emailService.SendEmailAsync(Email, "Password Reset", htmlBody); // Ensure that the HTML body is passed here
                if (string.IsNullOrEmpty(res) || SendMail == 0)
                {
                    return Ok("No User with this Email Found!");
                }
                return Ok("Email has been sent Successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword(PasswordResetModel Validation)
        {
            var res = await this._userService.ResetPassword(Validation);
            if (res == false)
            {
                return Ok("Failed to reset password!");
            }
            return Ok("Password Reset Succesfull!");
        }



    }
}
