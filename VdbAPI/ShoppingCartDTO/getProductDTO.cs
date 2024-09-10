namespace VdbAPI.ShoppingCartDTO
{
    public class getProductDTO
    {
        public int ProductId {  get; set; }
        public string? ProductName { get; set; }
        public Decimal? ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImg { get; set; }
    }
}
