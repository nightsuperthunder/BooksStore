using Books.API.Entities;
using Books.API.Exceptions;
using Books.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            
            Response.Cookies.Append("AccessToken", authResponse.AccessToken);
            
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
            
            Response.Cookies.Append("AccessToken", authResponse.AccessToken);
            
            return Ok(authResponse);
        }
        catch (ArgumentException) {
            return BadRequest();
        }
    }
    
    [HttpPost]
    [Route("logout")]
    public Task<IActionResult> Logout() {
        Response.Cookies.Delete("AccessToken");
        return Task.FromResult<IActionResult>(Ok());
    }
}