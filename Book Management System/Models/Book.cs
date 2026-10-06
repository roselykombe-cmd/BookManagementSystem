using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN is required")]
        public string ISBN { get; set; } = string.Empty;

        [Display(Name = "Published Year")]
        public int PublishedYear { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        public string Genre { get; set; } = string.Empty;
    }
}
