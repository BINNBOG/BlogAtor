namespace BlogAtor.API.Controllers;

using BlogAtor.Core.Entity;

public class SourceCollectorExportItem
{
    public SourceCollector SourceCollector { get; set; }
    public System.Object? Configuration { get; set; }
}
public class DataSourceExportItem
{
    public DataSource DataSource { get; set; }
    public ICollection<SourceCollectorExportItem> Collectors { get; set; }
}
public class DataSourceExportModel
{
    public DateTime ExportedAt { get; set; }
    public ICollection<DataSourceExportItem> DataSources { get; set; }
}