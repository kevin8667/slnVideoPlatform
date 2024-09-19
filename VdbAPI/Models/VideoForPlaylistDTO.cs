namespace VdbAPI.Models
{
    public class VideoForPlaylistDTO
    {
        public int VideoId { get; set; }
        public string VideoName { get; set; }
        public int? Episode { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
