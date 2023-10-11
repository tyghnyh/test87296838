using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsAPI;
using NewsVowel.Application;
using NewsVowel.Application.Services;
using NewsVowel.Infrastructure.NewsApi;

namespace NewsVowel.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<NewsApiOptions>(configuration.GetSection(nameof(NewsApiOptions)))
            .AddTransient<INewsApiClient, NewsApiClient>()
            .AddTransient<INewsProvder, NewsProvider>();

        return services;
    }
}