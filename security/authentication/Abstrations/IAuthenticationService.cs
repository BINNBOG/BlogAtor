namespace BlogAtor.Security.Authentication;

using BlogAtor.Core.Entity;

public interface IAuthenticationService
{
    public Task<User> RegisterAsync(User user, System.String secret);
    public Task<User?> AuthenticateAsync(User user, System.String secret);
}