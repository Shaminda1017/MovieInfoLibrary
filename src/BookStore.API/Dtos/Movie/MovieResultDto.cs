using System;

namespace MovieInfoLibrary.API.Dtos.Book
{
    public class MovieResultDto
    {
        public int MovieId { get; set; }

        public int GenreId { get; set; }

        public string GenreName { get; set; }

        public string MovieTitle { get; set; }

        public string Director { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public DateTime Release { get; set; }
    }
}