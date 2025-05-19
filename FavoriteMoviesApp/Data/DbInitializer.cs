using FavoriteMoviesApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FavoriteMoviesApp.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context)
        {
            try
            {
                Console.WriteLine("Creating database if it doesn't exist...");
                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("Database created or already exists.");

                if (!await context.Movies.AnyAsync())
                {
                    var movies = new List<Movie>
                    {
                        new() { Title = "The Shawshank Redemption", Genre = "Drama", Year = 1994, Director = "Frank Darabont", Plot = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BNDE3ODcxYzMtY2YzZC00NmNlLWJiNDMtZDViZWM2MzIxZDYwXkEyXkFqcGdeQXVyNjAwNDUxODI@._V1_.jpg" },
                        new() { Title = "The Godfather", Genre = "Crime", Year = 1972, Director = "Francis Ford Coppola", Plot = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BM2MyNjYxNmUtYTAwNi00MTYxLWJmNWYtYzZlODY3ZTk3OTFlXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_.jpg" },
                        new() { Title = "The Dark Knight", Genre = "Action", Year = 2008, Director = "Christopher Nolan", Plot = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_.jpg" },
                        new() { Title = "Pulp Fiction", Genre = "Crime", Year = 1994, Director = "Quentin Tarantino", Plot = "The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BNGNhMDIzZTUtNTBlZi00MTRlLWFjM2ItYzViMjE3YzI5MjljXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_.jpg" },
                        new() { Title = "Fight Club", Genre = "Drama", Year = 1999, Director = "David Fincher", Plot = "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much, much more.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMmEzNTkxYjQtZTc0MC00YTVjLTg5ZTEtZWMwOWVlYzY0NWIwXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_.jpg" },
                        new() { Title = "Inception", Genre = "Sci-Fi", Year = 2010, Director = "Christopher Nolan", Plot = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_.jpg" },
                        new() { Title = "The Matrix", Genre = "Sci-Fi", Year = 1999, Director = "Lana Wachowski, Lilly Wachowski", Plot = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg" },
                        new() { Title = "Goodfellas", Genre = "Biography", Year = 1990, Director = "Martin Scorsese", Plot = "The story of Henry Hill and his life in the mob, covering his relationship with his wife Karen and his mob partners Jimmy Conway and Tommy DeVito.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BY2NkZjEzMDgtN2RjYy00YzM1LWI4ZmQtMjIwYjFjNmI3ZGEwXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_.jpg" },
                        new() { Title = "The Silence of the Lambs", Genre = "Crime", Year = 1991, Director = "Jonathan Demme", Plot = "A young F.B.I. cadet must receive the help of an incarcerated and manipulative cannibal killer to help catch another serial killer.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BNjNhZTk0ZmEtNjJhMi00YzFlLWE1MmEtYzM1M2ZmMGMwMTU4XkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg" },
                        new() { Title = "Interstellar", Genre = "Adventure", Year = 2014, Director = "Christopher Nolan", Plot = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_.jpg" },
                        new() { Title = "The Lord of the Rings: The Fellowship of the Ring", Genre = "Adventure", Year = 2001, Director = "Peter Jackson", Plot = "A meek Hobbit from the Shire and eight companions set out on a journey to destroy the powerful One Ring.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BN2EyZjM3NzUtNWUzMi00MTgxLWI0NTctMzY4M2VlOTdjZWRiXkEyXkFqcGdeQXVyNDUzOTQ5MjY@._V1_.jpg" },
                        new() { Title = "Forrest Gump", Genre = "Drama", Year = 1994, Director = "Robert Zemeckis", Plot = "The presidencies of Kennedy and Johnson, the events of Vietnam, Watergate, and other historical events unfold through the perspective of an Alabama man.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BNWIwODRlZTUtY2U3ZS00Yzg1LWJhNzYtMmZiYmEyNmU1NjMzXkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_.jpg" },
                        new() { Title = "The Lord of the Rings: The Return of the King", Genre = "Adventure", Year = 2003, Director = "Peter Jackson", Plot = "Gandalf and Aragorn lead the World of Men against Sauron's army to draw his gaze from Frodo and Sam.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BNzA5ZDNlZWMtM2NhNS00NDJjLTk4NDItYTRmY2EwMWZlMTY3XkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_.jpg" },
                        new() { Title = "The Lord of the Rings: The Two Towers", Genre = "Adventure", Year = 2002, Director = "Peter Jackson", Plot = "While Frodo and Sam edge closer to Mordor with the help of the shifty Gollum, the divided fellowship makes a stand against Sauron's new ally.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BZGMxZTdjZmYtMmE2Ni00ZTdkLWI5NTgtNjlmMjBiNzU2MmI5XkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg" },
                        new() { Title = "Star Wars: Episode V - The Empire Strikes Back", Genre = "Action", Year = 1980, Director = "Irvin Kershner", Plot = "After the Rebels are brutally overpowered by the Empire on the ice planet Hoth, Luke Skywalker begins Jedi training with Yoda.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BYmU1NDRjNDgtMzhiMi00NjZmLTg5NGItZDNiZjU5NTU4OTE0XkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_.jpg" },
                        new() { Title = "The Green Mile", Genre = "Crime", Year = 1999, Director = "Frank Darabont", Plot = "The lives of guards on Death Row are affected by one of their charges: a black man accused of child murder and rape.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMTUxMzQyNjA5MF5BMl5BanBnXkFtZTYwOTU2NTY3._V1_.jpg" },
                        new() { Title = "The Prestige", Genre = "Drama", Year = 2006, Director = "Christopher Nolan", Plot = "After a tragic accident, two stage magicians engage in a battle to create the ultimate illusion.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMjA4NDI0MTIxNF5BMl5BanBnXkFtZTYwNTM0MzY2._V1_.jpg" },
                        new() { Title = "Gladiator", Genre = "Action", Year = 2000, Director = "Ridley Scott", Plot = "A former Roman General sets out to exact vengeance against the corrupt emperor who murdered his family.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMDliMmNhNDEtODUyOS00MjNlLTgxODEtN2U3NzIxMGVkZTA1L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg" },
                        new() { Title = "The Departed", Genre = "Crime", Year = 2006, Director = "Martin Scorsese", Plot = "An undercover cop and a mole in the police attempt to identify each other while infiltrating an Irish gang.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMTI1MTY2OTIxNV5BMl5BanBnXkFtZTYwNjQ4NjY3._V1_.jpg" },
                        new() { Title = "The Usual Suspects", Genre = "Crime", Year = 1995, Director = "Bryan Singer", Plot = "A sole survivor tells of the twisty events leading up to a horrific gun battle on a boat.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BYTViNjMyNmUtNDFkNC00ZDRlLThmMDUtZDU2YWE4NGI2ZjVmXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg" },
                        new() { Title = "Saving Private Ryan", Genre = "Drama", Year = 1998, Director = "Steven Spielberg", Plot = "Following the Normandy Landings, a group of U.S. soldiers go behind enemy lines to retrieve a paratrooper.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BZjhkMDM4MWItZTVjOC00ZDRhLThmYTAtM2I5NzBmNmNlMzI1XkEyXkFqcGdeQXVyNDYyMDk5MTU@._V1_.jpg" },
                        new() { Title = "Schindler's List", Genre = "Biography", Year = 1993, Director = "Steven Spielberg", Plot = "In German-occupied Poland during World War II, industrialist Oskar Schindler gradually becomes concerned for his Jewish workforce.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BNDE4OTMxMTctNmRhYy00NWE2LTg3YzItYTk3M2UwOTU5Njg4XkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg" },
                        new() { Title = "Se7en", Genre = "Crime", Year = 1995, Director = "David Fincher", Plot = "Two detectives, a rookie and a veteran, hunt a serial killer who uses the seven deadly sins as his motives.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BOTUwODM5MTctZjczMi00OTk4LTg3NWUtNmVhMTAzNTNjYjcyXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg" },
                        new() { Title = "The Lion King", Genre = "Animation", Year = 1994, Director = "Roger Allers, Rob Minkoff", Plot = "Lion prince Simba and his father are targeted by his bitter uncle, who wants to ascend the throne himself.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BYTYxNGMyZTYtMjE3MS00MzNjLWFjNmYtMDk3N2FmM2JiM2M1XkEyXkFqcGdeQXVyNjY5NDU4NzI@._V1_.jpg" },
                        new() { Title = "Memento", Genre = "Mystery", Year = 2000, Director = "Christopher Nolan", Plot = "A man with short-term memory loss attempts to track down his wife's murderer.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BZTcyNjk1MjgtOWI3Mi00YzQwLWI5MTktMzY4ZmI2NDAyNzYzXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg" },
                        new() { Title = "Django Unchained", Genre = "Western", Year = 2012, Director = "Quentin Tarantino", Plot = "With the help of a German bounty hunter, a freed slave sets out to rescue his wife from a brutal Mississippi plantation owner.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMjIyNTQ5NjQ1OV5BMl5BanBnXkFtZTcwODg1MDU4OA@@._V1_.jpg" },
                        new() { Title = "Spirited Away", Genre = "Animation", Year = 2001, Director = "Hayao Miyazaki", Plot = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches, and spirits.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMjlmZmI5MDctNDE2YS00YWE0LWE5ZWItZDBhYWQ0NTcxNWRhXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_.jpg" },
                        new() { Title = "The Big Lebowski", Genre = "Comedy", Year = 1998, Director = "Joel Coen, Ethan Coen", Plot = "Jeff 'The Dude' Lebowski, mistaken for a millionaire of the same name, seeks restitution for his ruined rug and enlists his bowling buddies to help get it.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMTQ0NjUzMDMyOV5BMl5BanBnXkFtZTcwODA1OTU2OQ@@._V1_.jpg" }
                    };

                    context.Movies.AddRange(movies);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Added {movies.Count} movies to the database.");
                }
                else
                {
                    Console.WriteLine("Movies already exist in the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw;
            }
        }
    }
}