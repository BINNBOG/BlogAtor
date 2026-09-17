namespace BlogAtor.API;

public class SearchOptions
{
    public System.Int32 Draw { get; set; }
    public System.Int32 Start { get; set; }
    public System.Int32 Length { get; set; }

    public ICollection<DatatablesColumnInfo> Columns { get; set; }
    public ICollection<DatatablesOrderInfo> Order { get; set; }
    public DatatablesSearch Search { get; set; }

    public System.String? GetSortColumnNAme() => Order.FirstOrDefault()?.Name;
    public System.Boolean IsAsending() => Order.FirstOrDefault()?.Ascending ?? true;
    public System.Boolean? IsInOrder(System.String name)
    {
        var result = Order.SingleOrDefault(o => o.Name == name);
        if (result == null)
        {
            return null;
        }
        return result.Ascending;
    }
}