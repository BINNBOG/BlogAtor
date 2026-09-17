namespace BlogAtor.Collector.Utils;

using BlogAtor.Collector.Abstractions;
using BlogAtor.Framework;

public static class UtilsRunnerExtensions
{
    public static void AddHttpClientProvider(this Runner runner)
    {
        runner.AddService<IHttpClientProvider, HttpClientProviderService>();
    }
}