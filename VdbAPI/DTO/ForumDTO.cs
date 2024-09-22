namespace VdbAPI.DTO {
    public class forumDto {
        public string? keyword {
            get; set;
        } = "";
        public int? categoryId { get; set; } = 0;
        public required int page {
            get; set;
        }
        public required int pageSize {
            get; set;
        }
    }
}
