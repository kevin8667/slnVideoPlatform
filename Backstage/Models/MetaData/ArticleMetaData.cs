﻿using System.ComponentModel.DataAnnotations;

namespace Backstage.Models.MetaData {
    public class ArticleMetaData {

        [Display(Name ="上架")]
        public bool Lock {
            get; set;
        }
        [Display(Name = "回覆數")]

        public int? ReplyCount {
            get; set;
        }
        [Display(Name = "發表日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}",ApplyFormatInEditMode = true)]
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
        [Display(Name ="主題")]
        public virtual Theme? Theme {
            get; set;
        }

        public int ArticleId {
            get; set;
        }

        [Display(Name = "作者")]

        public virtual MemberInfo? Member {
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
