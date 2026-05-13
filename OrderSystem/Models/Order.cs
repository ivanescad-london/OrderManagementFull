using System.Text.Json.Serialization;

namespace OrderSystem.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        // ----- Client -----
        public int ClientId { get; set; }
        [JsonIgnore]
        public Client? Client { get; set; }

        // ----- Supplier -----
        public int SupplierId { get; set; }
        [JsonIgnore]
        public Supplier? Supplier { get; set; }

        // ----- Goods -----
        public int GoodsId { get; set; }
        [JsonIgnore]
        public Goods? Goods { get; set; }

        public int Quantity { get; set; }

    }
}
