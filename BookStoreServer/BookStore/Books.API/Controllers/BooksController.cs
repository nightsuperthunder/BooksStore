using Books.API.Entities;
using Books.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers; 


[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase {
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService) {
        _bookService = bookService;
    }

    [HttpGet] 
    public async Task<IActionResult> GetAllBooks() {
        var books = await _bookService.GetAllBooks();
        return Ok(books);
    }
    
    [HttpGet("{id:guid}")] 
    public async Task<IActionResult> GetBook(Guid id) {
        var books = await _bookService.GetBook(id);
        return Ok(books);
    }
    
    [HttpPost] 
    public async Task<IActionResult> CreateBook(Book book) {
        var books = await _bookService.CreateBook(book);
        return Ok(books);
    }
    
    [HttpPut("{id:guid}")] 
    public async Task<IActionResult> UpdateBook(Book book, Guid id) {
        var books = await _bookService.UpdateBook(book, id);
        return Ok(books);
    }
    
    [HttpDelete("{id:guid}")] 
    public async Task<IActionResult> DeleteBook(Guid id) {
        await _bookService.DeleteBook(id);
        return Ok();
    }
}