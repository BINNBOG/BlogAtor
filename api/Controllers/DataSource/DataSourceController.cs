namespace BlogAtor.API.Controllers;

using System.Text.Json;

using BlogAtor.Collector.Configuration;
using BlogAtor.Core.Config;
using BlogAtor.Core.Entity;
using BlogAtor.Store.Abstrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DataSourceController : APIControllerBase
{
    private readonly IDataSourceStore _store;
    private readonly ISourceCollectorStore _sourceCollectorStore;
    private readonly IConfigStore _configStore;
    public DataSourceController(IDataSourceStore dataSourceStore, ISourceCollectorStore sourceCollectorStore, IConfigStore configStore, IUserStore userStore)
        : base(userStore)
    {
        _store = dataSourceStore;
        _sourceCollectorStore = sourceCollectorStore;
        _configStore = configStore;
    }
    [HttpGet]
    [ProducesResponseType<APIResponse<ICollection<DataSource>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<ICollection<DataSource>>>> GetDataSourcesAsync()
    {
        var result = await _store.GetAllAsync();
        return Ok(APIResponse.Success(result));
    }
    [HttpGet("{id}")]
    [ProducesResponseType<APIResponse<DataSource>>(StatusCodes.Status200OK)]
    [ProducesResponseType<APIResponse<DataSource>>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse<DataSource>>> GetDataSourceAsync(System.Int64 id)
    {
        var result = await _store.GetByIdAsync(id);
        if (result == null)
        {
            var errorInfo = new ErrorInfo("DataSourceGetErrorNotFound");
            return NotFound(APIResponse.Error<DataSource>(errorInfo));
        }
        return Ok(APIResponse.Success(result));
    }
    [HttpPost]
    [ProducesResponseType<APIResponse<DataSource>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<DataSource>>> AddAsync([FromBody] DataSource dataSource)
    {
        var result = await _store.AddEntityAsync(dataSource);
        return Ok(APIResponse.Success(result));
    }
    [HttpDelete]
    [ProducesResponseType<APIResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<APIResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> DeleteAsync([FromBody] DataSource dataSource)
    {
        var result = await _store.DeleteEntityAsync(dataSource);
        if (!result)
        {
            var errorInfo = new ErrorInfo("DataSourceDeleteErrorNotFound");
            return NotFound(APIResponse.Error(errorInfo));
        }
        return Ok(APIResponse.Success());
    }
    [HttpPost("search")]
    [ProducesResponseType<APIResponse<SearchResult<DataSource>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<SearchResult<DataSource>>>> SearchAsync([FromBody] SearchOptions options)
    {
        var total = await _store.CountAsync();
        var filtered = await _store.CountAsync(ds => ds.Name.Contains(options.Search.Value));

        var order = options.IsInOrder("Name");
        var result = await _store.SelectAsync(options.Start, options.Length, ds => ds.Name.Contains(options.Search.Value),
            order == null
                ? null
                : ds => ds.Name,
            orderDescending: !order ?? false);
        return Ok(APIResponse.Success(result.ToSearchResult(options.Draw, total, filtered)));
    }
    [HttpGet("export")]
    [ProducesResponseType<APIResponse<DataSourceExportModel>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<DataSourceExportModel>>> ExportDataSources()
    {
        var dataSources = await _store.GetAllAsync();
        var sourceCollectors = (await _sourceCollectorStore.GetAllAsync()).GroupBy(sc => sc.DataSourceId).ToDictionary(g => g.Key, g => g.ToList());
        // TODO: batch config get by array of IDs
        var export = new DataSourceExportModel()
        {
            ExportedAt = DateTime.UtcNow,
            DataSources = new List<DataSourceExportItem>()
        };
        foreach (var ds in dataSources)
        {
            var dataSourceExportItem = new DataSourceExportItem()
            {
                DataSource = ds,
                Collectors = []
            };
            if (sourceCollectors.ContainsKey(ds.Id))
            {
                var exportItems = new List<SourceCollectorExportItem>();
                foreach (var sc in sourceCollectors[ds.Id])
                {
                    var config = await _configStore.GetConfigById(sc.CollectorConfigId);
                    exportItems.Add(new SourceCollectorExportItem()
                    {
                        SourceCollector = sc,
                        Configuration = config
                    });
                }
                dataSourceExportItem.Collectors = exportItems;
            }
            export.DataSources.Add(dataSourceExportItem);
        }
        return Ok(APIResponse.Success(export));
    }
    [HttpPost("import")]
    [ProducesResponseType<APIResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> ImportDataSourcesAsync([FromBody] DataSourceExportModel export)
    {
        foreach (var dsInfo in export.DataSources) {
            dsInfo.DataSource.Id = 0;
            var dsAdded = await _store.AddEntityAsync(dsInfo.DataSource);
            foreach (var scInfo in dsInfo.Collectors)
            {
                if (scInfo.SourceCollector.CollectorType == RssCollectorConfiguration.cType)
                {
                    // TODO: tech debt. refactor
                    var temp = JsonSerializer.Serialize(scInfo.Configuration);
                    var config = JsonSerializer.Deserialize<ConfigBase<RssCollectorConfiguration>>(temp, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (config == null)
                    {
                        throw new NotImplementedException();
                    }

                    var configId = await _configStore.SaveConfiguration(config!);
                    scInfo.SourceCollector.Id = 0;
                    scInfo.SourceCollector.DataSourceId = dsAdded.Id;
                    scInfo.SourceCollector.CollectorConfigId = configId;
                    await _sourceCollectorStore.AddEntityAsync(scInfo.SourceCollector);
                }
            }
        }
        return Ok(APIResponse.Success());
    }
}