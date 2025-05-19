using FavoriteMoviesApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FavoriteMoviesApp.Data.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.AddedAt).IsRequired();

            builder.HasIndex(f => new { f.UserId, f.MovieId }).IsUnique();
        }
    }
}