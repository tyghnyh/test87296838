using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsVowel.Application;
using NewsVowel.Infrastructure;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();

        var services = new ServiceCollection()
            .AddAppServices(configuration)
            .AddInfrastructureServices(configuration);

        var executor = services
            .AddSingleton<Executor, Executor>()
            .BuildServiceProvider()
            .GetService<Executor>() ?? throw new Exception("Ошибка DI");

        executor.ExecuteAsync(args).GetAwaiter().GetResult();

        Console.ReadLine();
    }
}
