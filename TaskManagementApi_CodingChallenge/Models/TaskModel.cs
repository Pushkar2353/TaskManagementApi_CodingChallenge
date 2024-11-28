using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi_CodingChallenge.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title must not exceed 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Due Date is required")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Priority Level is required")]
        [StringLength(15, ErrorMessage = "Priority Level must not exceed 15 characters.")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "Task Status is required")]
        [StringLength(30, ErrorMessage = "Task Status must not exceed 30 characters.")]
        public string Status { get; set; }

        public TaskModel() { }
        public TaskModel(string title, string description, DateTime dueDate, string priority, string status)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = status;
        }
    }
}
