namespace BlogAtor.API;

public class DatatablesColumnInfo
{
    public System.String? Data { get; set; }
    public System.String Name { get; set; }
    public System.Boolean Searchable { get; set; }
    public System.Boolean Orderable { get; set; }
    public DatatablesSearch Search { get; set; }
}