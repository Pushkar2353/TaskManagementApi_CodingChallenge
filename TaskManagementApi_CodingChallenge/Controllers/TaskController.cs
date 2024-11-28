using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementApi_CodingChallenge.Data;
using TaskManagementApi_CodingChallenge.DTOs;
using TaskManagementApi_CodingChallenge.Models;
using TaskManagementApi_CodingChallenge.Services;

namespace TaskManagementApi_CodingChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                _logger.LogInformation("Fetching all tasks.");
                var tasks = await _taskService.GetAllTasksAsync();
                _logger.LogInformation("Successfully fetched {Count} tasks.", tasks.Count);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching tasks.");
                return StatusCode(500, "An error occurred while fetching tasks.");
            }
        }

        [HttpGet("GetTaskById/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                _logger.LogInformation("Fetching task with ID: {Id}.", id);
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    _logger.LogWarning("Task with ID: {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully fetched task with ID: {Id}.", id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching task with ID: {Id}.", id);
                return StatusCode(500, "An error occurred while fetching the task.");
            }
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] TaskModelDTO taskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid task data submitted.");
                    return BadRequest(ModelState);
                }

                await _taskService.AddTaskAsync(taskDto);
                _logger.LogInformation("Task with ID: {Id} successfully added.", taskDto.TaskId);
                return CreatedAtAction(nameof(GetTask), new { id = taskDto.TaskId }, taskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new task.");
                return StatusCode(500, "An error occurred while adding the task.");
            }
        }

        [HttpPut("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModelDTO taskDto)
        {
            try
            {
                if (id != taskDto.TaskId || !ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid update request for Task ID: {Id}.", id);
                    return BadRequest();
                }

                await _taskService.UpdateTaskAsync(id, taskDto);
                _logger.LogInformation("Task with ID: {Id} successfully updated.", id);
                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task with ID: {Id}.", id);
                return StatusCode(500, "An error occurred while updating the task.");
            }
        }

        [HttpDelete("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                _logger.LogInformation("Deleting task with ID: {Id}.", id);
                await _taskService.DeleteTaskAsync(id);
                _logger.LogInformation("Task with ID: {Id} successfully deleted.", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task with ID: {Id}.", id);
                return StatusCode(500, "An error occurred while deleting the task.");
            }
        }
    }
}
