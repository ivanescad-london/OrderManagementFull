// Services/ClientService.cs

using OrderSystem.UI.Models;

namespace OrderSystem.UI.Services
{
    public class ClientService : GenericService<Client>
    {
        public ClientService(HttpClient http) : base(http, "clients") { }
    }
}
