namespace FavoriteMoviesApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Director { get; set; } = string.Empty;
        public string Plot { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public List<Favorite> Favorites { get; set; } = new();
    }
}