using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieInfoLibrary.Domain.Interfaces;
using MovieInfoLibrary.Domain.Models;
using MovieInfoLibrary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MovieInfoLibrary.Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieInfoDbContext context) : base(context) { }

        public override async Task<List<Movie>> GetAll()
        {
            return await Db.Movies.AsNoTracking().Include(b => b.Genre)
                .OrderBy(b => b.MovieTitle)
                .ToListAsync();
        }

        public override async Task<Movie> GetById(int id)
        {
            return await Db.Movies.AsNoTracking().Include(b => b.Genre)
                .Where(b => b.MovieId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId)
        {
            return await Search(b => b.GenreId == genreId);
        }

        public async Task<IEnumerable<Movie>> SearchMovieWithGenre(string searchedValue)
        {
            return await Db.Movies.AsNoTracking()
                .Include(b => b.Genre)
                .Where(b => b.MovieTitle.Contains(searchedValue) || 
                            b.Director.Contains(searchedValue) ||
                            b.Description.Contains(searchedValue) ||
                            b.Genre.MovieTitle.Contains(searchedValue))
                .ToListAsync();
        }
    }
}