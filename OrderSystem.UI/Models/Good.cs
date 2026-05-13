// Models/Good.cs

namespace OrderSystem.UI.Models
{
    public class Good
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}