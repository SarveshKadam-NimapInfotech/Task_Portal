using Task_Portal.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Task_Portal.Services.IServices;
using System.Threading.Tasks;
using Task_Portal.Services.Services;

namespace Task_Portal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.RegisterUserAsync(request.Username, request.Email, request.Password, request.DateOfBirth, request.Country, request.State, request.City);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _userService.LoginAsync(request.Username, request.Password);
            if (token != null)
            {
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is missing." });
            }

            _userService.InvalidateToken(token);
            return Ok(new { message = "Successfully logged out." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var success = await _userService.ResetPasswordAsync(request.Username, request.NewPassword);
                if (success)
                {
                    return Ok();
                }
                return BadRequest("Failed to reset password.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("view-user")]
        public async Task<ActionResult<Users>> GetUserByIdAsync(string userId)
        {
            var userProfile = await _userService.GetUserByIdAsync(userId);
            if (userProfile == null)
            {
                return NotFound();
            }
            return userProfile;
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser(string userId, Users user)
        {
            var updatedUser = await _userService.UpdateUserAsync(userId, user);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return NoContent();
        }


        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpGet("all")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _userService.GetAllUsersAsync();
        //    return Ok(users);
        //}
    }
}
