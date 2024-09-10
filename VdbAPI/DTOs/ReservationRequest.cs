namespace VdbAPI.DTOs
{
    public class ReservationRequest
    {
        public int MemberID { get; set; }
        public int ShowtimeID { get; set; }
        public decimal TotalPrice { get; set; }
        public int TicketCount { get; set; }  // 增加票數欄位
        public string? PaymentMethod { get; set; }
        public int? CouponID { get; set; }
    }
}
