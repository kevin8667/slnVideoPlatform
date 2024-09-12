using Newtonsoft.Json;

namespace VdbAPI.Models
{
    public class PlayListCreateDTO
    {        
        public PlayList PlayList { get; set; }
        [JsonProperty("videoIds")]
        public List<int> VideoIds { get; set; } = new List<int>();
        [JsonProperty("collaboratorIds")]
        public List<int> CollaboratorIds { get; set; } = new List<int>();
    }
}