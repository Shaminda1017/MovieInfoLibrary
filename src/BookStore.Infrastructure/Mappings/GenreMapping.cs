using MovieInfoLibrary.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieInfoLibrary.Infrastructure.Mappings
{
    public class GenreMapping : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(c => c.MovieId);

            builder.Property(c => c.MovieTitle)
                .IsRequired()
                .HasColumnType("varchar(150)");

            // 1 : N => Category : Movies
            builder.HasMany(c => c.Movies)
                .WithOne(b => b.Category)
                .HasForeignKey(b => b.GenreId);

            builder.ToTable("Categories");
        }
    }
}