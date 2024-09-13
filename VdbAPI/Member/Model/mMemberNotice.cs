namespace VdbAPI.Member.Model
{
    public class mMemberNotice
    {
        public int? MemberNoticeID { get; set; }

        public int MemberID { get; set; }

        public string Title { get; set; }

        public string NoticeContent { get; set; }

        public string Status { get; set; }

        public int? RefNo { get; set; }

        public DateTime CreTime { get; set; }

        public string Action { get; set; }
    }
}
