using FavoriteMoviesApp.Data;
using FavoriteMoviesApp.DTOs;
using FavoriteMoviesApp.Models;
using FavoriteMoviesApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace FavoriteMoviesApp.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return null;

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = PasswordHasher.HashPassword(registerDto.Password),
                Name = registerDto.Name
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<UserDto?> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
                return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}