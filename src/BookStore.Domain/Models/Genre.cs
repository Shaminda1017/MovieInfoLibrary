using System.Collections.Generic;

namespace MovieInfoLibrary.Domain.Models
{
    public class Genre : Entity
    {
        public string MovieTitle { get; set; }

        /* EF Relations */
        public IEnumerable<Movie> Movies { get; set; }
    }
}