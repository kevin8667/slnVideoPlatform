namespace VdbAPI.Models
{    
    public class VideoCreateDTO
    {
        public required string VideoName { get; set; }
        public int TypeId { get; set; }
        public int? SeriesId { get; set; }
        public int MainGenreId { get; set; }
        public int? SeasonId { get; set; }
        public int? Episode { get; set; }
        public string? RunningTime { get; set; }
        public bool IsShowing { get; set; }
        public DateTime? ReleaseDate { get; set; } 
        public decimal? Rating { get; set; }
        public decimal? Popularity { get; set; }
        public string? ThumbnailPath { get; set; }
        public string Lang { get; set; }
        public string? Summary { get; set; }
        public int? Views { get; set; }
        public string? AgeRating { get; set; }
        public string? TrailerUrl { get; set; }
        public string? Bgpath { get; set; }
        public List<ImageCreateDTO>? Images { get; set; } // List of images to be added
    }

    public class ImageCreateDTO
    {
        public string? ImagePath { get; set; }
    }
}
