namespace BlogAtor.API;

public static class SearchResultExtensions
{
    public static SearchResult<T> ToSearchResult<T>(this ICollection<T> items, System.Int32 draw, System.Int32 total, System.Int32 filtered) where T : class
    {
        return new SearchResult<T>()
        {
            Draw = draw,
            Items = items,
            Total = total,
            Filtered = filtered
        };
    }
}