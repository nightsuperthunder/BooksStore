using Books.BL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.BL.Services; 

public class UserService : IUserService {
    private readonly BooksContext _context;

    public UserService(BooksContext context) {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers() {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUser(Guid id) {
        return await _context.Users.FirstAsync(x => x.Id == id);
    }

    public async Task<User> CreateUser(User user) { 
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteUser(Guid id) {
        _context.Users.Remove(new User { Id = id });
        await _context.SaveChangesAsync();
    }

    public async Task<User> UpdateUser(User user, Guid id) {
        user.Id = id;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}