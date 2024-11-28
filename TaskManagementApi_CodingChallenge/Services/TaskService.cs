using TaskManagementApi_CodingChallenge.DTOs;
using TaskManagementApi_CodingChallenge.Repository;

namespace TaskManagementApi_CodingChallenge.Services
{
    public interface ITaskService
    {
        Task<List<TaskModelDTO>> GetAllTasksAsync();
        Task<TaskModelDTO> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskModelDTO taskDto);
        Task UpdateTaskAsync(int id, TaskModelDTO taskDto);
        Task DeleteTaskAsync(int id);
    }

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<TaskModelDTO>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllTasksAsync();
        }

        public async Task<TaskModelDTO> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        public async Task AddTaskAsync(TaskModelDTO taskDto)
        {
            await _taskRepository.AddTaskAsync(taskDto);
        }

        public async Task UpdateTaskAsync(int id, TaskModelDTO taskDto)
        {
            await _taskRepository.UpdateTaskAsync(id, taskDto);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);
        }
    }


}
