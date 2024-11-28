using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi_CodingChallenge.DTOs
{
    public class TaskModelDTO
    {
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title must not exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Due Date is required")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Priority Level is required")]
        [StringLength(15, ErrorMessage = "Priority Level must not exceed 15 characters.")]
        public string Priority { get; set; } = string.Empty;

        [Required(ErrorMessage = "Task Status is required")]
        [StringLength(30, ErrorMessage = "Task Status must not exceed 30 characters.")]
        public string Status { get; set; } = string.Empty;
    }
}
