using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Portal.Data.Models;
using Task_Portal.Services.IServices;

namespace Task_Portal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks([FromQuery] TaskQueryParameters queryParameters)
        {
            var tasks = await _taskService.GetTasksAsync(queryParameters);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetTask(int id)
        {
            var task = await _taskService.GetsTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return task;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Tasks>> CreateTask(Tasks task)
        {
            var createdTask = await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(int id, Tasks task)
        {
            var updatedTask = await _taskService.UpdateTaskAsync(id, task);
            if (updatedTask == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusRequest request)
        {
            await _taskService.UpdateTaskStatusAsync(id, request.Status, request.Progress);
            return NoContent();
        }

        [HttpPost("{taskId}/assign")]
        public async Task<IActionResult> AssignTask(int taskId, [FromBody] AssignTaskRequest request)
        {
            await _taskService.AssignTaskAsync(taskId, request.UserId);
            return NoContent();
        }

        [HttpPatch("{taskId}/acceptance")]
        public async Task<IActionResult> UpdateTaskAcceptance(int taskId, [FromBody] UpdateTaskAcceptanceRequest request)
        {
            await _taskService.UpdateTaskAcceptanceAsync(taskId, request.IsAccepted);
            return NoContent();
        }

        //[HttpGet("categories")]
        //public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        //{
        //    var categories = await _taskService.GetCategoriesAsync();
        //    return Ok(categories);
        //}

        //[HttpGet("category/{categoryId}/tasks")]
        //public async Task<ActionResult<IEnumerable<Tasks>>> GetTasksByCategory(int categoryId, [FromQuery] TaskQueryParameters queryParameters)
        //{
        //    var tasks = await _taskService.GetTasksByCategoryAsync(categoryId, queryParameters);
        //    return Ok(tasks);
        //}

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasksByCategory(int categoryId)
        {
            var tasks = await _taskService.GetTasksByCategoryAsync(categoryId);
            return Ok(tasks);
        }

        [HttpPut("{taskId}/assign-category")]
        public async Task<IActionResult> AssignCategory(int taskId, [FromBody] AssignCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _taskService.AssignCategoryAsync(taskId, request.CategoryId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
