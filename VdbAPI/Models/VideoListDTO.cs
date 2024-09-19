namespace VdbAPI.Models
{
    public class VideoListDTO
    {
        public int VideoId { get; set; }
        public string? VideoName { get; set; }
        public int TypeId { get; set; }
        public string? TypeName { get; set; } // 如果需要顯示 Type 名稱
        public string? Summary { get; set; }
        public int? SeriesId { get; set; }
        public string? SeriesName { get; set; } // 系列名稱
        public int? SeasonId { get; set; }
        public string? SeasonName { get; set; } // 季名稱
        public int MainGenreId { get; set; }
        public string? MainGenreName { get; set; } // 內容類型名稱
        public int? Episode { get; set; }
        public string? ThumbnailPath { get; set; }
        public string? Bgpath { get; set; }
    }
}
