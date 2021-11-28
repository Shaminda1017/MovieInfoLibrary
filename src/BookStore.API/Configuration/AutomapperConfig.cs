using AutoMapper;
using MovieInfoLibrary.API.Dtos.Book;
using MovieInfoLibrary.API.Dtos.Category;
using MovieInfoLibrary.Domain.Models;

namespace MovieInfoLibrary.API.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Genre, GenreAddDto>().ReverseMap();
            CreateMap<Genre, GenreEditDto>().ReverseMap();
            CreateMap<Genre, GenreResultDto>().ReverseMap();
            CreateMap<Movie, MovieAddDto>().ReverseMap();
            CreateMap<Movie, MovieEditDto>().ReverseMap();
            CreateMap<Movie, MovieResultDto>().ReverseMap();
        }
    }
}