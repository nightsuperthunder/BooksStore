using Books.API.Exceptions;
using Books.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers; 

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase {
    private readonly IUserService _userService;
    
    public UserController(IUserService userService) {
        _userService = userService;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetUser(Guid id) {
        try {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }
        catch (NotFoundException) {
            return NotFound();
        }
    }
}