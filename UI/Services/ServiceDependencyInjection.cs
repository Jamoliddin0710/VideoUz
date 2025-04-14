using System.Globalization;
using System.Net.Http.Headers;
using Infrastructure.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
namespace Application.ServiceContract;

public static class ServiceDependencyInjection
{
    public static IServiceCollection AddRefitService(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration.GetValue<string>($"{nameof(AppOptions)}:{nameof(AppOptions.BackendApi)}");
        services
            .AddRefitClient<IAccountRefitClient>()
            .ConfigureHttpClient(CreateHttpClient(url));
        
        return services;
    }
    
    private static Action<HttpClient> CreateHttpClient(string route)
    {
   #if DEBUG
        var timeout = TimeSpan.FromMinutes(2);
  #else
        var timeout = TimeSpan.FromSeconds(10);
  #endif
        return s =>
        {
            s.BaseAddress = new Uri(route);
            s.DefaultRequestHeaders.AcceptLanguage.Add(
                new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.ToString()));
            s.Timeout = timeout;
        };
    }
}