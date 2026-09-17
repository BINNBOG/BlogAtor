namespace BlogAtor.Store.Sql.Data;

using BlogAtor.Core.Entity;

internal class DbSourceCollector : SourceCollector, IDbEntity<SourceCollector>
{
    public DbSourceCollector() {}
    public DbSourceCollector(SourceCollector entity)
    {
        Id = entity.Id;
        DataSourceId = entity.DataSourceId;
        CollectorType = entity.CollectorType;
        //CollectorParam = entity.CollectorParam;
        CollectorConfigId = entity.CollectorConfigId;
    }
    public DbDataSource DataSource { get; set; }
    public ICollection<DbDataItem> DataItems { get; set; }

    public SourceCollector ToEntity() => new SourceCollector()
    {
        Id = Id,
        DataSourceId = DataSourceId,
        CollectorType = CollectorType,
        //CollectorParam = CollectorParam
        CollectorConfigId = CollectorConfigId
    };
}