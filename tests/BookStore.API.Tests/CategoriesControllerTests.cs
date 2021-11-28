using System.Collections.Generic;
using AutoMapper;
using BookStore.API.Controllers;
using BookStore.API.Dtos.Category;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookStore.API.Tests
{
    public class CategoriesControllerTests
    {
        private readonly GenreController _categoriesController;
        private readonly Mock<IGenreService> _categoryServiceMock;
        private readonly Mock<IMapper> _mapperMock;

        public CategoriesControllerTests()
        {
            _categoryServiceMock = new Mock<IGenreService>();
            _mapperMock = new Mock<IMapper>();
            _categoriesController = new GenreController(_mapperMock.Object, _categoryServiceMock.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenExistCategory()
        {
            var categories = CreateCategoryList();
            var dtoExpected = MapModelToCategoryListDto(categories);

            _categoryServiceMock.Setup(c => c.GetAll()).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<IEnumerable<GenreResultDto>>(
                It.IsAny<List<Genre>>())).Returns(dtoExpected);

            var result = await _categoriesController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenDoesNotExistAnyCategory()
        {
            var categories = new List<Genre>();
            var dtoExpected = MapModelToCategoryListDto(categories);

            _categoryServiceMock.Setup(c => c.GetAll()).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<IEnumerable<GenreResultDto>>(It.IsAny<List<Genre>>())).Returns(dtoExpected);

            var result = await _categoriesController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromService_OnlyOnce()
        {
            var categories = CreateCategoryList();
            var dtoExpected = MapModelToCategoryListDto(categories);

            _categoryServiceMock.Setup(c => c.GetAll()).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<IEnumerable<GenreResultDto>>(It.IsAny<List<Genre>>())).Returns(dtoExpected);

            await _categoriesController.GetAll();

            _categoryServiceMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnOk_WhenCategoryExist()
        {
            var category = CreateCategory();
            var dtoExpected = MapModelToCategoryResultDto(category);

            _categoryServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<GenreResultDto>(It.IsAny<Genre>())).Returns(dtoExpected);

            var result = await _categoriesController.GetById(2);
         
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            _categoryServiceMock.Setup(c => c.GetById(2)).ReturnsAsync((Genre)null);

            var result = await _categoriesController.GetById(2);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromService_OnlyOnce()
        {
            var category = CreateCategory();
            var dtoExpected = MapModelToCategoryResultDto(category);

            _categoryServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<GenreResultDto>(It.IsAny<Genre>())).Returns(dtoExpected);

            await _categoriesController.GetById(2);

            _categoryServiceMock.Verify(mock => mock.GetById(2), Times.Once);
        }

        [Fact]
        public async void Add_ShouldReturnOk_WhenCategoryIsAdded()
        {
            var category = CreateCategory();
            var categoryAddDto = new GenreAddDto() { Name = category.Name };
            var categoryResultDto = MapModelToCategoryResultDto(category);

            _mapperMock.Setup(m => m.Map<Genre>(It.IsAny<GenreAddDto>())).Returns(category);
            _categoryServiceMock.Setup(c => c.Add(category)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<GenreResultDto>(It.IsAny<Genre>())).Returns(categoryResultDto);

            var result = await _categoriesController.Add(categoryAddDto);
           
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Add_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var categoryAddDto = new GenreAddDto();
            _categoriesController.ModelState.AddModelError("Name", "The field name is required");

            var result = await _categoriesController.Add(categoryAddDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Add_ShouldReturnBadRequest_WhenCategoryResultIsNull()
        {
            var category = CreateCategory();
            var categoryAddDto = new GenreAddDto() { Name = category.Name };

            _mapperMock.Setup(m => m.Map<Genre>(It.IsAny<GenreAddDto>())).Returns(category);
            _categoryServiceMock.Setup(c => c.Add(category)).ReturnsAsync((Genre)null);

            var result = await _categoriesController.Add(categoryAddDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromService_OnlyOnce()
        {
            var category = CreateCategory();
            var categoryAddDto = new GenreAddDto() { Name = category.Name };

            _mapperMock.Setup(m => m.Map<Genre>(It.IsAny<GenreAddDto>())).Returns(category);
            _categoryServiceMock.Setup(c => c.Add(category)).ReturnsAsync(category);

            await _categoriesController.Add(categoryAddDto);

            _categoryServiceMock.Verify(mock => mock.Add(category), Times.Once);
        }

        [Fact]
        public async void Update_ShouldReturnOk_WhenCategoryIsUpdatedCorrectly()
        {
            var category = CreateCategory();
            var categoryEditDto = new GenreEditDto() { Id = category.Id,  Name = "Test" };

            _mapperMock.Setup(m => m.Map<Genre>(It.IsAny<GenreEditDto>())).Returns(category);
            _categoryServiceMock.Setup(c => c.GetById(category.Id)).ReturnsAsync(category);
            _categoryServiceMock.Setup(c => c.Update(category)).ReturnsAsync(category);

            var result = await _categoriesController.Update(categoryEditDto.Id, categoryEditDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenCategoryIdIsDifferentThenParameterId()
        {
            var categoryEditDto = new GenreEditDto() { Id = 1,  Name = "Test" };

            var result = await _categoriesController.Update(2, categoryEditDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var categoryEditDto = new GenreEditDto() { Id = 1 };
            _categoriesController.ModelState.AddModelError("Name", "The field name is required");

            var result = await _categoriesController.Update(1, categoryEditDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromService_OnlyOnce()
        {
            var category = CreateCategory();
            var categoryEditDto = new GenreEditDto() { Id = category.Id, Name = "Test" };

            _mapperMock.Setup(m => m.Map<Genre>(It.IsAny<GenreEditDto>())).Returns(category);
            _categoryServiceMock.Setup(c => c.GetById(category.Id)).ReturnsAsync(category);
            _categoryServiceMock.Setup(c => c.Update(category)).ReturnsAsync(category);

            await _categoriesController.Update(categoryEditDto.Id, categoryEditDto);

            _categoryServiceMock.Verify(mock => mock.Update(category), Times.Once);
        }

        [Fact]
        public async void Remove_ShouldReturnOk_WhenCategoryIsRemoved()
        {
            var category = CreateCategory();
            _categoryServiceMock.Setup(c => c.GetById(category.Id)).ReturnsAsync(category);
            _categoryServiceMock.Setup(c => c.Remove(category)).ReturnsAsync(true);

            var result = await _categoriesController.Remove(category.Id);
         
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void Remove_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            var category = CreateCategory();
            _categoryServiceMock.Setup(c => c.GetById(category.Id)).ReturnsAsync((Genre)null);

            var result = await _categoriesController.Remove(category.Id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Remove_ShouldReturnBadRequest_WhenResultIsFalse()
        {
            var category = CreateCategory();
            _categoryServiceMock.Setup(c => c.GetById(category.Id)).ReturnsAsync(category);
            _categoryServiceMock.Setup(c => c.Remove(category)).ReturnsAsync(false);

            var result = await _categoriesController.Remove(category.Id);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromService_OnlyOnce()
        {
            var category = CreateCategory();
            _categoryServiceMock.Setup(c => c.GetById(category.Id)).ReturnsAsync(category);
            _categoryServiceMock.Setup(c => c.Remove(category)).ReturnsAsync(true);

            await _categoriesController.Remove(category.Id);

            _categoryServiceMock.Verify(mock => mock.Remove(category), Times.Once);
        }

        [Fact]
        public async void Search_ShouldReturnOk_WhenCategoryWithSearchedNameExist()
        {
            var categoryList = CreateCategoryList();
            var category = CreateCategory();

            _categoryServiceMock.Setup(c => c.Search(category.Name))
                .ReturnsAsync(categoryList);
            _mapperMock.Setup(m => m.Map<List<Genre>>(It.IsAny<IEnumerable<Genre>>())).Returns(categoryList);

            var result = await _categoriesController.Search(category.Name);
            var actual = (OkObjectResult)result.Result;

            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async void Search_ShouldReturnNotFound_WhenCategoryWithSearchedNameDoesNotExist()
        {
            var category = CreateCategory();
            var categoryList = new List<Genre>();

            var dtoExpected = MapModelToCategoryResultDto(category);
            category.Name = dtoExpected.Name;

            _categoryServiceMock.Setup(c => c.Search(category.Name))
                .ReturnsAsync(categoryList);
            _mapperMock.Setup(m => m.Map<IEnumerable<Genre>>(It.IsAny<Genre>())).Returns(categoryList);

            var result = await _categoriesController.Search(category.Name);
            var actual = (NotFoundObjectResult)result.Result;

            Assert.IsType<NotFoundObjectResult>(actual);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromService_OnlyOnce()
        {
            var categoryList = CreateCategoryList();
            var category = CreateCategory();

            _categoryServiceMock.Setup(c => c.Search(category.Name))
                .ReturnsAsync(categoryList);
            _mapperMock.Setup(m => m.Map<List<Genre>>(It.IsAny<IEnumerable<Genre>>())).Returns(categoryList);

            await _categoriesController.Search(category.Name);

            _categoryServiceMock.Verify(mock => mock.Search(category.Name), Times.Once);
        }

        private Genre CreateCategory()
        {
            return new Genre()
            {
                Id = 2,
                Name = "Category Name 2"
            };
        }

        private GenreResultDto MapModelToCategoryResultDto(Genre category)
        {
            var categoryDto = new GenreResultDto()
            {
                Id = category.Id,
                Name = category.Name
            };
            return categoryDto;
        }

        private List<Genre> CreateCategoryList()
        {
            return new List<Genre>()
            {
                new Genre()
                {
                   Id = 1,
                   Name = "Category Name 1"
                },
                new Genre()
                {
                    Id = 2,
                    Name = "Category Name 2"
                },
                new Genre()
                {
                    Id = 3,
                    Name = "Category Name 3"
                }
            };
        }

        private List<GenreResultDto> MapModelToCategoryListDto(List<Genre> categories)
        {
            var listCategories = new List<GenreResultDto>();

            foreach (var item in categories)
            {
                var category = new GenreResultDto()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                listCategories.Add(category);
            }
            return listCategories;
        }
    }
}