namespace BlogAtor.Core.Entity;

using BlogAtor.Framework.Entity;

public class DataContent : EntityBase
{
    public System.String RawContent { get; set; }
    public System.Int64 DataItemId { get; set; }
}