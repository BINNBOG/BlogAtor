namespace BlogAtor;

using BlogAtor.API;
using BlogAtor.Collector.Rss;
using BlogAtor.Collector.Utils;
using BlogAtor.Core;
using BlogAtor.Framework;
using BlogAtor.Security.Authentication;
using BlogAtor.Store.Observer;
using BlogAtor.Store.Sql;
using BlogAtor.Publisher;

public class Program
{
    public static System.Int32 Main(System.String[] args) => MainAsync(args).GetAwaiter().GetResult();

    public static async Task<System.Int32> MainAsync(System.String[] args)
    {
        var runner = new Runner();
        runner.AddService<IStartupConfigProvider, StartupConfigProviderService>();

        runner.AddSqlStore();
        runner.AddStoreObserver();

        runner.AddPasswordAuth();
        
        runner.AddHttpClientProvider();
        runner.AddRss();

        runner.AddPublisher();

        runner.AddWebAPI();

        runner.Build();
        await runner.RunAsync();
        
        return 0;
    }
}