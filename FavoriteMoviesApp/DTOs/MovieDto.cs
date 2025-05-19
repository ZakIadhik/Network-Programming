namespace FavoriteMoviesApp.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Director { get; set; } = string.Empty;
        public string Plot { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
    }
}