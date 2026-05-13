// Services/SupplierService.cs

using OrderSystem.UI.Models;

namespace OrderSystem.UI.Services
{
    public class SupplierService : GenericService<Supplier>
    {
        public SupplierService(HttpClient http) : base(http, "suppliers") { }
    }
}
