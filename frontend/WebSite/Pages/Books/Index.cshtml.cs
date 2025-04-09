using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSite.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<Book> Books { get; set; } = new List<Book>();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("CatalogApi");
            var response = await client.GetAsync("books");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Books = JsonSerializer.Deserialize<List<Book>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        public class Book
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Author { get; set; }
        }
    }
}
