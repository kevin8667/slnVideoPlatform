namespace VdbAPI.DTO {
    public class AllReactionsDTO {
        public LikeDTO? ArticleReaction {
            get; set;
        }
        public List<LikeDTO>? PostReactions {
            get; set;
        }
    }
}
