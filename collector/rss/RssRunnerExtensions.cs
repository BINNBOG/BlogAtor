namespace BlogAtor.Collector.Rss;

using BlogAtor.Collector.Abstractions;
using BlogAtor.Framework;

public static class RssRunnerExtensions
{
    public static void AddRss(this Runner runner)
    {
        runner.AddModule<IDataSourceCollectorManager, CollectorManagerService>();
        runner.AddModule<IDataSourceCollector, RssCollectorModule>();
    }
}