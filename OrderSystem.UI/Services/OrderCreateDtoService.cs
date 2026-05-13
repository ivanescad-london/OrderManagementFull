// Services/OrderCreateDtoService.cs

using OrderSystem.UI.Models;

namespace OrderSystem.UI.Services
{
    public class OrderCreateDtoService : GenericService<OrderCreateDto>
    {
        public OrderCreateDtoService(HttpClient http) : base(http, "orders") { }
    }
}
