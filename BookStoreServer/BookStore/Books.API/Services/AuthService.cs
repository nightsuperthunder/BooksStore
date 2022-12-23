using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Books.API.Entities;
using Books.API.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Books.API.Services; 

public class AuthService : IAuthService {
    private readonly BooksContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthService(BooksContext context, JwtSettings jwtSettings) {
        _context = context;
        _jwtSettings = jwtSettings;
    }

    public async Task<AuthResponse> Login(User user) {
        var userToLogin = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
        if (userToLogin == null) {
            throw new NotFoundException($"User with {user.Email} email not found");
        }

        if (userToLogin.Password != GenerateHash(user.Password, userToLogin.PasswordSalt)) {
            throw new ArgumentException("Incorrect password");
        }

        return GenerateToken(userToLogin);
    }

    public async Task<AuthResponse> Register(User user) {
        var userToLogin = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
        if (userToLogin != null) {
            throw new ArgumentException("User already exist");
        }

        user.PasswordSalt = GenerateSalt();
        user.Password = GenerateHash(user.Password, user.PasswordSalt);

        _context.Add(user);
        await _context.SaveChangesAsync();

        return GenerateToken(user);
    }

    private AuthResponse GenerateToken(User user) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, user.Email),
            }),

            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var authResponse = new AuthResponse {
            AccessToken = tokenHandler.WriteToken(token)
        };

        return authResponse;
    }
    
    private byte[] GenerateSalt() {
        var rng = RandomNumberGenerator.Create();
        var buff = new byte[16];
        rng.GetBytes(buff);
        return buff;
    }
    
    private string GenerateHash(string input, byte[] salt) {
        var bytes = Encoding.UTF8.GetBytes(input + salt);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}