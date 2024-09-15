namespace VdbAPI.DTO {
    public class ChatroomDTO {
        public required int SenderId {
            get; set;
        }
        public required string ChatMessage {
            get; set;
        }
        public required string Nickname { get; set; }
    }
}
