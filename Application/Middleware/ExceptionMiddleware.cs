using System.ComponentModel.DataAnnotations;
using Application.Constants;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

public sealed class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger)
{
    private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ApiException exception)
        {
            if (context.Response.HasStarted)
                throw;
            logger.LogError("Error message: {0}, error content: {1}", exception.Message, exception.Content);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = ContentTypes.JSON;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ServiceResponse<object>()
            {
                Data = null,
                Error = new ErrorModel()
                {
                    Code = StatusCodes.Status500InternalServerError.ToString(),
                    Details = [exception.Content, exception.InnerException?.Message],
                    Message = $"[{exception.Source}] " + exception.Message
                },
                IsSuccessful = false
            }, _jsonSerializerSettings));
        }
        catch (ValidationException exception)
        {
            if (context.Response.HasStarted)
                throw;

            logger.LogInformation("Validation error message: {0}, error content: {1}", exception.Message,
                string.Join(". ", exception.InnerException?.Message ?? exception.Message));

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            context.Response.ContentType = ContentTypes.JSON;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ServiceResponse<object>()
            {
                Data = null,
                Error = new ErrorModel()
                {
                    Code = StatusCodes.Status400BadRequest.ToString(),
                    Details = new List<string?>()
                    {
                        exception.InnerException?.Message ?? exception.Message
                    },
                    Message = "Validation exception!"
                },
                IsSuccessful = false
            }, _jsonSerializerSettings));

        }
        catch (UnauthorizedAccessException exception)
        {
            if (context.Response.HasStarted)
                throw;

            logger.LogInformation("Unauthorized exception message: {0}, error content: {1}", exception.Message,
                string.Join(". ", exception.InnerException?.Message ?? exception.Message));

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            context.Response.ContentType = ContentTypes.JSON;
        }
        catch (HttpRequestException exception)
        {
            if (context.Response.HasStarted)
                throw;

            logger.LogError("Error message: {0}, error content: {1}", exception.Message, string.Join(". ", exception.InnerException?.Message ?? exception.Message));

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            context.Response.ContentType = ContentTypes.JSON;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ServiceResponse<object>()
            {
                Data = null,
                Error = new ErrorModel()
                {
                    Code = StatusCodes.Status400BadRequest.ToString(),
                    Details = new List<string?>()
                    {
                        exception.InnerException?.Message ?? exception.Message
                    },
                    Message = "System exception!"
                },
                IsSuccessful = false
            }, _jsonSerializerSettings));
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
                throw;

            logger.LogError("Error message: {0}, error content: {1}", exception.Message, string.Join(". ", exception.InnerException?.Message ?? exception.Message));

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            context.Response.ContentType = ContentTypes.JSON;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ServiceResponse<object>()
            {
                Data = null,
                Error = new ErrorModel()
                {
                    Code = StatusCodes.Status400BadRequest.ToString(),
                    Details = new List<string?>()
                    {
                        exception.InnerException?.Message ?? exception.Message
                    },
                    Message = "System exception!"
                },
                IsSuccessful = false
            }, _jsonSerializerSettings));

        }
    }
}