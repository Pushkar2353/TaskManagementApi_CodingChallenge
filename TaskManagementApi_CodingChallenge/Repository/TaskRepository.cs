using Microsoft.EntityFrameworkCore;
using TaskManagementApi_CodingChallenge.Data;
using TaskManagementApi_CodingChallenge.DTOs;
using TaskManagementApi_CodingChallenge.Models;

namespace TaskManagementApi_CodingChallenge.Repository
{
    public interface ITaskRepository
    {
        Task<List<TaskModelDTO>> GetAllTasksAsync();
        Task<TaskModelDTO> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskModelDTO taskDto);
        Task UpdateTaskAsync(int id, TaskModelDTO taskDto);
        Task DeleteTaskAsync(int id);
    }

    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskModelDTO>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Select(task => new TaskModelDTO
                {
                    TaskId = task.TaskId,
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    Priority = task.Priority,
                    Status = task.Status
                })
                .ToListAsync();
        }

        public async Task<TaskModelDTO> GetTaskByIdAsync(int id)
        {
            var task = await _context.Tasks
                .Where(t => t.TaskId == id)
                .Select(t => new TaskModelDTO
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Priority = t.Priority,
                    Status = t.Status
                })
                .FirstOrDefaultAsync();

            return task;
        }

        public async Task AddTaskAsync(TaskModelDTO taskDto)
        {
            // Map TaskModelDTO to TaskModel
            var task = new TaskModel
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Priority = taskDto.Priority,
                Status = taskDto.Status
            };

            // Add the TaskModel entity to the DbSet
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(int id, TaskModelDTO taskDto)
        {
            var existingTask = await _context.Tasks.FindAsync(id);

            if (existingTask != null)
            {
                existingTask.Title = taskDto.Title;
                existingTask.Description = taskDto.Description;
                existingTask.DueDate = taskDto.DueDate;
                existingTask.Priority = taskDto.Priority;
                existingTask.Status = taskDto.Status;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }

}
