using Books.BL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.BL.Services; 

public class BookService : IBookService {
    private readonly BooksContext _context;

    public BookService(BooksContext context) {
        _context = context;
    }

    public async Task<List<Book>> GetAllBooks() { 
        return await _context.Books.ToListAsync();
    }

    public async Task<Book> GetBook(Guid id) {
        return await _context.Books.FirstAsync(x => x.Id == id);
    }

    public async Task<Book> CreateBook(Book book) {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task DeleteBook(Guid id) {
        _context.Remove(new Book { Id = id });
        await _context.SaveChangesAsync();
    }

    public async Task<Book> UpdateBook(Book book, Guid id) {
        book.Id = id;
        _context.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }
}