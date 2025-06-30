using FavoriteMoviesApp.Data;
using FavoriteMoviesApp.DTOs;
using FavoriteMoviesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FavoriteMoviesApp.Services
{
    public class MovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieDto>> SearchMovies(string? title = null, string? genre = null, int? year = null)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(m => m.Title.ToLower().Contains(title.ToLower()));

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(m => m.Genre.ToLower().Contains(genre.ToLower()));

            if (year.HasValue)
                query = query.Where(m => m.Year == year);

            return await query.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Genre = m.Genre,
                Year = m.Year,
                Director = m.Director,
                Plot = m.Plot,
                PosterUrl = m.PosterUrl
            }).ToListAsync();
        }

        public async Task<MovieDto?> GetMovieById(int id)
        {
            return await _context.Movies
                .Where(m => m.Id == id)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre,
                    Year = m.Year,
                    Director = m.Director,
                    Plot = m.Plot,
                    PosterUrl = m.PosterUrl
                })
                .FirstOrDefaultAsync();
        }
    }
}