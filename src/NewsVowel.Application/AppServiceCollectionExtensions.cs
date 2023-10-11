using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsVowel.Application.Services;

namespace NewsVowel.Application;

public static class AppServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<INewsQueryHandler, NewsQueryHandler>();


        return services;
    }
}