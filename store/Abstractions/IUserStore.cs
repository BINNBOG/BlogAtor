namespace BlogAtor.Store.Abstrations;

using BlogAtor.Core.Entity;
using BlogAtor.Framework.Store;

public interface IUserStore : IEntityStore<User>
{
    public Task<ICollection<UserRole>> GetUserRolesAsync(User user);
    public Task<User> RegisterUserAsync(User user, System.String authSercret);
    public Task<ICollection<UserRole>> AddUserRolesAsync(User user, ICollection<UserRole> roles);
    public Task<ICollection<UserRole>> RemoveUserRolesAsync(User user, ICollection<UserRole> roles);
    public Task<System.String?> GetAuthSecretAsync(User user);
    public Task<User?> GetUserByUsernameAsync(System.String username);
}