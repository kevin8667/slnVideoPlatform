namespace VdbAPI.Member.ViewModel
{
    public class JwtAuthObject
    {
        public string Email { get; set; }
        public long ExpiredTime { get; set; }
        public int MemberId { get; set; }
    }
}
