using Books.API.Entities;
using Books.API.Exceptions;
using Books.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Books.API.Controllers; 

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase {
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) {
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(User user) {
        try {
            var authResponse = await _authService.Login(user);
            
            Response.Cookies.Append("access-token", authResponse.AccessToken, 
                new CookieOptions{SameSite = SameSiteMode.None, Secure = true});
            
            return Ok(authResponse);
        }
        catch (NotFoundException) {
            return NotFound();
        }
        catch (ArgumentException) {
            return BadRequest();
        }
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(User user) {
        try {
            var authResponse = await _authService.Register(user);
            
            Response.Cookies.Append("access-token", authResponse.AccessToken, 
                new CookieOptions{SameSite = SameSiteMode.None, Secure = true});            
            return Ok(authResponse);
        }
        catch (ArgumentException) {
            return BadRequest();
        }
    }
    
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] JObject data) {
        string refreshToken;
        try {
            refreshToken = data["refreshToken"].Value<string>();
        }
        catch (Exception) {
            return BadRequest();
        }

        try {
            var authResponse = await _authService.Refresh(refreshToken);

            Response.Cookies.Append("access-token", authResponse.AccessToken,
                new CookieOptions { SameSite = SameSiteMode.None, Secure = true });
            return Ok(authResponse);
        }
        catch (NotFoundException) {
            return NotFound();
        }
        catch (ArgumentException) {
            return Unauthorized();
        }
    }
    
    [HttpPost]
    [Route("logout")]
    public Task<IActionResult> Logout() {
        Response.Cookies.Delete("access-token",
            new CookieOptions { SameSite = SameSiteMode.None, Secure = true });
        return Task.FromResult<IActionResult>(Ok());
    }
}