using System.Globalization;
using System.Net.Http.Headers;
using Application.Helpers;
using Infrastructure.DTOs;
using Application.Helpers;
using Refit;
using UI.Services;
namespace UI.Services;

public static class ServiceDependencyInjection
{
    public static IServiceCollection AddRefitService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<AuthTokenHandler>();
        var url = configuration.GetValue<string>($"{nameof(AppOptions)}:{nameof(AppOptions.BackendApi)}");
        services
            .AddRefitClient<IAccountRefitClient>()
            .ConfigureHttpClient(CreateHttpClient(url))
            .AddHttpMessageHandler<AuthTokenHandler>();
        
        services
            .AddRefitClient<IChannelRefitService>()
            .AddHttpMessageHandler<AuthTokenHandler>()
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