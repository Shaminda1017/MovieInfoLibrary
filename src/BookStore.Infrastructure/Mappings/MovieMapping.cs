using MovieInfoLibrary.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieInfoLibrary.Infrastructure.Mappings
{
    public class MovieMapping : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(b => b.MovieId);

            builder.Property(b => b.MovieTitle)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(b => b.Director)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(b => b.Description)
                .IsRequired(false)
                .HasColumnType("varchar(350)");

            builder.Property(b => b.Price)
                .IsRequired();

            builder.Property(b => b.Release)
                .IsRequired();

            builder.Property(b => b.GenreId)
                .IsRequired();

            builder.ToTable("Movies");
        }
    }
}