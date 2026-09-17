namespace BlogAtor.API;

public class APIResponse<DT> : APIResponse
{
    public DT? Data { get; set; }
}

public class APIResponse
{
    public System.Boolean IsSuccess { get; set; }
    public ErrorInfo? ErrorInfo { get; set; }
    public static APIResponse Success() => new APIResponse()
    {
        IsSuccess = true
    };
    public static APIResponse Error(ErrorInfo errorInfo) => new APIResponse()
    {
        IsSuccess = false,
        ErrorInfo = errorInfo
    };
    public static APIResponse<DT> Success<DT>(DT data) => new APIResponse<DT>()
    {
        IsSuccess = true,
        Data = data
    };
    public static APIResponse<DT> Error<DT>(ErrorInfo errorInfo) => new APIResponse<DT>()
    {
        IsSuccess = false,
        ErrorInfo = errorInfo
    };
}