namespace OrderSystem.UI.Services
{
    public interface IGenericService<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task CreateAsync(T item);
        Task UpdateAsync(int id, T item);
        Task DeleteAsync(int id);
    }
}
