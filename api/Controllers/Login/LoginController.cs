namespace BlogAtor.API.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogAtor.Core.Entity;
using BlogAtor.Security.Authentication;
using BlogAtor.Store.Abstrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IUserStore _store;
    public LoginController(IAuthenticationService authService, IUserStore store)
    {
        _authService = authService;
        _store = store;
    }
    [HttpPost]
    [ProducesResponseType<APIResponse<System.String>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<System.String>>> LoginAsync([FromBody] LoginModel request)
    {
        var user = new User()
        {
            Username = request.Username
        };
        var userEntity = await _authService.AuthenticateAsync(user, request.Password);
        if (userEntity == null)
        {
            var errorInfo = new ErrorInfo("AuthenticationError");
            return Unauthorized(APIResponse.Error(errorInfo));
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.NameIdentifier, userEntity.Id.ToString())
        };
        var roles = await _store.GetUserRolesAsync(userEntity);
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.ToString())));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebAPIModule.cJwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(7);

        var token = new JwtSecurityToken(
            WebAPIModule.cIssuer,
            WebAPIModule.cIssuer,
            claims,
            expires: expiry,
            signingCredentials: creds
        );
        return Ok(APIResponse.Success(new JwtSecurityTokenHandler().WriteToken(token)));
    }
}