
namespace Application.DTOs;

public sealed class ServiceResponse<T>
{
    public bool IsSuccessful { get; set; }
    public ErrorModel? Error { get; set; } 
    public T? Data { get; set; }

    public ServiceResponse()
    {
        
    }
    
    public ServiceResponse(bool isSuccessful, ErrorModel error, T? data)
    {
        IsSuccessful = isSuccessful;
        Error = error;
        Data = data;
    }

    public static ServiceResponse<T> Success(T data)
    {
        return new (true, null, data);
    }

    public static ServiceResponse<T> Failure(ErrorModel error)
    {
        return new(false, error, default);
    }
    
    public static ServiceResponse<T> Failure(string code , string message,  List<string> details = null)
    {
        return new(false, new ErrorModel(code, message, details), default);
    }
}

public sealed class ErrorModel
{
    public string Code { get; set; }
    public string Message { get; set; }
    public List<string> Details { get; set; }

    public ErrorModel()
    {
        
    }
    
    public ErrorModel(string code, string message, List<string>? details = null)
    {
        Code = code;
        Message = message;
        Details = details;
    }
}

public class FilterResponseModel<T>
{
    public int ItemsCount { get; set; }

    public IEnumerable<T> Data { get; set; }
}