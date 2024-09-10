namespace VdbAPI.Models
{
    public class PlayListCreateDTO
    {
        public PlayList PlayList { get; set; }
        public List<int> VideoIds { get; set; } = new List<int>();
        public List<int> CollaboratorIds { get; set; } = new List<int>();
    }
}