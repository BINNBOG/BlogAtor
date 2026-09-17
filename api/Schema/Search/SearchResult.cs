namespace BlogAtor.API;

public class SearchResult<T> where T : class
{
    public System.Int32 Draw { get; set; }
    public System.Int32 Total { get; set; }
    public System.Int32 Filtered { get; set; }
    public ICollection<T> Items { get; set; }
}