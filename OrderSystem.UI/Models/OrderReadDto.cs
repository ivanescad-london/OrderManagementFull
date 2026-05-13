// Models/OrderReadDto.cs

namespace OrderSystem.UI.Models
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public string Client { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public string Goods { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}
