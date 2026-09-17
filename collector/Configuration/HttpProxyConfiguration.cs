namespace BlogAtor.Collector.Configuration;

public class HttpProxyConfiguration
{
    public System.String Url { get; set; }
    public System.Boolean Authentication { get; set; }
    public System.String? Username { get; set; }
    public System.String? Password { get; set; }
}