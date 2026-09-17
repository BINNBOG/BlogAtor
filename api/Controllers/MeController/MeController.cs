namespace BlogAtor.API.Controllers;

using BlogAtor.Store.Abstrations;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MeController : APIControllerBase
{
    public MeController(IUserStore userStore) : base(userStore)
    {
    }

    [HttpGet]
    [ProducesResponseType<APIResponse<MeResponseModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<APIResponse<MeResponseModel>>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<APIResponse<MeResponseModel>>> GetMeAsync()
    {
        var user = await GetCurrentUser();
        if (user == null)
        {
            var errorInfo = new ErrorInfo("NotAuthenticated");
            return Unauthorized(APIResponse.Error(errorInfo));
        }
        var roles = await GetCurrentUserRoles();
        return Ok(APIResponse.Success(new MeResponseModel()
        {
            Me = user,
            Roles = roles
        }));
        
    }
}