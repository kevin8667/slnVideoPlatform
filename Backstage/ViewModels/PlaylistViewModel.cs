using Backstage.Models;

namespace Backstage.ViewModels
{
    public class PlaylistViewModel
    {
        public IEnumerable<PlayList> PlayLists { get; set; } = new List<PlayList>();
        public IEnumerable<PlayListCollaborator> PlayListCollaborators { get; set; } = new List<PlayListCollaborator>();
        public IEnumerable<PlayListItem> PlayListItems { get; set; } = new List<PlayListItem>();
        public IEnumerable<MemberPlayList> MemberPlayLists { get; set; } = new List<MemberPlayList>();
    }
}

