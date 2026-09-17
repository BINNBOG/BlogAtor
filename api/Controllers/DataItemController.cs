namespace BlogAtor.API.Controllers;

using System.Linq.Expressions;
using BlogAtor.Core.Entity;
using BlogAtor.Store.Abstrations;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DataItemController : ControllerBase
{
    private readonly IDataItemStore _store;
    public DataItemController(IDataItemStore blogStore)
    {
        _store = blogStore;
    }

    [HttpGet]
    [ProducesResponseType<APIResponse<ICollection<DataItem>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<ICollection<DataItem>>>> GetDataItems()
    {
        var items = await _store.GetAllAsync();
        var ordered = items.OrderByDescending(di => di.UpdatedDate).ToList();
        return Ok(APIResponse.Success(ordered));
    }
    private Tuple<Expression<Func<DataItem, System.Object>>, System.Boolean>? GetOrderBySelector(SearchOptions options)
    {
        var orderBy = options.IsInOrder("Title");
        if (orderBy != null)
        {
            return new Tuple<Expression<Func<DataItem, System.Object>>, System.Boolean>(di => di.Title, orderBy.Value);
        }
        orderBy = options.IsInOrder("Updated");
        if (orderBy != null)
        {
            return new Tuple<Expression<Func<DataItem, System.Object>>, System.Boolean>(di => di.UpdatedDate, orderBy.Value);
        }
        orderBy = options.IsInOrder("Collected");
        if (orderBy != null)
        {
            return new Tuple<Expression<Func<DataItem, System.Object>>, System.Boolean>(di => di.CollectedDate, orderBy.Value);
        }
        return null;
    }
    [HttpPost("search")]
    [ProducesResponseType<APIResponse<SearchResult<DataItem>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<SearchResult<DataItem>>>> SearchAsync([FromBody] SearchOptions options)
    {
        var total = await _store.CountAsync();
        var filtered = await _store.CountAsync(di => di.Title.Contains(options.Search.Value));
        var orderByInfo = GetOrderBySelector(options);
        var items = await _store.SelectAsync(options.Start, options.Length, di => di.Title.Contains(options.Search.Value),
            orderByInfo?.Item1, orderByInfo?.Item2 ?? false);
        return Ok(APIResponse.Success(items.ToSearchResult(options.Draw, total, filtered)));
    }
}