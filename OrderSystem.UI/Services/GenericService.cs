namespace OrderSystem.UI.Services
{
    public class GenericService<T> : IGenericService<T>
    {
        private readonly HttpClient _http;
        private readonly string _endpoint;

        public GenericService(HttpClient http, string endpoint)
        {
            _http = http;
            _endpoint = endpoint;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<T>>($"api/{_endpoint}/GetAll")
                ?? new List<T>();
        }
        public async Task CreateAsync(T item) =>
            await _http.PostAsJsonAsync($"api/{_endpoint}/Create", item);

        public Task<T?> GetByIdAsync(int id) =>
            _http.GetFromJsonAsync<T>($"api/{_endpoint}/Read/{id}");

        public async Task UpdateAsync(int id, T item) =>
            await _http.PutAsJsonAsync($"api/{_endpoint}/Update/{id}", item);

        public async Task DeleteAsync(int id) =>
            await _http.DeleteAsync($"api/{_endpoint}/Delete/{id}");
    }

}
