namespace BlogAtor.Store.Sql.Data;

using BlogAtor.Core.Entity;

internal class DbDataContent : DataContent, IDbEntity<DataContent>
{
    public DbDataContent()
    {
    }
    public DbDataContent(DataContent entity)
    {
        Id = entity.Id;
        DataItemId = entity.DataItemId;
        RawContent = entity.RawContent;
    }

    public DbDataItem DataItem { get; set; }

    public DataContent ToEntity() => new DataContent()
    {
        Id = Id,
        DataItemId = DataItemId,
        RawContent = RawContent
    };
}
