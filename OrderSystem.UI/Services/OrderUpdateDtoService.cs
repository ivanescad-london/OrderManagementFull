// Services/OrderUpdateDtoService.cs

using OrderSystem.UI.Models;

namespace OrderSystem.UI.Services
{
    public class OrderUpdateDtoService : GenericService<OrderUpdateDto>
    {
        public OrderUpdateDtoService(HttpClient http) : base(http, "orders") { }
    }
}
