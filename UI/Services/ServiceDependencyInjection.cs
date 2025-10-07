using System.Globalization;
using System.Net.Http.Headers;
using Application.Helpers;
using Infrastructure.DTOs;
using Application.Helpers;
using Application.ServiceContract;
using Infrastructure.Client;
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

        services
            .AddRefitClient<ICategoryRefitService>()
            .AddHttpMessageHandler<AuthTokenHandler>()
            .ConfigureHttpClient(CreateHttpClient(url));

        services
            .AddRefitClient<ICourseRefitService>()
            .AddHttpMessageHandler<AuthTokenHandler>()
            .ConfigureHttpClient(CreateHttpClient(url));

        services
            .AddRefitClient<IStorageRefitService>()
            .AddHttpMessageHandler<AuthTokenHandler>()
            .ConfigureHttpClient(CreateHttpClient(url));

        services
            .AddRefitClient<ICourseRefitService>()
            .AddHttpMessageHandler<AuthTokenHandler>()
            .ConfigureHttpClient(CreateHttpClient(url));

        services
            .AddRefitClient<IContentRefitService>()
            .AddHttpMessageHandler<AuthTokenHandler>()
            .ConfigureHttpClient(CreateHttpClient(url));

        services
            .AddRefitClient<IModuleRefitService>()
            .AddHttpMessageHandler<AuthTokenHandler>()
            .ConfigureHttpClient(CreateHttpClient(url));
        
        services
            .AddRefitClient<IQuizRefitService>()
            .AddHttpMessageHandler<AuthTokenHandler>()
            .ConfigureHttpClient(CreateHttpClient(url));

        services.AddRefitClient<IOllamaRefitService>()
            .ConfigureHttpClient(c => 
            {
                c.BaseAddress = new Uri("http://localhost:11434");
                c.Timeout = TimeSpan.FromMinutes(5); 
            });
        
        services.AddRefitClient<IGPTRefitService>()
            .ConfigureHttpClient(c =>
            {
                /*c.BaseAddress = new Uri("https://api.openai.com");
                c.DefaultRequestHeaders.Add("Authorization",
                    "Bearer gpt-key");*/
                c.Timeout = TimeSpan.FromMinutes(5);
            });

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