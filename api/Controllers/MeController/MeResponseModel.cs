namespace BlogAtor.API.Controllers;

using BlogAtor.Core.Entity;

public class MeResponseModel
{
    public User Me { get; set; }
    public ICollection<UserRole> Roles { get; set; }
}