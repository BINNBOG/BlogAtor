namespace BlogAtor.Security.Authentication;

using System.Security.Cryptography;
using System.Text;

using BlogAtor.Core.Entity;
using BlogAtor.Framework;
using BlogAtor.Store.Abstrations;

internal class PasswordAuthenticationService : ServiceBase, IAuthenticationService
{
    private readonly IUserStore _store;
    public PasswordAuthenticationService(IUserStore store, IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
        _store = store;
    }

    public async Task<User?> AuthenticateAsync(User user, System.String secret)
    {
        var userSecret = await _store.GetAuthSecretAsync(user);
        var userEntity = await _store.GetUserByUsernameAsync(user.Username);
        if (userSecret == null || userEntity == null)
        {
            return null;
        }
        
        using (var md5 = MD5.Create())
        {
            var hashedSecret = md5.ComputeHash(Encoding.ASCII.GetBytes(secret));
            var b64secret = Convert.ToBase64String(hashedSecret);
            if (userSecret == b64secret)
            {
                return userEntity;
            }
            return null;
        }
    }

    public async Task<User> RegisterAsync(User user, System.String secret)
    {
        using (var md5 = MD5.Create())
        {
            var hashedSecret = md5.ComputeHash(Encoding.ASCII.GetBytes(secret));
            var userSecret = Convert.ToBase64String(hashedSecret);
            var registeredUser = await _store.RegisterUserAsync(user, userSecret);
            return registeredUser;
        }
    }
}