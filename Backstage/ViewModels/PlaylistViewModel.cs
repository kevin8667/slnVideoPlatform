using Backstage.Models;

namespace Backstage.ViewModels
{
    public class PlaylistViewModel
    {
        public IEnumerable<PlayList> PlayLists { get; set; } = new List<PlayList>();
        public IEnumerable<PlayListCollaborator> PlayListCollaborators { get; set; } = new List<PlayListCollaborator>();
        public IEnumerable<PlayListItem> PlayListItems { get; set; } = new List<PlayListItem>();
        public IEnumerable<MemberPlayList> MemberPlayLists { get; set; } = new List<MemberPlayList>();

        // 分頁相關屬性
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1; 
        public int TotalRecords { get; set; } = 0; 

        // 排序相關屬性
        public string SortOrder { get; set; } = "asc";
        public string SortBy { get; set; } = "PlayListId";

        // 每頁顯示的筆數
        public int PageSize { get; set; } = 25;
    }
}

