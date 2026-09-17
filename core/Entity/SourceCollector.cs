namespace BlogAtor.Core.Entity;

using BlogAtor.Framework.Entity;

public class SourceCollector : EntityBase
{
    public System.Int64 DataSourceId { get; set; }
    public System.String CollectorType { get; set; }
    //public System.String CollectorParam { get; set; }
    public System.Int64 CollectorConfigId { get; set; }
}