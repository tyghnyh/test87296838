using NewsVowel.Application.Model;

namespace NewsVowel.Application.Services
{
    public interface INewsQueryHandler
    {
        IAsyncEnumerable<ArticleHandleResult> HandleAsync(string topic, ArticleCategories category);
    }
}
