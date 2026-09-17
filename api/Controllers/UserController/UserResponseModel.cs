namespace BlogAtor.API.Controllers;

using BlogAtor.Core.Entity;

public class UserResponseModel
{
    public User User { get; set; }
    public ICollection<UserRole> Roles { get; set; }
}