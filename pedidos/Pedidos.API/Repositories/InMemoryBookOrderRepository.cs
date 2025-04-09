using Pedidos.API.Models;

namespace Pedidos.API.Repositories
{
    public class InMemoryBookOrderRepository : IBookOrderRepository
    {
        private readonly List<BookOrder> _orders = new();
        private int _nextId = 1;

        public InMemoryBookOrderRepository()
        {
            // Add some sample data
            _orders.Add(new BookOrder
            {
                Id = _nextId++,
                CustomerName = "João Silva",
                CustomerEmail = "joao@example.com",
                OrderDate = DateTime.Now.AddDays(-5),
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 1, BookId = 101, BookTitle = "O Senhor dos Anéis", Quantity = 1, Price = 45.90m },
                    new OrderItem { Id = 2, BookId = 102, BookTitle = "Harry Potter", Quantity = 2, Price = 35.50m }
                }
            });
        }

        public Task<BookOrder> CreateOrderAsync(BookOrder order)
        {
            order.Id = _nextId++;
            order.OrderDate = DateTime.Now;
            
            // Assign IDs to items
            int itemId = 1;
            foreach (var item in order.Items)
            {
                item.Id = itemId++;
            }

            _orders.Add(order);
            return Task.FromResult(order);
        }

        public Task<IEnumerable<BookOrder>> GetAllOrdersAsync()
        {
            return Task.FromResult<IEnumerable<BookOrder>>(_orders);
        }

        public Task<BookOrder?> GetOrderByIdAsync(int id)
        {
            return Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
        }
    }
}
