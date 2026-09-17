namespace BlogAtor.Store.Sql.Data;

using BlogAtor.Core.Entity;

internal class DbUser : User, IDbEntity<User>
{
    public DbUser()
    {}
    public System.String AuthSecret { get; set; }
    public ICollection<DbUserRole> Roles { get; set; }
    public DbUser(User entity)
    {
        Id = entity.Id;
        Username = entity.Username;
    }
    public User ToEntity() => new User()
    {
        Id = Id,
        Username = Username
    };
}