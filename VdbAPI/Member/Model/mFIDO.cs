namespace VdbAPI.Member.Model
{
    public class mFIDO
    {
        public int FIDOCredentialID { get; set; }

        public int MemberID { get; set; }

        public string CredentialID { get; set; }

        public byte[] PublicKey { get; set; }

        public byte[] UserHandle { get; set; }

        public long SignatureCounter { get; set; }

        public long AverageCounter { get; set; }

        public string CredType { get; set; }

        public DateTime RegDateTime { get; set; }

        public DateTime? LastUsedDateTime { get; set; }
    }
}
