using Books.API.Entities;

namespace Books.API.Services; 

public interface IAuthService {
    Task<AuthResponse> Login(User user);
    Task<AuthResponse> Register(User user);
    Task<AuthResponse> Refresh(string refreshToken);
}