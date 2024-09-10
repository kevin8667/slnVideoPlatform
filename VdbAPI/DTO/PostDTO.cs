namespace VdbAPI.DTO {
    public class PostDTO {
        public int PostId {
            get; set;
        }
        public int ArticleId {
            get; set;
        }
        public int PosterId {
            get; set;
        }
        public string? PostContent {
            get; set;
        } = "回覆無內文，請盡速處理。";
        public DateTime PostDate {
            get; set;
        }
        public bool Lock {
            get; set;
        }
        public string? NickName {
            get; set;
        } = "異常:無名字";
    }

}
