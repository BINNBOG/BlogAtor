namespace BlogAtor.Store.Sql.Data;

using BlogAtor.Core.Entity;

internal class DbUserRole
{
    public System.Int64 Id { get; set ;}
    public DbUser User { get; set; }
    public System.Int64 UserId { get; set; }
    public UserRole Role { get; set; }
}