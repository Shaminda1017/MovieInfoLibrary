using System;
using System.Collections.Generic;
using MovieInfoLibrary.Domain.Interfaces;
using MovieInfoLibrary.Domain.Models;
using MovieInfoLibrary.Domain.Services;
using Moq;
using Xunit;

namespace MovieInfoLibrary.Domain.Tests
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly MovieInfoService _bookService;

        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _bookService = new MovieInfoService(_movieRepositoryMock.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfBook_WhenMoviesExist()
        {
            var books = CreateMovieList();

            _movieRepositoryMock.Setup(c => c.GetAll()).ReturnsAsync(books);

            var result = await _bookService.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<Movie>>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnNull_WhenMoviesDoNotExist()
        {
            _movieRepositoryMock.Setup(c => c.GetAll())
                .ReturnsAsync((List<Movie>)null);

            var result = await _bookService.GetAll();

            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromRepository_OnlyOnce()
        {
            _movieRepositoryMock.Setup(c => c.GetAll())
                .ReturnsAsync(new List<Movie>());

            await _bookService.GetAll();

            _movieRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnMovie_WhenBookExist()
        {
            var book = CreateMovie();

            _movieRepositoryMock.Setup(c => c.GetById(book.MovieId))
                .ReturnsAsync(book);

            var result = await _bookService.GetById(book.MovieId);

            Assert.NotNull(result);
            Assert.IsType<Movie>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenMovieDoesNotExist()
        {
            _movieRepositoryMock.Setup(c => c.GetById(1))
                .ReturnsAsync((Movie)null);

            var result = await _bookService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromRepository_OnlyOnce()
        {
            _movieRepositoryMock.Setup(c => c.GetById(1))
                .ReturnsAsync(new Movie());

            await _bookService.GetById(1);

            _movieRepositoryMock.Verify(mock => mock.GetById(1), Times.Once);
        }

        [Fact]
        public async void GetMoviesByCategory_ShouldReturnAListOfMovie_WhenMoviesWithSearchedCategoryExist()
        {
            var bookList = CreateMovieList();

            _movieRepositoryMock.Setup(c => c.GetMoviesByGenre(2))
                .ReturnsAsync(bookList);

            var result = await _bookService.GetMoviesByGenre(2);

            Assert.NotNull(result);
            Assert.IsType<List<Movie>>(result);
        }

        [Fact]
        public async void GetMoviesByGenre_ShouldReturnNull_WhenMoviesWithSearchedGenreDoNotExist()
        {
            _movieRepositoryMock.Setup(c => c.GetMoviesByGenre(2))
                .ReturnsAsync((IEnumerable<Movie>)null);

            var result = await _bookService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetMoviesByGenre_ShouldCallGetMoviesByGenreFromRepository_OnlyOnce()
        {
            var bookList = CreateMovieList();

            _movieRepositoryMock.Setup(c => c.GetMoviesByGenre(2))
                .ReturnsAsync(bookList);

            await _bookService.GetMoviesByGenre(2);

            _movieRepositoryMock.Verify(mock => mock.GetMoviesByGenre(2), Times.Once);
        }

        [Fact]
        public async void Search_ShouldReturnAListOfMovie_WhenMoviesWithSearchedNameExist()
        {
            var bookList = CreateMovieList();
            var searchedBook = CreateMovie();
            var bookName = searchedBook.MovieTitle;

            _movieRepositoryMock.Setup(c =>
                c.Search(c => c.MovieTitle.Contains(bookName))).ReturnsAsync(bookList);

            var result = await _bookService.Search(searchedBook.MovieTitle);

            Assert.NotNull(result);
            Assert.IsType<List<Movie>>(result);
        }

        [Fact]
        public async void Search_ShouldReturnNull_WhenMoviesWithSearchedNameDoNotExist()
        {
            var searchedBook = CreateMovie();
            var bookName = searchedBook.MovieTitle;

            _movieRepositoryMock.Setup(c =>
                    c.Search(c => c.MovieTitle.Contains(bookName)))
                .ReturnsAsync((IEnumerable<Movie>)(null));

            var result = await _bookService.Search(searchedBook.MovieTitle);

            Assert.Null(result);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromRepository_OnlyOnce()
        {
            var bookList = CreateMovieList();
            var searchedBook = CreateMovie();
            var bookName = searchedBook.MovieTitle;

            _movieRepositoryMock.Setup(c =>
                    c.Search(c => c.MovieTitle.Contains(bookName)))
                .ReturnsAsync(bookList);

            await _bookService.Search(searchedBook.MovieTitle);

            _movieRepositoryMock.Verify(mock => mock.Search(c => c.MovieTitle.Contains(bookName)), Times.Once);
        }

        [Fact]
        public async void SearchMovieWithGenre_ShouldReturnAListOfMovie_WhenMoviesWithSearchedGenreExist()
        {
            var bookList = CreateMovieList();
            var searchedBook = CreateMovie();

            _movieRepositoryMock.Setup(c =>
                c.SearchMovieWithGenre(searchedBook.MovieTitle))
                .ReturnsAsync(bookList);

            var result = await _bookService.SearchMovieWithGenre(searchedBook.MovieTitle);

            Assert.NotNull(result);
            Assert.IsType<List<Movie>>(result);
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldReturnNull_WhenBooksWithSearchedCategoryDoNotExist()
        {
            var searchedBook = CreateMovie();

            _movieRepositoryMock.Setup(c =>
                c.SearchMovieWithGenre(searchedBook.MovieTitle))
                .ReturnsAsync((IEnumerable<Movie>)null);

            var result = await _bookService.SearchMovieWithGenre(searchedBook.MovieTitle);

            Assert.Null(result);
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldCallSearchBookWithCategoryFromRepository_OnlyOnce()
        {
            var bookList = CreateMovieList();
            var searchedBook = CreateMovie();

            _movieRepositoryMock.Setup(c =>
                    c.SearchMovieWithGenre(searchedBook.MovieTitle))
                .ReturnsAsync(bookList);

            await _bookService.SearchMovieWithGenre(searchedBook.MovieTitle);

            _movieRepositoryMock.Verify(mock => mock.SearchMovieWithGenre(searchedBook.MovieTitle), Times.Once);
        }

        [Fact]
        public async void Add_ShouldAddBook_WhenBookNameDoesNotExist()
        {
            var book = CreateMovie();

            _movieRepositoryMock.Setup(c =>
                c.Search(c => c.MovieTitle == book.MovieTitle))
                .ReturnsAsync(new List<Movie>());
            _movieRepositoryMock.Setup(c => c.Add(book));

            var result = await _bookService.Add(book);

            Assert.NotNull(result);
            Assert.IsType<Movie>(result);
        }

        [Fact]
        public async void Add_ShouldNotAddBook_WhenBookNameAlreadyExist()
        {
            var book = CreateMovie();
            var bookList = new List<Movie>() { book };

            _movieRepositoryMock.Setup(c =>
                c.Search(c => c.MovieTitle == book.MovieTitle))
                .ReturnsAsync(bookList);

            var result = await _bookService.Add(book);

            Assert.Null(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromRepository_OnlyOnce()
        {
            var book = CreateMovie();

            _movieRepositoryMock.Setup(c =>
                    c.Search(c => c.MovieTitle == book.MovieTitle))
                .ReturnsAsync(new List<Movie>());
            _movieRepositoryMock.Setup(c => c.Add(book));

            await _bookService.Add(book);

            _movieRepositoryMock.Verify(mock => mock.Add(book), Times.Once);
        }

        [Fact]
        public async void Update_ShouldUpdateBook_WhenBookNameDoesNotExist()
        {
            var book = CreateMovie();

            _movieRepositoryMock.Setup(c =>
                c.Search(c => c.MovieTitle == book.MovieTitle && c.MovieId != book.MovieId))
                .ReturnsAsync(new List<Movie>());
            _movieRepositoryMock.Setup(c => c.Update(book));

            var result = await _bookService.Update(book);

            Assert.NotNull(result);
            Assert.IsType<Movie>(result);
        }

        [Fact]
        public async void Update_ShouldNotUpdateBook_WhenBookDoesNotExist()
        {
            var book = CreateMovie();
            var bookList = new List<Movie>()
            {
                new Movie()
                {
                    MovieId = 2,
                    MovieTitle = "Book Test 2",
                    Director = "Director Test 2"
                }
            };

            _movieRepositoryMock.Setup(c =>
                c.Search(c => c.MovieTitle == book.MovieTitle && c.MovieId != book.MovieId))
                .ReturnsAsync(bookList);

            var result = await _bookService.Update(book);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldCallAddFromRepository_OnlyOnce()
        {
            var book = CreateMovie();

            _movieRepositoryMock.Setup(c =>
                    c.Search(c => c.MovieTitle == book.MovieTitle && c.MovieId != book.MovieId))
                .ReturnsAsync(new List<Movie>());

            await _bookService.Update(book);

            _movieRepositoryMock.Verify(mock => mock.Update(book), Times.Once);
        }

        [Fact]
        public async void Remove_ShouldReturnTrue_WhenBookCanBeRemoved()
        {
            var book = CreateMovie();

            var result = await _bookService.Remove(book);

            Assert.True(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromRepository_OnlyOnce()
        {
            var book = CreateMovie();

            await _bookService.Remove(book);

            _movieRepositoryMock.Verify(mock => mock.Remove(book), Times.Once);
        }

        private Movie CreateMovie()
        {
            return new Movie()
            {
                MovieId = 1,
                MovieTitle = "Book Test",
                Director = "Director Test",
                Description = "Description Test",
                Price = 10,
                GenreId = 1,
                Release = DateTime.MinValue.AddYears(40)
            };
        }

        private List<Movie> CreateMovieList()
        {
            return new List<Movie>()
            {
                new Movie()
                {
                    MovieId = 1,
                    MovieTitle = "Book Test 1",
                    Director = "Director Test 1",
                    Description = "Description Test 1",
                    Price = 10,
                    GenreId = 1
                },
                new Movie()
                {
                    MovieId = 2,
                    MovieTitle = "Book Test 2",
                    Director = "Director Test 2",
                    Description = "Description Test 2",
                    Price = 20,
                    GenreId = 1
                },
                new Movie()
                {
                    MovieId = 3,
                    MovieTitle = "Book Test 3",
                    Director = "Director Test 3",
                    Description = "Description Test 3",
                    Price = 30,
                    GenreId = 2
                }
            };
        }
    }
}