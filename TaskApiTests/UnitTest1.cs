using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagementApi_CodingChallenge.Controllers;
using TaskManagementApi_CodingChallenge.Data;
using TaskManagementApi_CodingChallenge.DTOs;
using TaskManagementApi_CodingChallenge.Models;
using TaskManagementApi_CodingChallenge.Services;

namespace TaskApiTests
{
    [TestFixture]
    public class TaskControllerTests
    {
        private Mock<ITaskService> _mockTaskService;
        private TaskController _controller;

        [SetUp]
        public void Setup()
        {
            _mockTaskService = new Mock<ITaskService>();
            _controller = new TaskController(_mockTaskService.Object, Mock.Of<ILogger<TaskController>>());
        }

        [Test]
        public async Task GetAllTasks_ShouldReturnOkResult_WhenTasksExist()
        {
            var tasks = new List<TaskModelDTO>
            {
                new TaskModelDTO { TaskId = 1, Title = "Task 1", Description = "Description 1", DueDate = DateTime.Now.AddDays(1), Priority = "High", Status = "Pending" },
                new TaskModelDTO { TaskId = 2, Title = "Task 2", Description = "Description 2", DueDate = DateTime.Now.AddDays(2), Priority = "Medium", Status = "In Progress" }
            };

            _mockTaskService.Setup(service => service.GetAllTasksAsync()).ReturnsAsync(tasks);

            var result = await _controller.GetAllTasks();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnValue = okResult.Value as IEnumerable<TaskModelDTO>;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(tasks.Count, returnValue.Count());
        }

        [Test]
        public async Task GetTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            var taskId = 999;
            _mockTaskService.Setup(service => service.GetTaskByIdAsync(taskId)).ReturnsAsync((TaskModelDTO)null);

            var result = await _controller.GetTask(taskId);

            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task AddTask_ShouldReturnCreatedResult_WhenTaskIsAdded()
        {
            var taskDto = new TaskModelDTO
            {
                TaskId = 1,
                Title = "New Task",
                Description = "Task Description",
                DueDate = DateTime.Now.AddDays(3),
                Priority = "Low",
                Status = "Pending"
            };

            _mockTaskService.Setup(service => service.AddTaskAsync(It.IsAny<TaskModelDTO>())).Returns(Task.CompletedTask);

            var result = await _controller.AddTask(taskDto);

            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            Assert.AreEqual("GetTask", createdAtActionResult.ActionName);
        }

        [Test]
        public async Task UpdateTask_ShouldReturnOkResult_WhenTaskIsUpdated()
        {
            var updatedTaskDto = new TaskModelDTO
            {
                TaskId = 1,
                Title = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(5),
                Priority = "High",
                Status = "Completed"
            };

            _mockTaskService.Setup(service => service.UpdateTaskAsync(It.IsAny<int>(), It.IsAny<TaskModelDTO>())).Returns(Task.CompletedTask);

            var result = await _controller.UpdateTask(1, updatedTaskDto);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task DeleteTask_ShouldReturnOkResult_WhenTaskIsDeleted()
        {
            var taskId = 1;
            _mockTaskService.Setup(service => service.DeleteTaskAsync(taskId)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteTask(taskId);

            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task AddTask_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            var invalidTaskDto = new TaskModelDTO
            {
                TaskId = 0, // Invalid taskId (could be used as a trigger for invalid data)
                Title = "", // Invalid title
                Description = "Description",
                DueDate = DateTime.Now.AddDays(3),
                Priority = "Low",
                Status = "Pending"
            };

            _controller.ModelState.AddModelError("Title", "Title is required");

            var result = await _controller.AddTask(invalidTaskDto);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task UpdateTask_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            var invalidTaskDto = new TaskModelDTO
            {
                TaskId = 1,
                Title = "", // Invalid title
                Description = "Description",
                DueDate = DateTime.Now.AddDays(3),
                Priority = "Low",
                Status = "Pending"
            };

            _controller.ModelState.AddModelError("Title", "Title is required");

            var result = await _controller.UpdateTask(1, invalidTaskDto);

            var badRequestResult = result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }
    }
}