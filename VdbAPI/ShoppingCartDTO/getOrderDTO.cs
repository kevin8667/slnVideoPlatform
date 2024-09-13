namespace VdbAPI.ShoppingCartDTO
{
    public class getOrderDTO
    {
        public int OrderId { get; set; }
        public int? ShoppingCartId { get; set; }

        public int? MemberId { get; set; }
        public int? PlanId { get; set; }
        public string PlanName { get; set; }
        public int? VideoId { get; set; }
        public string VideoName { get; set; }
        public int? CouponId { get; set; }
        public string CouponName { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? OrderTotalPrice { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryAddress { get; set; }
        public int? PaymentStatus { get; set; }
        //public int? DriverId { get; set; }
        //public string? DriverName { get; set; }
        public int? DeliveryStatus { get; set; }
    }
}
