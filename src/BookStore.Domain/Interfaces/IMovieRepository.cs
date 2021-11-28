using System.Collections.Generic;
using System.Threading.Tasks;
using MovieInfoLibrary.Domain.Models;

namespace MovieInfoLibrary.Domain.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        new Task<List<Movie>> GetAll();
        new Task<Movie> GetById(int id);
        Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId);
        Task<IEnumerable<Movie>> SearchMovieWithGenre(string searchedValue);
    }
}