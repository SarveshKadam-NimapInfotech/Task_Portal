﻿using Task_Portal.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Portal.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Logout()
        {
            var claims = User.Claims;
            var shortLivedToken = await _userService.LogoutAsync(claims);

            return Ok(new { Token = shortLivedToken });
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

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpGet("all")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _userService.GetAllUsersAsync();
        //    return Ok(users);
        //}
    }
}
