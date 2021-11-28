using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieInfoLibrary.Domain.Interfaces;
using MovieInfoLibrary.Domain.Models;

namespace MovieInfoLibrary.Domain.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _categoryRepository;
        private readonly IMovieService _bookService;

        public GenreService(IGenreRepository genreRepository, IMovieService movieService)
        {
            _categoryRepository = genreRepository;
            _bookService = movieService;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Genre> GetById(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public async Task<Genre> Add(Genre genreId)
        {
            if (_categoryRepository.Search(c => c.MovieTitle == genreId.MovieTitle).Result.Any())
                return null;

            await _categoryRepository.Add(genreId);
            return genreId;
        }

        public async Task<Genre> Update(Genre genreId)
        {
            if (_categoryRepository.Search(c => c.MovieTitle == genreId.MovieTitle && c.MovieId != genreId.MovieId).Result.Any())
                return null;

            await _categoryRepository.Update(genreId);
            return genreId;
        }

        public async Task<bool> Remove(Genre genreId)
        {
            var books = await _bookService.GetMoviesByGenre(genreId.MovieId);
            if (books.Any()) return false;

            await _categoryRepository.Remove(genreId);
            return true;
        }

        public async Task<IEnumerable<Genre>> Search(string genreName)
        {
            return await _categoryRepository.Search(c => c.MovieTitle.Contains(genreName));
        }

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }
    }
}