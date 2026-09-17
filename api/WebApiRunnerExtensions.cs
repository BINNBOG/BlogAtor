namespace BlogAtor.API;

using BlogAtor.Framework;

public static class WebApiRunnerExtensions
{
    public static void AddWebAPI(this Runner runner)
    {
        runner.AddModule<WebAPIModule>();
    }
}