using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

public class SearchModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SearchModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public string Query { get; set; }

    public List<Book> Books { get; set; } = new();

    public async Task OnPostAsync()
    {
        if (string.IsNullOrEmpty(Query)) return;

        var client = _httpClientFactory.CreateClient("CatalogApi");
        HttpResponseMessage response;

        // Verifica se a consulta é um número (ID) ou texto (nome)
        if (int.TryParse(Query, out int id))
        {
            // Busca por ID
            response = await client.GetAsync($"books/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var book = JsonSerializer.Deserialize<Book>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Books = new List<Book> { book };
            }
        }
        else
        {
            // Busca por nome
            response = await client.GetAsync($"books/search?name={Uri.EscapeDataString(Query)}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Books = JsonSerializer.Deserialize<List<Book>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
    }
}
