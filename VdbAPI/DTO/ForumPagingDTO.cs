using VdbAPI.Models;

namespace VdbAPI.DTO {
    public class ForumPagingDTO {

        public int TotalCount {
            get;
            set;
        }
        public IEnumerable<ArticleView>? ForumResult {
            get;
            set;
        }
    }
}
