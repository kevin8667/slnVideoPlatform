namespace VdbAPI.DTO {
    public class AllReactionsDTO {
        public LikeDTO? ArticleReaction {
            get; set;
        }
        public IEnumerable<LikeDTO>? PostReactions {
            get; set;
        }
    }
}
