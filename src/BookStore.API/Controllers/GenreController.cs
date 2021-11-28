using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MovieInfoLibrary.API.Dtos.Genre;
using MovieInfoLibrary.Domain.Interfaces;
using MovieInfoLibrary.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieInfoLibrary.API.Controllers
{
    [Route("api/[controller]")]
    public class GenreController : MainController
    {
        private readonly IGenreService _categoryService;
        private readonly IMapper _mapper;

        public GenreController(IMapper mapper,
                                    IGenreService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();

            return Ok(_mapper.Map<IEnumerable<GenreResultDto>>(categories));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var genreId = await _categoryService.GetById(id);

            if (genreId == null) return NotFound();

            return Ok(_mapper.Map<GenreResultDto>(genreId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(GenreAddDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var genreId = _mapper.Map<Genre>(categoryDto);
            var categoryResult = await _categoryService.Add(genreId);

            if (categoryResult == null) return BadRequest();

            return Ok(_mapper.Map<GenreResultDto>(categoryResult));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, GenreEditDto categoryDto)
        {
            if (id != categoryDto.MovieId) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            await _categoryService.Update(_mapper.Map<Genre>(categoryDto));

            return Ok(categoryDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove(int id)
        {
            var genreId = await _categoryService.GetById(id);
            if (genreId == null) return NotFound();

            var result = await _categoryService.Remove(genreId);

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [Route("search/{genreId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Genre>>> Search(string genreId)
        {
            var categories = _mapper.Map<List<Genre>>(await _categoryService.Search(genreId));

            if (categories == null || categories.Count == 0)
                return NotFound("None genreId was founded");

            return Ok(categories);
        }
    }
}