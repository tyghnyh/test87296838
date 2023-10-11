using Microsoft.Extensions.Options;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsVowel.Application;
using NewsVowel.Application.Model;
using NewsVowel.Infrastructure.NewsApi;

namespace NewsVowel.Infrastructure
{
    internal class NewsProvider : INewsProvder
    {
        private readonly INewsApiClient _newsApiClient;
        private readonly NewsApiOptions _options;

        public NewsProvider(INewsApiClient newsApiClient, IOptions<NewsApiOptions> options)
        {
            _newsApiClient = newsApiClient;
            _options = options.Value;
        }

        public async IAsyncEnumerable<Article> GetNewsAsync(string topic, ArticleCategories category, Languages language)
        {
            int page = 1;

            var cat = (Categories)(int)category;

            while(page <= _options.MaxPageCount )
            {
                var request = new TopHeadlinesRequest {Q = topic, Category = cat, LanguageCode = language.ToString(), Page = page, PageSize = _options.PageSize};

                var response = await _newsApiClient.GetTopHeadlinesAsync(request);

                if (response.Status != Statuses.Ok)
                {
                    throw new Exception(response.Error.Message);
                }

                if (response.TotalResults == 0)
                {
                    break;
                }

                foreach (var a in response.Articles)
                {
                    yield return new Article(a.Author, a.Title, a.Description, a.Content);
                }
               
                page++;
            }
        }
    }
}