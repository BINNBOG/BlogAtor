namespace BlogAtor.Store.Sql;

using BlogAtor.Framework;
using BlogAtor.Store.Abstrations;

using Microsoft.Extensions.DependencyInjection;

public static class SqlStoreRunnerExtensions
{
    public static void AddSqlStore(this Runner runner)
    {
        runner.Services.AddDbContext<BlogContext>(ServiceLifetime.Transient);     

        runner.AddTransient<IConfigStore, ConfigStore>();

        runner.AddTransient<IUserStore, UserStore>();

        runner.AddTransient<IDataSourceStore, DataSourceStore>();
        runner.AddTransient<ISourceCollectorStore, SourceCollectorStore>();
        runner.AddTransient<IDataItemStore, DataItemStore>();
        runner.AddTransient<IDataContentStore, DataContentStore>();
    }
}