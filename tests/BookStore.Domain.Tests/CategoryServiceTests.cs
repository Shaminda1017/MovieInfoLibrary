using System.Collections.Generic;
using MovieInfoLibrary.Domain.Interfaces;
using MovieInfoLibrary.Domain.Models;
using MovieInfoLibrary.Domain.Services;
using Moq;
using Xunit;

namespace MovieInfoLibrary.Domain.Tests
{
    public class CategoryServiceTests
    {
        private readonly Mock<IGenreRepository> _categoryRepositoryMock;
        private readonly Mock<IMovieService> _bookService;
        private readonly GenreService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<IGenreRepository>();
            _bookService = new Mock<IMovieService>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _bookService.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfGenres_WhenGenresExist()
        {
            var categories = CreateCategoryList();

            _categoryRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync(categories);

            var result = await _categoryService.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<Genre>>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnNull_WhenGenresDoNotExist()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync((List<Genre>)null);

            var result = await _categoryService.GetAll();

            Assert.Null(result);
        }
        
        [Fact]
        public async void GetAll_ShouldCallGetAllFromRepository_OnlyOnce()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync((List<Genre>)null);

            await _categoryService.GetAll();

            _categoryRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnGenre_WhenGenreExist()
        {
            var genreId = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                c.GetById(genreId.MovieId)).ReturnsAsync(genreId);

            var result = await _categoryService.GetById(genreId.MovieId);

            Assert.NotNull(result);
            Assert.IsType<Genre>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenGenreDoesNotExist()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetById(1)).ReturnsAsync((Genre)null);

