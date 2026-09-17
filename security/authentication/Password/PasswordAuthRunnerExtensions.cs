namespace BlogAtor.Security.Authentication;

using BlogAtor.Framework;

public static class PasswordAuthRunnerExtensions
{
    public static void AddPasswordAuth(this Runner runner)
    {
        runner.AddService<IAuthenticationService, PasswordAuthenticationService>();
    }
}