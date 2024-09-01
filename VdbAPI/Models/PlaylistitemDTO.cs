namespace VdbAPI.Models
{
    public class PlaylistitemDTO
    {
        public int PlayListId { get; set; }
        public int VideoId { get; set; }
        public int VideoPosition { get; set; }
        public string VideoName { get; set; }
        public int? ThumbnailId { get; set; }
        public int? Episode { get; set; }
    }
}
