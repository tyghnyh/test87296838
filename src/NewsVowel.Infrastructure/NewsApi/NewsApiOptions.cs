namespace NewsVowel.Infrastructure.NewsApi
{
    public class NewsApiOptions
    {
        public string ApiKey { get; set; }

        public int PageSize { get; set; } = 10;

        public int MaxPageCount { get; set; } = 10;
    }
}
