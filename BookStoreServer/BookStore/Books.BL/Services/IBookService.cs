using Books.BL.Entities;

namespace Books.BL.Services;

public interface IBookService {
    Task<List<Book>> GetAllBooks();
    Task<Book> GetBook(Guid id);
    Task<Book> CreateBook(Book book);
    Task DeleteBook(Guid id);
    Task<Book> UpdateBook(Book book, Guid id);
}