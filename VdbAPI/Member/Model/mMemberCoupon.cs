namespace VdbAPI.Member.Model
{
    public class mMemberCoupon
    {
        public int SerialNo { get; set; }

        public int MemberID { get; set; }

        public int CouponID { get; set; }

        public string Status { get; set; }

        public DateTime? UseTime { get; set; }

        public DateTime GetTime { get; set; }

        public string ActionType { get; set; }

        public int? ActionRefNo { get; set; }

        public int? GiftID { get; set; }
    }
}
