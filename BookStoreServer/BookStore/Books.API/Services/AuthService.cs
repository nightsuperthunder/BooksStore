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
    private readonly IUserService _userService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(BooksContext context, JwtSettings jwtSettings, IUserService userService) {
        _context = context;
        _jwtSettings = jwtSettings;
        _userService = userService;
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

        await _userService.CreateUser(user);
        
        return GenerateToken(user);
    }

    public async Task<AuthResponse> Refresh(string refreshToken) {
        if (ValidateRefreshToken(refreshToken, out var userId)) {
            var user = await _userService.GetUser(userId);

            return GenerateToken(user);
        }

        throw new ArgumentException();
    }

    private bool ValidateRefreshToken(string refreshToken, out Guid id) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshKey));
        ClaimsPrincipal principal;
        try {
            principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters {
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            }, out _);
        }
        catch {
            id = Guid.Empty;
            return false;
        }

        id = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return true;
    }

    private AuthResponse GenerateToken(User user) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var accessKey = Encoding.ASCII.GetBytes(_jwtSettings.AccessKey);
        var refreshKey = Encoding.ASCII.GetBytes(_jwtSettings.RefreshKey);


        var accessTokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(accessKey), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var refreshTokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(refreshKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
        var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);
        var authResponse = new AuthResponse {
            AccessToken = tokenHandler.WriteToken(accessToken),
            RefreshToken = tokenHandler.WriteToken(refreshToken)
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