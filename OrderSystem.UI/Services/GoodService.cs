// Services/GoodsService.cs

using OrderSystem.UI.Models;

namespace OrderSystem.UI.Services
{
    public class GoodService : GenericService<Good>
    {
        public GoodService(HttpClient http) : base(http, "goods") { }
    }
}
