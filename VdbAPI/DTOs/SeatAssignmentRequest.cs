namespace VdbAPI.DTOs
{
    public class SeatAssignmentRequest
    {
        public int ReservationId { get; set; }  // 訂單ID
        public int ShowtimeId { get; set; }     // 場次ID
        public int TicketCount { get; set; }    // 票數
    }
}
