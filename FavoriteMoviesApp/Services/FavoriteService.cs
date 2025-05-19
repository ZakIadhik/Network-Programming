using FavoriteMoviesApp.Data;
using FavoriteMoviesApp.DTOs;
using FavoriteMoviesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FavoriteMoviesApp.Services
{
    public class FavoriteService
    {
        private readonly AppDbContext _context;

        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToFavorites(int userId, int movieId)
        {
            if (await _context.Favorites.AnyAsync(f => f.UserId == userId && f.MovieId == movieId))
            {
                Console.WriteLine("Movie is already in favorites.");
                return false;
            }

            if (!await _context.Movies.AnyAsync(m => m.Id == movieId))
            {
                Console.WriteLine($"Movie with ID {movieId} does not exist.");
                return false;
            }

            var favorite = new Favorite
            {
                UserId = userId,
                MovieId = movieId
            };

            _context.Favorites.Add(favorite);
            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("Movie added to favorites successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding movie to favorites: {ex.Message}");
                return false;
            }
        }

        public async Task<List<MovieDto>> GetUserFavorites(int userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Movie)
                .Select(f => new MovieDto
                {
                    Id = f.Movie.Id,
                    Title = f.Movie.Title,
                    Genre = f.Movie.Genre,
                    Year = f.Movie.Year,
                    Director = f.Movie.Director,
                    Plot = f.Movie.Plot,
                    PosterUrl = f.Movie.PosterUrl
                })
                .ToListAsync();
        }

        public async Task<bool> RemoveFromFavorites(int userId, int movieId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == movieId);

            if (favorite == null)
            {
                Console.WriteLine("Movie is not in favorites.");
                return false;
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            Console.WriteLine("Movie removed from favorites successfully.");
            return true;
        }
    }
}