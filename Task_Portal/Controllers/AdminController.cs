using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Portal.Data.Models;
using Task_Portal.Services.IServices;
using OfficeOpenXml;
using System.IO;

namespace Task_Portal.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(int pageNumber, int pageSize)
        {
            var users = await _adminService.GetUsersAsync(pageNumber, pageSize);
            return Ok(users);
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _adminService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpPost("categories/import")]
        public async Task<IActionResult> ImportCategories([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            await _adminService.ImportCategoriesAsync(stream);
            return Ok();
        }

        [HttpPost("users/import")]
        public async Task<IActionResult> ImportUsers([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            await _adminService.ImportUsersAsync(stream);
            return Ok();
        }

        [HttpGet("users/export")]
        public async Task<IActionResult> ExportUsers()
        {
            var fileBytes = await _adminService.ExportUsersAsync();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Users.xlsx");
        }

        [HttpGet("tasks/export")]
        public async Task<IActionResult> ExportTasks()
        {
            var fileBytes = await _adminService.ExportTasksAsync();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tasks.xlsx");
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            await _adminService.CreateCategoryAsync(category);
            return Ok();
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _adminService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            await _adminService.UpdateCategoryAsync(id, category);
            return Ok();
        }

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _adminService.DeleteCategoryAsync(id);
            return Ok();
        }

        [HttpPost("assign-admin-role/{userId}")]
        public async Task<IActionResult> AssignAdminRole(int userId)
        {
            await _adminService.AssignAdminRoleAsync(userId);
            return Ok("Admin role assigned successfully.");
        }
    }
}
