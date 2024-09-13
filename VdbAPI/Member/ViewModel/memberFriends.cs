namespace VdbAPI.Member.ViewModel
{
    public class memberFriends
    {
        public int FriendListID { get; set; }

        public int MemberID { get; set; }

        public int FriendID { get; set; }

        public DateTime CreationDate { get; set; }

        public string FriendStatus { get; set; }

        public string InvitedMessage { get; set; }

        public string NickName { get; set; }

        public string MemberName { get; set; }

        public string? PhotoPath { get; set; }
    }
}
