namespace BlogAtor.API.Controllers;

using System.Security.Claims;
using BlogAtor.Core.Entity;
using BlogAtor.Store.Abstrations;
using Microsoft.AspNetCore.Mvc;

public class APIControllerBase : ControllerBase
{
    private readonly IUserStore _userStore;
    public APIControllerBase(IUserStore userStore)
    {
        _userStore = userStore;
    }

    protected async Task<System.Int64?> GetCurrentUserId()
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return null;
        }

        var idClaim = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (idClaim == null)
        {
            return null;
        }
        System.Int64 userId;
        if (!System.Int64.TryParse(idClaim, out userId))
        {
            return null;
        }
        return userId;
    }
    protected async Task<User?> GetCurrentUser()
    {
        var id = await GetCurrentUserId();
        if (id == null)
        {
            return null;
        }
        var user = await _userStore.GetByIdAsync(id.Value);
        if (user == null)
        {
            return null;
        }
        return user;
    }

    protected async Task<ICollection<UserRole>> GetCurrentUserRoles()
    {
        var user = await GetCurrentUser();
        if (user == null)
        {
            return [];
        }
        var roles = await _userStore.GetUserRolesAsync(user);
        return roles;
    }
}