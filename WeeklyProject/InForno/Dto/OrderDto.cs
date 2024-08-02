namespace InForno.Dto
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
        public bool IsProcessed { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }
}
