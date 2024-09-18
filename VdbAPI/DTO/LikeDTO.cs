namespace VdbAPI.DTO {
    public class LikeDTO {
        public int MemberId {
            get; set;
        }
        public int ContentId {
            get; set;
        }
        public bool? ReactionType {
            get; set;
        }
    }
}
