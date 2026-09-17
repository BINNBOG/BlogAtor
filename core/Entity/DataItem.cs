namespace BlogAtor.Core.Entity;

using BlogAtor.Framework.Entity;

public class DataItem : EntityBase
{
    public System.String Uid { get; set; }
    public DataItemType ItemType { get; set; }
    public DateTime CollectedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public System.String Title { get; set; }
    public System.String Link { get; set; }

    public System.Int64 DataSourceId { get; set; }
    public System.Int64 CollectorId { get; set; }
}