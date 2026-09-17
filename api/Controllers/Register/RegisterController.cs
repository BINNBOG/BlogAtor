namespace BlogAtor.API.Controllers;

using BlogAtor.Core.Entity;
using BlogAtor.Security.Authentication;
using BlogAtor.Store.Abstrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IUserStore _store;
    public RegisterController(IAuthenticationService authService, IUserStore store)
    {
        _authService = authService;
        _store = store;
    }
    [HttpPost]
    [ProducesResponseType<APIResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> RegisterAsync([FromBody] RegisterModel request)
    {
        var user = new User()
        {
            Username = request.Username
        };

        var firstUser = (await _store.CountAsync()) == 0;
        var registered = await _authService.RegisterAsync(user, request.Password);
        if (firstUser)
        {
            await _store.AddUserRolesAsync(registered, [UserRole.Admin]);
        }
        return Ok(APIResponse.Success());
    }
}