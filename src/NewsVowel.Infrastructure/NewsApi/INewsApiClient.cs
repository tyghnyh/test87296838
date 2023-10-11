using NewsAPI.Models;

namespace NewsVowel.Infrastructure.NewsApi
{
    internal interface INewsApiClient
    {
        Task<ArticlesResult> GetTopHeadlinesAsync(TopHeadlinesRequest request);
    }
}
