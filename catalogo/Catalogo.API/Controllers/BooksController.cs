using Microsoft.AspNetCore.Mvc;

namespace Catalogo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private static readonly List<Book> Books = new()
    {
        new Book(1, "The Catcher in the Rye", "J.D. Salinger", 1951),
        new Book(2, "To Kill a Mockingbird", "Harper Lee", 1960),
        new Book(3, "1984", "George Orwell", 1949),
        new Book(4, "Pride and Prejudice", "Jane Austen", 1813),
        new Book(5, "The Great Gatsby", "F. Scott Fitzgerald", 1925)
    };

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        return Ok(Books);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetBookById(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpGet("search")]
    public IActionResult GetBooksByName([FromQuery] string name)
    {
        var matchingBooks = Books.Where(b => b.Title.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!matchingBooks.Any())
        {
            return NotFound();
        }
        return Ok(matchingBooks);
    }
}

public record Book(int Id, string Title, string Author, int YearPublished);
