namespace BlogAtor.API;

public class ErrorInfo
{
    public System.String ErrorKey { get; private set; }
    public IDictionary<System.String, System.String> ErrorData { get; private set; }
    public ErrorInfo(System.String key)
    {
        ErrorKey = key;
        ErrorData = new Dictionary<System.String, System.String>();
    }
    public ErrorInfo WithData(System.String key, System.String value)
    {
        ErrorData.Add(key, value);
        return this;
    }
}