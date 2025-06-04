using LibraryAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Application.DTOs
{
    public class BookDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        [Required]
        public int PublicationYear { get; set; }
        [Required]
        public string Genre { get; set; } = string.Empty;
        [Required]
        public BookStatus Status { get; set; }
    }
} 