using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieInfoLibrary.API.Dtos.Book;
using MovieInfoLibrary.Domain.Interfaces;
using MovieInfoLibrary.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieInfoLibrary.API.Controllers
{
    [Route("api/[controller]")]
    public class MoviesController : MainController
    {
        private readonly IMovieService _bookService;
        private readonly IMapper _mapper;

        public MoviesController(IMapper mapper,
                                IMovieService movieService)
        {
            _mapper = mapper;
            _bookService = movieService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAll();

            return Ok(_mapper.Map<IEnumerable<MovieResultDto>>(books));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetById(id);

            if (book == null) return NotFound();

            return Ok(_mapper.Map<MovieResultDto>(book));
        }

        [HttpGet]
        [Route("get-books-by-genreId/{genreId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMoviesByGenre(int genreId)
        {
            var books = await _bookService.GetMoviesByGenre(genreId);

            if (!books.Any()) return NotFound();

            return Ok(_mapper.Map<IEnumerable<MovieResultDto>>(books));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(MovieAddDto bookDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var book = _mapper.Map<Movie>(bookDto);
            var bookResult = await _bookService.Add(book);

            if (bookResult == null) return BadRequest();

            return Ok(_mapper.Map<MovieResultDto>(bookResult));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, MovieEditDto bookDto)
        {
            if (id != bookDto.MovieId) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            await _bookService.Update(_mapper.Map<Movie>(bookDto));

            return Ok(bookDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove(int id)
        {
            var book = await _bookService.GetById(id);
            if (book == null) return NotFound();

            await _bookService.Remove(book);

            return Ok();
        }

        [HttpGet]
        [Route("search/{bookName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Movie>>> Search(string bookName)
        {
            var books = _mapper.Map<List<Movie>>(await _bookService.Search(bookName));

            if (books == null || books.Count == 0) return NotFound("None book was founded");

            return Ok(books);
        }

        [HttpGet]
        [Route("search-book-with-genreId/{searchedValue}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Movie>>> SearchMovieWithGenre(string searchedValue)
        {
            var books = _mapper.Map<List<Movie>>(await _bookService.SearchMovieWithGenre(searchedValue));

            if (!books.Any()) return NotFound("None book was founded");

            return Ok(_mapper.Map<IEnumerable<MovieResultDto>>(books));
        }
    }
}