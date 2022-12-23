using Books.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.API.Services; 

public class BooksContext : DbContext {
    public BooksContext(DbContextOptions<BooksContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}