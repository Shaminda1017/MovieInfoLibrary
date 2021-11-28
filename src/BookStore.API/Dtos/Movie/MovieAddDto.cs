using System;
using System.ComponentModel.DataAnnotations;

namespace MovieInfoLibrary.API.Dtos.Book
{
    public class MovieAddDto
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public int GenreId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string MovieTitle { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string Director { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public DateTime Release { get; set; }
    }
}