using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.DTOs
{
    public class TodoDto
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = null!;
        [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string? Description { get; set; }
        public TodoStatus Status { get; set; }
        public TodoPriority Priority { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Due date must be a valid date.")]
        public DateTime? DueDate { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Creation date must be a valid date.")]
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Modification date must be a valid date.")]
        public DateTime LastModifiedDate { get; set; }
    }
}
