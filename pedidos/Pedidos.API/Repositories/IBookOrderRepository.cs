using Pedidos.API.Models;

namespace Pedidos.API.Repositories
{
    public interface IBookOrderRepository
    {
        Task<IEnumerable<BookOrder>> GetAllOrdersAsync();
        Task<BookOrder?> GetOrderByIdAsync(int id);
        Task<BookOrder> CreateOrderAsync(BookOrder order);
    }
}
