namespace BlogAtor.Store.Sql.Data;

using BlogAtor.Core.Entity;

internal class DbDataSource : DataSource, IDbEntity<DataSource>
{
    public DbDataSource() {}
    public DbDataSource(DataSource entity)
    {
        Id = entity.Id;
        Name = entity.Name;
    }
    public ICollection<DbSourceCollector> Collectors { get; set; }
    public ICollection<DbDataItem> DataItems { get; set; }

    public DataSource ToEntity() => new DataSource()
    {
        Id = Id,
        Name = Name
    };
}