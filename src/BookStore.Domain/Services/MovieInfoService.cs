using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieInfoLibrary.Domain.Interfaces;
using MovieInfoLibrary.Domain.Models;

namespace MovieInfoLibrary.Domain.Services
{
    public class MovieInfoService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieInfoService(IMovieRepository bookRepository)
        {
            _movieRepository = bookRepository;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await _movieRepository.GetAll();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _movieRepository.GetById(id);
        }

        public async Task<Movie> Add(Movie book)
        {
            if (_movieRepository.Search(b => b.MovieTitle == book.MovieTitle).Result.Any())
                return null;

            await _movieRepository.Add(book);
            return book;
        }

        public async Task<Movie> Update(Movie book)
        {
            if (_movieRepository.Search(b => b.MovieTitle == book.MovieTitle && b.MovieId != book.MovieId).Result.Any())
                return null;

            await _movieRepository.Update(book);
            return book;
        }

        public async Task<bool> Remove(Movie book)
        {
            await _movieRepository.Remove(book);
            return true;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId)
        {
            return await _movieRepository.GetMoviesByGenre(genreId);
        }

        public async Task<IEnumerable<Movie>> Search(string bookName)
        {
            return await _movieRepository.Search(c => c.MovieTitle.Contains(bookName));
        }

        public async Task<IEnumerable<Movie>> SearchMovieWithGenre(string searchedValue)
        {
            return await _movieRepository.SearchMovieWithGenre(searchedValue);
        }

        public void Dispose()
        {
            _movieRepository?.Dispose();
        }
    }
}