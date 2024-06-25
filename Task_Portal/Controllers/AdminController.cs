using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Portal.Data.Models;

namespace Task_Portal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
    //    private readonly ApplicationDbContext _context;

    //    public AdminController(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

    //    [HttpGet("users")]
    //    public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    //    {
    //        var users = await _context.Users
    //            .Skip((page - 1) * pageSize)
    //            .Take(pageSize)
    //            .ToListAsync();

    //        return Ok(users);
    //    }

    //    [HttpGet("export-users")]
    //    public async Task<IActionResult> ExportUsers()
    //    {
    //        var users = await _context.Users.ToListAsync();

    //        var stream = new MemoryStream();
    //        using (var package = new ExcelPackage(stream))
    //        {
    //            var worksheet = package.Workbook.Worksheets.Add("Users");
    //            worksheet.Cells.LoadFromCollection(users, true);
    //            package.Save();
    //        }
    //        stream.Position = 0;
    //        var content = stream.ToArray();
    //        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Users.xlsx");
    //    }

    //    [HttpGet("export-tasks")]
    //    public async Task<IActionResult> ExportTasks()
    //    {
    //        var tasks = await _context.Tasks.Include(t => t.Category).ToListAsync();

    //        var stream = new MemoryStream();
    //        using (var package = new ExcelPackage(stream))
    //        {
    //            var worksheet = package.Workbook.Worksheets.Add("Tasks");
    //            worksheet.Cells.LoadFromCollection(tasks, true);
    //            package.Save();
    //        }
    //        stream.Position = 0;
    //        var content = stream.ToArray();
    //        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tasks.xlsx");
    //    }

    //    [HttpPost("import-categories")]
    //    public async Task<IActionResult> ImportCategories([FromForm] IFormFile file)
    //    {
    //        using (var stream = new MemoryStream())
    //        {
    //            await file.CopyToAsync(stream);
    //            using (var package = new ExcelPackage(stream))
    //            {
    //                var worksheet = package.Workbook.Worksheets[0];
    //                var rowCount = worksheet.Dimension.Rows;

    //                for (int row = 2; row <= rowCount; row++)
    //                {
    //                    var categoryName = worksheet.Cells[row, 1].Value?.ToString();
    //                    if (!string.IsNullOrEmpty(categoryName))
    //                    {
    //                        _context.Categories.Add(new Category { Name = categoryName });
    //                    }
    //                }
    //            }
    //        }
    //        await _context.SaveChangesAsync();
    //        return Ok();
    //    }

    //    [HttpPost("import-users")]
    //    public async Task<IActionResult> ImportUsers([FromForm] IFormFile file)
    //    {
    //        using (var stream = new MemoryStream())
    //        {
    //            await file.CopyToAsync(stream);
    //            using (var package = new ExcelPackage(stream))
    //            {
    //                var worksheet = package.Workbook.Worksheets[0];
    //                var rowCount = worksheet.Dimension.Rows;

    //                for (int row = 2; row <= rowCount; row++)
    //                {
    //                    var username = worksheet.Cells[row, 1].Value?.ToString();
    //                    var email = worksheet.Cells[row, 2].Value?.ToString();
    //                    var password = worksheet.Cells[row, 3].Value?.ToString(); // You may want to hash this before saving
    //                    var dateOfBirth = DateTime.Parse(worksheet.Cells[row, 4].Value?.ToString());
    //                    var country = worksheet.Cells[row, 5].Value?.ToString();
    //                    var state = worksheet.Cells[row, 6].Value?.ToString();
    //                    var city = worksheet.Cells[row, 7].Value?.ToString();

    //                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email))
    //                    {
    //                        _context.Users.Add(new User
    //                        {
    //                            Username = username,
    //                            Email = email,
    //                            Password = password,
    //                            DateOfBirth = dateOfBirth,
    //                            Country = country,
    //                            State = state,
    //                            City = city
    //                        });
    //                    }
    //                }
    //            }
    //        }
    //        await _context.SaveChangesAsync();
    //        return Ok();
    //    }

    //    [HttpPost("users/{userId}/roles/{roleId}")]
    //    public async Task<IActionResult> AssignRole(int userId, int roleId)
    //    {
    //        var user = await _context.Users.FindAsync(userId);
    //        var role = await _context.Roles.FindAsync(roleId);

    //        if (user == null || role == null)
    //        {
    //            return NotFound();
    //        }

    //        var userRole = new UserRole
    //        {
    //            UserId = userId,
    //            RoleId = roleId
    //        };

    //        _context.UserRoles.Add(userRole);
    //        await _context.SaveChangesAsync();

    //        return Ok();
    //    }

    //    [HttpDelete("users/{userId}/roles/{roleId}")]
    //    public async Task<IActionResult> RemoveRole(int userId, int roleId)
    //    {
    //        var userRole = await _context.UserRoles
    //            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

    //        if (userRole == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.UserRoles.Remove(userRole);
    //        await _context.SaveChangesAsync();

    //        return Ok();
    //    }

    //    [HttpPost("categories")]
    //    public async Task<IActionResult> CreateCategory(Category category)
    //    {
    //        _context.Categories.Add(category);
    //        await _context.SaveChangesAsync();
    //        return Ok(category);
    //    }

    //    [HttpPut("categories/{id}")]
    //    public async Task<IActionResult> UpdateCategory(int id, Category category)
    //    {
    //        var existingCategory = await _context.Categories.FindAsync(id);
    //        if (existingCategory == null)
    //        {
    //            return NotFound();
    //        }

    //        existingCategory.Name = category.Name;
    //        await _context.SaveChangesAsync();

    //        return Ok(existingCategory);
    //    }

    //    [HttpDelete("categories/{id}")]
    //    public async Task<IActionResult> DeleteCategory(int id)
    //    {
    //        var category = await _context.Categories.FindAsync(id);
    //        if (category == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.Categories.Remove(category);
    //        await _context.SaveChangesAsync();

    //        return Ok();
    //    }
    }
}
