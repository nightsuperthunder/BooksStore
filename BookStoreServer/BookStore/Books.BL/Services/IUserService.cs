﻿using Books.BL.Entities;

namespace Books.BL.Services;

public interface IUserService {
    Task<List<User>> GetAllUsers();
    Task<User> GetUser(Guid id);
    Task<User> CreateUser(User user);
    Task DeleteUser(Guid id);
    Task<User> UpdateUser(User user, Guid id);
}