namespace Application.DTOs;

public class APIResponse
{
    public APIResponse(int statusCode, string title = null, string message = null, object result = null)
    {
        StatusCode = statusCode;
        if (StatusCode is 200 or 201)
        {
            IsSuccess = true;
        }
        else
        {
            IsSuccess = false;
        }
        Title = title ?? GetDefaultTitleOrMessage(statusCode);
        Message = message ?? GetDefaultTitleOrMessage(statusCode);
    }

    private string? GetDefaultTitleOrMessage(int statusCode)
    {
        return statusCode switch
        {
            200 or 201 => "Succes",
            400 => "Bad request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not found",
            500 => "Internal Server Error",
            _ => null
        };
    }

    public int StatusCode { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public object Result { get; set; }
    public bool IsSuccess { get; set; }
}