            var result = await _categoryService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromRepository_OnlyOnce()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetById(1)).ReturnsAsync((Genre)null);

            await _categoryService.GetById(1);

            _categoryRepositoryMock.Verify(mock => mock.GetById(1), Times.Once);
        }

        [Fact]
        public async void Add_ShouldAddCategory_WhenGenreNameDoesNotExist()
        {
            var genreId = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                c.Search(c => c.MovieTitle == genreId.MovieTitle))
                .ReturnsAsync(new List<Genre>());
            _categoryRepositoryMock.Setup(c => c.Add(genreId));

            var result = await _categoryService.Add(genreId);
            
            Assert.NotNull(result);
            Assert.IsType<Genre>(result);
        }

        [Fact]
        public async void Add_ShouldNotAddCategory_WhenGenreNameAlreadyExist()
        {
            var genreId = CreateCategory();
            var categoryList = new List<Genre>() { genreId };

            _categoryRepositoryMock.Setup(c =>
                c.Search(c => c.MovieTitle == genreId.MovieTitle)).ReturnsAsync(categoryList);

            var result = await _categoryService.Add(genreId);

            Assert.Null(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromRepository_OnlyOnce()
        {
            var genreId = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                    c.Search(c => c.MovieTitle == genreId.MovieTitle))
                .ReturnsAsync(new List<Genre>());
            _categoryRepositoryMock.Setup(c => c.Add(genreId));

            await _categoryService.Add(genreId);

            _categoryRepositoryMock.Verify(mock => mock.Add(genreId), Times.Once);
        }

        [Fact]
        public async void Update_ShouldUpdateCategory_WhenGenreNameDoesNotExist()
        {
            var genreId = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                c.Search(c => c.MovieTitle == genreId.MovieTitle && c.MovieId != genreId.MovieId))
                .ReturnsAsync(new List<Genre>());
            _categoryRepositoryMock.Setup(c => c.Update(genreId));

            var result = await _categoryService.Update(genreId);

            Assert.NotNull(result);
            Assert.IsType<Genre>(result);
        }
        
        [Fact]
        public async void Update_ShouldNotUpdateCategory_WhenGenreDoesNotExist()
        {
            var genreId = CreateCategory();
            var categoryList = new List<Genre>()
            {
                new Genre()
                {
                    MovieId = 2,
                    MovieTitle = "Genre MovieTitle 2"
                }
            };

            _categoryRepositoryMock.Setup(c => 
                    c.Search(c => c.MovieTitle == genreId.MovieTitle && c.MovieId != genreId.MovieId))
                .ReturnsAsync(categoryList);

            var result = await _categoryService.Update(genreId);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromRepository_OnlyOnce()
        {
            var genreId = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                    c.Search(c => c.MovieTitle == genreId.MovieTitle && c.MovieId != genreId.MovieId))
                .ReturnsAsync(new List<Genre>());

            await _categoryService.Update(genreId);

            _categoryRepositoryMock.Verify(mock => mock.Update(genreId), Times.Once);
        }

        [Fact]
        public async void Remove_ShouldRemoveCategory_WhenGenreDoNotHaveRelatedBooks()
        {
            var genreId = CreateCategory();

            _bookService.Setup(b =>
                b.GetMoviesByGenre(genreId.MovieId)).ReturnsAsync(new List<Movie>());

            var result = await _categoryService.Remove(genreId);

            Assert.True(result);
        }

        [Fact]
        public async void Remove_ShouldNotRemoveCategory_WhenGenreHasRelatedBooks()
        {
            var genreId = CreateCategory();

            var books = new List<Movie>()
            {
                new Movie()
                {
                    MovieId = 1,
                    MovieTitle = "Test MovieTitle 1",
                    Director = "Test Director 1",
                    GenreId = genreId.MovieId
                }
            };

            _bookService.Setup(b => b.GetMoviesByGenre(genreId.MovieId)).ReturnsAsync(books);

            var result = await _categoryService.Remove(genreId);

            Assert.False(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromRepository_OnlyOnce()
        {
            var genreId = CreateCategory();

            _bookService.Setup(b =>
                b.GetMoviesByGenre(genreId.MovieId)).ReturnsAsync(new List<Movie>());

            await _categoryService.Remove(genreId);

            _categoryRepositoryMock.Verify(mock => mock.Remove(genreId), Times.Once);
        }


        [Fact]
        public async void Search_ShouldReturnAListOfGenre_WhenGenresWithSearchedNameExist()
        {
            var categoryList = CreateCategoryList();
            var searchedCategory = CreateCategory();
            var genreName = searchedCategory.MovieTitle;

            _categoryRepositoryMock.Setup(c =>
                c.Search(c => c.MovieTitle.Contains(genreName)))
                .ReturnsAsync(categoryList);

                var result = await _categoryService.Search(searchedCategory.MovieTitle);

            Assert.NotNull(result);
            Assert.IsType<List<Genre>>(result);
        }

        [Fact]
        public async void Search_ShouldReturnNull_WhenGenresWithSearchedNameDoNotExist()
        {
            var searchedCategory = CreateCategory();
            var genreName = searchedCategory.MovieTitle;

            _categoryRepositoryMock.Setup(c =>
                c.Search(c => c.MovieTitle.Contains(genreName)))
                .ReturnsAsync((IEnumerable<Genre>)(null));

            var result = await _categoryService.Search(searchedCategory.MovieTitle);

            Assert.Null(result);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromRepository_OnlyOnce()
        {
            var categoryList = CreateCategoryList();
            var searchedCategory = CreateCategory();
            var genreName = searchedCategory.MovieTitle;

            _categoryRepositoryMock.Setup(c =>
                    c.Search(c => c.MovieTitle.Contains(genreName)))
                .ReturnsAsync(categoryList);

            await _categoryService.Search(searchedCategory.MovieTitle);

            _categoryRepositoryMock.Verify(mock => mock.Search(c => c.MovieTitle.Contains(genreName)), Times.Once);
        }

        private Genre CreateCategory()
        {
            return new Genre()
            {
                MovieId = 1,
                MovieTitle = "Genre MovieTitle 1"
            };
        }

        private List<Genre> CreateCategoryList()
        {
            return new List<Genre>()
            {
                new Genre()
                {
                    MovieId = 1,
                    MovieTitle = "Genre MovieTitle 1"
                },
                new Genre()
                {
                    MovieId = 2,
                    MovieTitle = "Genre MovieTitle 2"
                },
                new Genre()
                {
                    MovieId = 3,
                    MovieTitle = "Genre MovieTitle 3"
                }
            };
        }
    }
}