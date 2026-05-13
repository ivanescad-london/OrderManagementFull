// Models/OrderUpdateDto.cs

namespace OrderSystem.UI.Models
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public int ClientId { get; set; }
        public int SupplierId { get; set; }
        public int GoodsId { get; set; }

        public int Quantity { get; set; }
    }
}
