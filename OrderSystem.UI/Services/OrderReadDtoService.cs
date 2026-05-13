// Services/OrderReadDtoService.cs

using OrderSystem.UI.Models;

namespace OrderSystem.UI.Services
{
    public class OrderReadDtoService : GenericService<OrderReadDto>
    {
        public OrderReadDtoService(HttpClient http) : base(http, "orders") { }
    }
}
