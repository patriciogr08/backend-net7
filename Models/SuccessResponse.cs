public class SuccessResponse<T> where T : class
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public SuccessResponse(int statusCode, string message, T data = null)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }
}
