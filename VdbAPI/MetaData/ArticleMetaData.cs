using System.ComponentModel.DataAnnotations;

namespace VdbAPI.MetaData {
    public class ArticleMetaData {

        [Required(ErrorMessage = "主題ID是必填項")]
        public int ThemeId {
            get; set;
        }

        [Required(ErrorMessage = "作者ID是必填項")]
        public int AuthorId {
            get; set;
        }

        [Required(ErrorMessage = "標題是必填項")]
        public required string Title {
            get; set;
        }

        [Required(ErrorMessage = "文章內容是必填項")]
        public required string ArticleContent {
            get; set;
        }

        [Required(ErrorMessage = "發表時間為必填")]
        public required DateTime PostDate {
            get; set;
        }

        [Required(ErrorMessage = "更新時間為必填")]
        public required DateTime UpdateDate {
            get; set;
        }

        [Required(ErrorMessage = "回覆數為必填")]
        public int? ReplyCount {
            get; set;
        }

        [Required(ErrorMessage = "上架狀態為必填")]
        public required bool Lock {
            get; set;
        }

    }
}
