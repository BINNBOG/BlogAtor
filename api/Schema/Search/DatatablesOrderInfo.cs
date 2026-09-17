namespace BlogAtor.API;

public class DatatablesOrderInfo
{
    private readonly System.String cOrderAscending = "asc";
    public System.Int64 Column { get; set; }
    public System.String Dir { get; set; }
    public System.Boolean Ascending => Dir == cOrderAscending;
    public System.String Name { get; set ;}
}