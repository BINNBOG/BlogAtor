namespace BlogAtor.Collector.Rss;

using System.Net;
using System.ServiceModel.Syndication;
using System.Threading.Tasks.Dataflow;
using System.Xml;

using BlogAtor.Collector.Abstractions;
using BlogAtor.Collector.Configuration;
using BlogAtor.Core.Config;
using BlogAtor.Core.Entity;
using BlogAtor.Framework;
using BlogAtor.Store.Abstrations;

using Microsoft.Extensions.Logging;

public class RssCollectorModule : ModuleBase, IDataSourceCollector
{
    private readonly System.String cUserAgentHeader = "User-Agent";
    private readonly System.String cUserAgentValue = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36 Edg/134.0.0.0";
    private readonly IHttpClientProvider _httpClientProvider;
    private readonly IConfigStore _configStore;
    private readonly ISourceCollectorStore _store;
    private readonly Lazy<IDataSourceCollectorManager> _collectionManager;
    private IDataSourceCollectorManager CollectionManager => _collectionManager.Value;
    public RssCollectorModule(IHttpClientProvider httpClientProvider, IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
        _httpClientProvider = httpClientProvider;
        _configStore = GetService<IConfigStore>();
        _store = GetService<ISourceCollectorStore>();
        _collectionManager = new Lazy<IDataSourceCollectorManager>(() => 
            GetService<IDataSourceCollectorManager>()); // TODO: move into seperate module
        AddPeriodicJob(CollectAsync, TimeSpan.FromMinutes(10));
    }
    private readonly System.String cCollectorType = RssCollectorConfiguration.cType;
    public string Type => cCollectorType;

    private async Task<Stream> DownloadRssFeed(System.String uri, System.Boolean useProxy)
    {
        HttpClient httpClient;
        if (useProxy)
        {
            var proxyIds = await _httpClientProvider.ListWebProxyIds();
            // TODO: iterate over proxies
            if (proxyIds.Count() == 0)
            {
                throw new NotImplementedException("missing http proxy configurations");
            }

            httpClient = (await _httpClientProvider.GetProxyClient(proxyIds.First()))!;
        }
        else
        {
            httpClient = new HttpClient();
        }

        httpClient.Timeout = TimeSpan.FromSeconds(5);
        httpClient.DefaultRequestHeaders.Add(cUserAgentHeader, cUserAgentValue);
        
        var resp = await httpClient.GetAsync(uri);
        return await resp.Content.ReadAsStreamAsync();
    }

    public async Task<System.Boolean> ValidateParameter(System.Int64 configId)
    {
        var config = await _configStore.GetConfiguration<RssCollectorConfiguration>(configId);
        if (config == null)
        {
            return false;
        }

        Uri? uri;
        if (!Uri.TryCreate(config.Configuration.Url, UriKind.Absolute, out uri) || uri == null)
        {
            return false;
        }

        try
        {
            var stream = await DownloadRssFeed(uri.AbsoluteUri, config.Configuration.UseProxy);
            var reader = XmlReader.Create(stream, new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Ignore
            });
            SyndicationFeed.Load(reader);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    private async Task CollectAsync()
    {
        Logger.LogInformation($"Collecting rss blogs...");

        var allCollectors = await _store.GetAllAsync();
        var rssCollectors = allCollectors.Where(c => c.CollectorType == Type).ToList();

        var collectionBlock = new ActionBlock<Tuple<SourceCollector, RssCollectorConfiguration>>(CollectFromRssSource, new ExecutionDataflowBlockOptions()
        {
            MaxDegreeOfParallelism = 16
        });
        var configIds = rssCollectors.Select(sc => sc.CollectorConfigId).ToList();
        var configs = await _configStore.GetConfigsByIds(configIds);
        foreach (var rssCollector in rssCollectors)
        {
            if (!configs.ContainsKey(rssCollector.CollectorConfigId))
            {
                Logger.LogWarning($"Missing rss collector config in database <configId={rssCollector.CollectorConfigId}>");
                continue;
            }
            var config = configs[rssCollector.CollectorConfigId] as ConfigBase<RssCollectorConfiguration>;
            if (config == null)
            {
                Logger.LogWarning($"Invalid config type in database. Expected RSS collector config <configId={rssCollector.CollectorConfigId}>");
                continue;
            }
            collectionBlock.Post(new Tuple<SourceCollector, RssCollectorConfiguration>(
                rssCollector,
                config.Configuration));
        }
        Logger.LogInformation($"Processing {rssCollectors.Count()} rss sources");
        collectionBlock.Complete();
        await collectionBlock.Completion;
        Logger.LogInformation($"Done collecting rss blogs...");
    }
    private async Task CollectFromRssSource(Tuple<SourceCollector, RssCollectorConfiguration> sourceCollectorInfo)
    {
        var (sourceCollector, config) = sourceCollectorInfo;
        if (config == null)
        {
            return;
        }
        var url = config.Url;
        try
        {
            var data = await DownloadRssFeed(url, config.UseProxy);
            var reader = XmlReader.Create(data);
            var feed = SyndicationFeed.Load(reader);

            foreach (var item in feed.Items)
            {
                var dataItem = FeedItemToDataItem(item, sourceCollector);
                Logger.LogInformation($"Got data item: Title={dataItem.Title}");
                await CollectionManager.SubmitDataItemAsync(dataItem);

            }
        }
        catch (Exception e)
        {
            Logger.LogError($"Error processing rss source <param={config.Url}>");
        }
    }
    private DataItem FeedItemToDataItem(SyndicationItem feedItem, SourceCollector collector)
    {
        var dataItem = new DataItem()
        {
            DataSourceId = collector.DataSourceId,
            CollectorId = collector.Id,

            Title = feedItem.Title.Text,
            ItemType = DataItemType.BlogPost,
            Uid = feedItem.Id,
            CollectedDate = DateTime.UtcNow,
            UpdatedDate = feedItem.PublishDate.Date.ToUniversalTime(),
            Link = feedItem.Links.SingleOrDefault()?.GetAbsoluteUri().AbsoluteUri ?? ""
        };
        return dataItem;
    }
}