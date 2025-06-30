using FavoriteMoviesApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FavoriteMoviesApp.Data.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Title).IsRequired().HasMaxLength(200);
            builder.Property(m => m.Genre).HasMaxLength(100);
            builder.Property(m => m.Year);
            builder.Property(m => m.Director).HasMaxLength(100);
            builder.Property(m => m.Plot).HasMaxLength(500);
            builder.Property(m => m.PosterUrl).HasMaxLength(300);

            builder.HasMany(m => m.Favorites)
                .WithOne(f => f.Movie)
                .HasForeignKey(f => f.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}