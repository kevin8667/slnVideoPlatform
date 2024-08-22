using System.ComponentModel.DataAnnotations;

namespace Backstage.Models.MetaData {
    public class ArticleViewMetaData {
        [Display(Name = "上架")]
        public bool Lock {
            get; set;
        }
        [Display(Name = "回覆數")]

        public int? ReplyCount {
            get; set;
        }
        [Display(Name = "發表日期")]
        public DateTime? PostDate {
            get; set;
        }
        [Display(Name = "標題")]

        public string? Title {
            get; set;
        }
        [Display(Name = "內容")]

        public string? ArticleContent {
            get; set;
        }
        [Display(Name = "主題")]
        public string? ThemeName {
            get; set;
        }

        public int ArticleId {
            get; set;
        }

        [Display(Name = "作者")]

        public string? MemberName {
            get; set;
        }
        [Display(Name = "作者")]

        public int AuthorId {
            get; set;
        }
        [Display(Name = "主題")]
        public int ThemeId {
            get; set;
        }
    }
}