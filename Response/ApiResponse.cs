namespace AMNApi.Response;

public class ApiResponse<T>
{
    public ApiResponse(T data, string? message = null, MetaDataResponse? meta = null)
    {
        Data = data;
        Message = message;
        Meta = meta;
    }

    public T Data { get; set; }
    public string? Message { get; set; }
    public MetaDataResponse? Meta { get; set; }
}