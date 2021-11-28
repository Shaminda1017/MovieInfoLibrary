using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieInfoLibrary.Domain.Models;

namespace MovieInfoLibrary.Domain.Interfaces
{
    public interface IBookService : IDisposable
    {
        Task<IEnumerable<Movie>> GetAll();
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie book);
        Task<Movie> Update(Movie book);
        Task<bool> Remove(Movie book);
        Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId);
        Task<IEnumerable<Movie>> Search(string bookName);
        Task<IEnumerable<Movie>> SearchMovieWithGenre(string searchedValue);
    }
}