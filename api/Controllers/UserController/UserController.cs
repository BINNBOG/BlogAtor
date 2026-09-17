namespace BlogAtor.API.Controllers;

using System.Linq.Expressions;
using BlogAtor.Core.Entity;
using BlogAtor.Store.Abstrations;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : APIControllerBase
{
    private IUserStore _userStore;
    public UserController(IUserStore userStore) : base(userStore)
    {
        _userStore = userStore;
    }
    [HttpGet]
    [ProducesResponseType<APIResponse<ICollection<UserResponseModel>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<ICollection<UserResponseModel>>>> GetUsersAsync()
    {
        var users = await _userStore.GetAllAsync();
        var results = new List<UserResponseModel>();
        foreach (var user in users)
        {
            var roles = await _userStore.GetUserRolesAsync(user);
            results.Add(new UserResponseModel()
            {
                User = user,
                Roles = roles
            });
        }
        return Ok(APIResponse.Success(results));
    }
    [HttpPost("{userId}/roles")]
    [ProducesResponseType<APIResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> UpdateUserRolesAsync([FromBody] ICollection<UserRole> roles, System.Int64 userId)
    {
        var user = await _userStore.GetByIdAsync(userId);
        if (user == null)
        {
            var errorInfo = new ErrorInfo("UserNotFoundError");
            return NotFound(APIResponse.Error(errorInfo));
        }

        var currentRoles = await _userStore.GetUserRolesAsync(user);
        var toAdd = roles.Except(currentRoles).ToList();
        var toRemove = currentRoles.Except(roles).ToList();

        if (toAdd.Count() > 0)
        {
            await _userStore.AddUserRolesAsync(user, toAdd);
        }
        if (toRemove.Count() > 0)
        {
            await _userStore.RemoveUserRolesAsync(user, toRemove);
        }
        return Ok(APIResponse.Success());
    }
    [HttpGet("roles")]
    [ProducesResponseType<APIResponse<ICollection<UserRole>>>(StatusCodes.Status200OK)]
    public ActionResult<APIResponse<ICollection<UserRole>>> GetAllRoles()
    {
        return Ok(APIResponse.Success(Enum.GetValues<UserRole>()));
    }
    [HttpDelete]
    [ProducesResponseType<APIResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<APIResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<APIResponse>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> DeleteUserAsync([FromBody] User user)
    {
        var dbUser = await _userStore.GetByIdAsync(user.Id);
        if (dbUser == null)
        {
            var error = new ErrorInfo("UserNotFoundError");
            return NotFound(APIResponse.Error(error));
        }
        if (await _userStore.DeleteEntityAsync(dbUser))
        {
            return Ok(APIResponse.Success());
        }
        else
        {
            var error = new ErrorInfo("UserDeleteError");
            return BadRequest(APIResponse.Error(error));
        }
    }
    [HttpPost("search")]
    [ProducesResponseType<APIResponse<SearchResult<UserResponseModel>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<SearchResult<UserResponseModel>>>> SearchUsersAsync([FromBody] SearchOptions options)
    {
        var total = await _userStore.CountAsync();

        Expression<Func<User, System.Object>>? sortExpression = null;
        var sortColumn = options.GetSortColumnNAme();

        if (sortColumn == "Username")
        {
            sortExpression = (u) => u.Username;
        }
        else if (sortColumn == "Id")
        {
            sortExpression = u => u.Id;
        }

        var users = await _userStore.SelectAsync(options.Start, options.Length,
            u => u.Username.Contains(options.Search.Value),
            sortExpression, !options.IsAsending());

        var results = new List<UserResponseModel>();
        foreach (var user in users) 
        {
            var roles = await _userStore.GetUserRolesAsync(user);
            results.Add(new UserResponseModel()
            {
                User = user,
                Roles = roles
            });
        }

        return Ok(APIResponse.Success(results.ToSearchResult(options.Draw, total, total)));
    }
}