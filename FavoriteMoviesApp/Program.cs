using FavoriteMoviesApp.Data;
using FavoriteMoviesApp.Services;
using FavoriteMoviesApp.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FavoriteMoviesApp
{
    class Program
    {
        private static IServiceProvider _serviceProvider = null!;
        private static UserDto? _currentUser = null;

        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<AuthService>();
            services.AddScoped<MovieService>();
            services.AddScoped<FavoriteService>();
            services.AddScoped<PdfService>();
            services.AddScoped<EmailService>();

            _serviceProvider = services.BuildServiceProvider();

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await DbInitializer.Initialize(context);
            }

            await RunApplication();
        }

        static async Task RunApplication()
        {
            while (true)
            {
                if (_currentUser == null)
                {
                    Console.WriteLine("\n=== Favorite Movies App ===");
                    Console.WriteLine("1. Register");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("3. Exit");
                    Console.Write("Select an option: ");
                    var option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            await Register();
                            break;
                        case "2":
                            await Login();
                            break;
                        case "3":
                            Console.WriteLine("Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid option. Try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"\n=== Welcome, {_currentUser.Name}! ===");
                    Console.WriteLine("1. Search Movies");
                    Console.WriteLine("2. View Favorites");
                    Console.WriteLine("3. Add Movie to Favorites");
                    Console.WriteLine("4. Remove Movie from Favorites");
                    Console.WriteLine("5. Export Favorites to Email");
                    Console.WriteLine("6. Logout");
                    Console.Write("Select an option: ");
                    var option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            await SearchMovies();
                            break;
                        case "2":
                            await ViewFavorites();
                            break;
                        case "3":
                            await AddToFavorites();
                            break;
                        case "4":
                            await RemoveFromFavorites();
                            break;
                        case "5":
                            await ExportFavorites();
                            break;
                        case "6":
                            _currentUser = null;
                            Console.WriteLine("Logged out successfully.");
                            break;
                        default:
                            Console.WriteLine("Invalid option. Try again.");
                            break;
                    }
                }
            }
        }

        static async Task Register()
        {
            Console.Write("Enter name: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Enter email: ");
            var email = Console.ReadLine() ?? "";
            Console.Write("Enter password: ");
            var password = Console.ReadLine() ?? "";

            using var scope = _serviceProvider.CreateScope();
            var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

            var registerDto = new RegisterDto
            {
                Name = name,
                Email = email,
                Password = password
            };

            var user = await authService.Register(registerDto);
            if (user == null)
            {
                Console.WriteLine("Registration failed. Email already exists.");
            }
            else
            {
                Console.WriteLine("Registration successful!");
            }
        }

        static async Task Login()
        {
            Console.Write("Enter email: ");
            var email = Console.ReadLine() ?? "";
            Console.Write("Enter password: ");
            var password = Console.ReadLine() ?? "";

            using var scope = _serviceProvider.CreateScope();
            var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

            var loginDto = new LoginDto
            {
                Email = email,
                Password = password
            };

            var user = await authService.Login(loginDto);
            if (user == null)
            {
                Console.WriteLine("Login failed. Invalid email or password.");
            }
            else
            {
                _currentUser = user;
                Console.WriteLine($"Welcome, {user.Name}!");
            }
        }

        static async Task SearchMovies()
        {
            Console.Write("Enter title (or leave empty): ");
            var title = Console.ReadLine();
            Console.Write("Enter genre (or leave empty): ");
            var genre = Console.ReadLine();
            Console.Write("Enter year (or leave empty): ");
            var yearInput = Console.ReadLine();
            int? year = string.IsNullOrEmpty(yearInput) ? null : int.Parse(yearInput);

            using var scope = _serviceProvider.CreateScope();
            var movieService = scope.ServiceProvider.GetRequiredService<MovieService>();

            var movies = await movieService.SearchMovies(title, genre, year);
            if (movies.Count == 0)
            {
                Console.WriteLine("No movies found.");
                return;
            }

            foreach (var movie in movies)
            {
                Console.WriteLine($"\nID: {movie.Id}");
                Console.WriteLine($"Title: {movie.Title}");
                Console.WriteLine($"Genre: {movie.Genre}");
                Console.WriteLine($"Year: {movie.Year}");
                Console.WriteLine($"Director: {movie.Director}");
                Console.WriteLine($"Plot: {movie.Plot}");
                Console.WriteLine($"Poster: {movie.PosterUrl}");
                Console.WriteLine("---");
            }
        }

        static async Task ViewFavorites()
        {
            using var scope = _serviceProvider.CreateScope();
            var favoriteService = scope.ServiceProvider.GetRequiredService<FavoriteService>();

            var favorites = await favoriteService.GetUserFavorites(_currentUser!.Id);
            if (favorites.Count == 0)
            {
                Console.WriteLine("No favorite movies.");
                return;
            }

            foreach (var movie in favorites)
            {
                Console.WriteLine($"\nID: {movie.Id}");
                Console.WriteLine($"Title: {movie.Title}");
                Console.WriteLine($"Genre: {movie.Genre}");
                Console.WriteLine($"Year: {movie.Year}");
                Console.WriteLine($"Director: {movie.Director}");
                Console.WriteLine($"Plot: {movie.Plot}");
                Console.WriteLine($"Poster: {movie.PosterUrl}");
                Console.WriteLine("---");
            }
        }

        static async Task AddToFavorites()
        {
            try
            {
                Console.Write("Enter movie ID to add to favorites: ");
                if (!int.TryParse(Console.ReadLine(), out var movieId))
                {
                    Console.WriteLine("Invalid movie ID. Please enter a number.");
                    return;
                }

                using var scope = _serviceProvider.CreateScope();
                var favoriteService = scope.ServiceProvider.GetRequiredService<FavoriteService>();

                var success = await favoriteService.AddToFavorites(_currentUser!.Id, movieId);
                if (!success)
                {
                    Console.WriteLine("Failed to add movie to favorites. It may already be in favorites or the movie doesn't exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task RemoveFromFavorites()
        {
            Console.Write("Enter movie ID to remove from favorites: ");
            if (!int.TryParse(Console.ReadLine(), out var movieId))
            {
                Console.WriteLine("Invalid movie ID.");
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var favoriteService = scope.ServiceProvider.GetRequiredService<FavoriteService>();

            var success = await favoriteService.RemoveFromFavorites(_currentUser!.Id, movieId);
            Console.WriteLine(success ? "Movie removed from favorites!" : "Failed to remove movie. It may not be in favorites.");
        }

        static async Task ExportFavorites()
        {
            using var scope = _serviceProvider.CreateScope();
            var favoriteService = scope.ServiceProvider.GetRequiredService<FavoriteService>();
            var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

            var favorites = await favoriteService.GetUserFavorites(_currentUser!.Id);
            if (favorites.Count == 0)
            {
                Console.WriteLine("No favorite movies to export.");
                return;
            }

            try
            {
                await emailService.SendFavoritesEmail(_currentUser!.Email, _currentUser!.Name, favorites);
                Console.WriteLine("Favorites exported and sent to your email!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}