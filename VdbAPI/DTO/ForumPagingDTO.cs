using VdbAPI.Models;

namespace VdbAPI.DTO {
    public class ForumPagingDTO {

        public int TotalCount {
            get;
            set;
        }
        public int TotalPages {
            get;
            set;
        }
        public List<ArticleView>? ForumResult {
            get;
            set;
        }
    }
}
