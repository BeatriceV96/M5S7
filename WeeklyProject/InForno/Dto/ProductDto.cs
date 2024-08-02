namespace InForno.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DeliveryTime { get; set; }
        public IFormFile Photo { get; set; }
        public string Ingredients { get; set; }
    }
}
