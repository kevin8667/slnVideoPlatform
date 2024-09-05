namespace VdbAPI.Models
{
    public class PlaylistDTO
    {
        public int PlayListId { get; set; }
        public string PlayListName { get; set; }
        public string PlayListDescription { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int AddedCount { get; set; }
        public int SharedCount { get; set; }
        public string ShowImage { get; set; }
    }
}
