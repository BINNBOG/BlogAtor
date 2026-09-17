namespace BlogAtor.Store.Sql.Data;

using BlogAtor.Core.Entity;

public class DbDataItem : DataItem, IDbEntity<DataItem>
{
    public DbDataItem() {}
    public DbDataItem(DataItem entity)
    {
        Id = entity.Id;
        DataSourceId = entity.DataSourceId;
        CollectorId = entity.CollectorId;

        Uid = entity.Uid;

        Title = entity.Title;
        Link = entity.Link;
        UpdatedDate = entity.UpdatedDate;
        CollectedDate = entity.CollectedDate;
    }
    internal DbDataSource DataSource { get; set; }
    internal DbSourceCollector Collector { get; set; }
    internal ICollection<DbDataContent> Contents { get; set; }

    public DataItem ToEntity() => new DataItem()
    {
        Id = Id,
        DataSourceId = DataSourceId,
        CollectorId = CollectorId,

        Uid = Uid,

        Title = Title,
        Link = Link,
        UpdatedDate = UpdatedDate,
        CollectedDate = CollectedDate
    };

    public bool IsPublished { get; set; } = false;
}