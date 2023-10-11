using NewsVowel.Application.Model;

namespace NewsVowel.Application
{
    public interface INewsProvder
    {
        IAsyncEnumerable<Article> GetNewsAsync(string topic, ArticleCategories category, Languages language);
    }
}
