using MovieInfoLibrary.Domain.Interfaces;
using MovieInfoLibrary.Domain.Services;
using MovieInfoLibrary.Infrastructure.Context;
using MovieInfoLibrary.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MovieInfoLibrary.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MovieInfoDbContext>();

            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();

            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IMovieService, MovieInfoService>();

            return services;
        }
    }
}