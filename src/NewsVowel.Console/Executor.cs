using NewsVowel.Application.Model;
using NewsVowel.Application.Services;

public class Executor
{
    private readonly INewsQueryHandler _handler;

    public Executor(INewsQueryHandler handler)
    {
        _handler = handler;
    }

    public async Task ExecuteAsync(string[] args)
    {
        string topic;
        string category;

        if (args.Length == 2)
        {
            topic = args[0];
            category = args[1];
        }
        else
        {
            Console.WriteLine(@"Введите ключевое слово поиска");
            topic = Console.ReadLine();

            Console.WriteLine($"Введите категорию из списка: {string.Join(", ", Enum.GetNames(typeof(ArticleCategories)))}");
            category = Console.ReadLine();
        }



        if (!Enum.TryParse(typeof(ArticleCategories), category, out var cat))
        {
            Console.WriteLine($"Категория новостей может быть одной из следующих: {string.Join(", ", Enum.GetNames(typeof(ArticleCategories)))}");
        }

        try
        {
            await foreach (var a in _handler.HandleAsync(topic, (ArticleCategories)cat!))
            {
                Console.WriteLine($"{a.NewsContent} | {a.MaxVowelCountWord}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}