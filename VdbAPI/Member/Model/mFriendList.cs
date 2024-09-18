namespace VdbAPI.Member.Model
{
    public class mFriendList
    {
        public int FriendListID { get; set; }

        public int MemberID { get; set; }

        public int FriendID { get; set; }

        public DateTime CreationDate { get; set; }

        public string FriendStatus { get; set; }

        public string InvitedMessage { get; set; }

    }
}
