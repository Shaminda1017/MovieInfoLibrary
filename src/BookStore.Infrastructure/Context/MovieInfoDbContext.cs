using System.Linq;
using MovieInfoLibrary.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieInfoLibrary.Infrastructure.Context
{
    public class MovieInfoDbContext : DbContext
    {
        public MovieInfoDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Genre> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(150)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieInfoDbContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}