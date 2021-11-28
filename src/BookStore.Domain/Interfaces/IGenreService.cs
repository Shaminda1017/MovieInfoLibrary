using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieInfoLibrary.Domain.Models;

namespace MovieInfoLibrary.Domain.Interfaces
{
    public interface ICategoryService : IDisposable
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetById(int id);
        Task<Genre> Add(Genre genreId);
        Task<Genre> Update(Genre genreId);
        Task<bool> Remove(Genre genreId);
        Task<IEnumerable<Genre>> Search(string genreName);
    }
}