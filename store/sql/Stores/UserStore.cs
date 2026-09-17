namespace BlogAtor.Store.Sql;

using BlogAtor.Core.Entity;
using BlogAtor.Store.Abstrations;
using BlogAtor.Store.Sql.Data;

using Microsoft.EntityFrameworkCore;

internal class UserStore : EntityStoreServiceBase<User, DbUser>, IUserStore
{
    public UserStore(BlogContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }

    public async Task<System.String?> GetAuthSecretAsync(User user)
    {
        var dbUser = await DbSet.SingleOrDefaultAsync(u => u.Username == user.Username);
        return dbUser?.AuthSecret;
    }

    public async Task<ICollection<UserRole>> GetUserRolesAsync(User user)
    {
        return await Context.UserRoles.Where(u => u.UserId == user.Id).Select(ur => ur.Role).ToListAsync();
    }

    public async Task<User> RegisterUserAsync(User user, System.String authSercret)
    {
        var dbUser = new DbUser(user);
        dbUser.AuthSecret = authSercret;
        var added = await DbSet.AddAsync(dbUser);
        await Context.SaveChangesAsync();
        return added.Entity.ToEntity();
    }

    public async Task<ICollection<UserRole>> AddUserRolesAsync(User user, ICollection<UserRole> roles)
    {
        var existingRoles = await GetUserRolesAsync(user);
        var toAdd = roles.Except(existingRoles).ToList();
        if (toAdd.Count() == 0)
        {
            return [];
        }

        await Context.UserRoles.AddRangeAsync(toAdd.Select(role => new DbUserRole()
        {
            UserId = user.Id,
            Role = role
        }));
        await Context.SaveChangesAsync();
        return toAdd;
    }

    public async Task<ICollection<UserRole>> RemoveUserRolesAsync(User user, ICollection<UserRole> roles)
    {
        var toRemove = await Context.UserRoles.Where(ur => ur.UserId == user.Id 
                                                        && roles.Contains(ur.Role)).ToListAsync();

        Context.UserRoles.RemoveRange(toRemove);
        await Context.SaveChangesAsync();

        return toRemove.Select(ur => ur.Role).ToList();
    }

    public async Task<User?> GetUserByUsernameAsync(System.String username)
    {
        return await DbSet.SingleOrDefaultAsync(u => u.Username == username);
    }
}