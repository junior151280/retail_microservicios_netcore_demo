using System.Text;
using System.Text.Json;
using Website.Models;

namespace Website.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://localhost:5272/api/BookOrders";
        private readonly JsonSerializerOptions _jsonOptions;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<BookOrder>> GetAllOrdersAsync()
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<BookOrder>>(content, _jsonOptions) ?? new List<BookOrder>();
        }

        public async Task<BookOrder?> GetOrderByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
                
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BookOrder>(content, _jsonOptions);
        }

        public async Task<BookOrder> CreateOrderAsync(BookOrder order)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(order, _jsonOptions),
                Encoding.UTF8,
                "application/json");
                
            var response = await _httpClient.PostAsync(_apiBaseUrl, content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BookOrder>(responseContent, _jsonOptions) 
                ?? throw new Exception("Failed to deserialize created order");
        }
    }
}
