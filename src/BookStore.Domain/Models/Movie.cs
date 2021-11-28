using System;

namespace MovieInfoLibrary.Domain.Models
{
    public class Movie : Entity
    {
        public string MovieTitle { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime Release { get; set; }
        public int GenreId { get; set; }

        /* EF Relation */
        public Genre Genre { get; set; }
    }
}