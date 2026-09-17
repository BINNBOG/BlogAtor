namespace BlogAtor.API.Controllers;

using BlogAtor.Collector.Abstractions;
using BlogAtor.Collector.Configuration;
using BlogAtor.Core.Config;
using BlogAtor.Core.Entity;
using BlogAtor.Store.Abstrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class SourceCollectorController : ControllerBase
{
    private readonly IDataSourceStore _dataSourceStore;
    private readonly ISourceCollectorStore _store;
    private readonly IConfigStore _configStore;
    private readonly IDataSourceCollectorManager _dataSourceCollectorManager;
    public SourceCollectorController(IDataSourceStore dataSourceStore, ISourceCollectorStore store, IConfigStore configStore, IDataSourceCollectorManager dataSourceCollectorManager)
    {
        _dataSourceStore = dataSourceStore;
        _store = store;
        _configStore = configStore;
        _dataSourceCollectorManager = dataSourceCollectorManager;
    }
    [HttpGet("types")]
    [ProducesResponseType<APIResponse<ICollection<System.String>>>(StatusCodes.Status200OK)]
    public ActionResult<APIResponse<ICollection<System.String>>> GetCollectorTypes()
    {
        return Ok(APIResponse.Success(_dataSourceCollectorManager.CollectorTypes));
    }
    [HttpGet("{dataSourceId}")]
    [ProducesResponseType<APIResponse<ICollection<SourceCollector>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<ICollection<SourceCollector>>>> GetAsync(System.Int64 dataSourceId)
    {
        var result = await _dataSourceStore.GetDataSourceCollectors(dataSourceId);
        return Ok(APIResponse.Success(result));
    }
    [HttpDelete]
    [ProducesResponseType<APIResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<APIResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> DeleteAsync([FromBody] SourceCollector sourceCollector)
    {
        var result = await _store.DeleteEntityAsync(sourceCollector);
        if (!result)
        {
            var errorInfo = new ErrorInfo("SourceCollectorDeleteErrorNotFound");
            return NotFound(APIResponse.Error(errorInfo));
        }
        return Ok(APIResponse.Success());
    }
    [HttpPost("{dataSourceId}/rss")]
    [ProducesResponseType<APIResponse<SourceCollector>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<SourceCollector>>> AddAsync([FromBody] RssCollectorConfiguration configuration, System.Int64 dataSourceId)
    {
        var sourceCollector = new SourceCollector()
        {
            CollectorType = RssCollectorConfiguration.cType,
            DataSourceId = dataSourceId
        };

        var config = new ConfigBase<RssCollectorConfiguration>()
        {
            Configuration = configuration
        };
        var configId = await _configStore.SaveConfiguration(config);
        sourceCollector.CollectorConfigId = configId;

        var validataionResult = await _dataSourceCollectorManager.ValidateCollectorConfiguration(sourceCollector.CollectorType, sourceCollector.CollectorConfigId);
        if (!validataionResult)
        {
            await _configStore.DeleteConfigById(configId);
            var errorInfo = new ErrorInfo("AddSourceCollectorErrorValidation");
            return BadRequest(APIResponse.Error(errorInfo));
        }
        
        var result = await _store.AddEntityAsync(sourceCollector);
        return Ok(APIResponse.Success(result));
    }
    [HttpPost("{dataSourceId}/search")]
    [ProducesResponseType<APIResponse<SearchResult<SourceCollectorInfoModel>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<SearchResult<SourceCollectorInfoModel>>>> SearchAsync([FromBody] SearchOptions options, System.Int64 dataSourceId)
    {
        var sourceCollectors = await _store.SelectAsync(options.Start, options.Length, sc => sc.DataSourceId == dataSourceId);
        var configIds = sourceCollectors.Select(sc => sc.CollectorConfigId).ToList();
        var configs = await _configStore.GetConfigsByIds(configIds);

        var result = sourceCollectors.Select(sc => new SourceCollectorInfoModel()
        {
            SourceCollector = sc,
            Configuration = configs.ContainsKey(sc.CollectorConfigId) ? configs[sc.CollectorConfigId] : null
        }).ToList();
        var count = result.Count();

        return Ok(APIResponse.Success(result.ToSearchResult(options.Draw, count, count)));
    }
}