using MovieInfoLibrary.Domain.Interfaces;
using MovieInfoLibrary.Domain.Models;
using MovieInfoLibrary.Infrastructure.Context;

namespace MovieInfoLibrary.Infrastructure.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieInfoDbContext context) : base(context) { }
    }
}