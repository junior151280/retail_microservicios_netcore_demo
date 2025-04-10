using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Services
{
    public interface IOrderService
    {
        Task<List<BookOrder>> GetOrdersAsync();
        Task<BookOrder> GetOrderAsync(int id);
        Task<BookOrder> CreateOrderAsync(BookOrder order);
    }

    public class OrderService : IOrderService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public OrderService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<BookOrder>> GetOrdersAsync()
        {
            var client = _httpClientFactory.CreateClient("PedidosApi");
            var response = await client.GetAsync("bookorders");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<BookOrder>>(json, _jsonOptions) ?? new List<BookOrder>();
            }
            
            return new List<BookOrder>();
        }

        public async Task<BookOrder> GetOrderAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("PedidosApi");
            var response = await client.GetAsync($"bookorders/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BookOrder>(json, _jsonOptions);
            }
            
            return null;
        }

        public async Task<BookOrder> CreateOrderAsync(BookOrder order)
        {
            var client = _httpClientFactory.CreateClient("PedidosApi");
            var response = await client.PostAsJsonAsync("bookorders", order);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BookOrder>(json, _jsonOptions);
            }
            
            return null;
        }
    }
}
