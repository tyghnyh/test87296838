using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.Options;
using NewsVowel.Infrastructure.NewsApi;

namespace NewsAPI
{
    /// <summary>
    /// Use this to get results from NewsAPI.org.
    /// </summary>
    public class NewsApiClient : INewsApiClient
    {
        private string BASE_URL = "https://newsapi.org/v2/";

        private HttpClient HttpClient;

        private string ApiKey;

        /// <summary>
        /// Use this to get results from NewsAPI.org.
        /// </summary>
        public NewsApiClient(IOptions<NewsApiOptions> options)
        {
            ApiKey = options.Value.ApiKey;

            HttpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
            HttpClient.DefaultRequestHeaders.Add("user-agent", "News-API-csharp/0.1");
            HttpClient.DefaultRequestHeaders.Add("x-api-key", ApiKey);
        }

        /// <summary>
        /// Query the /v2/top-headlines endpoint for live top news headlines.
        /// </summary>
        /// <param name="request">The params and filters for the request.</param>
        /// <returns></returns>
        public async Task<ArticlesResult> GetTopHeadlinesAsync(TopHeadlinesRequest request)
        {
            // build the querystring
            var queryParams = new List<string>();

            // q
            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                queryParams.Add("q=" + request.Q);
            }

            // sources
            if (request.Sources.Count > 0)
            {
                queryParams.Add("sources=" + string.Join(",", request.Sources));
            }

            if (request.Category.HasValue)
            {
                queryParams.Add("category=" + request.Category.Value.ToString().ToLowerInvariant());
            }

            if (!string.IsNullOrEmpty(request.LanguageCode))
            {
                queryParams.Add("language=" + request.LanguageCode.ToLowerInvariant());
            }

            if (request.Country.HasValue)
            {
                queryParams.Add("country=" + request.Country.Value.ToString().ToLowerInvariant());
            }

            // page
            if (request.Page > 1)
            {
                queryParams.Add("page=" + request.Page);
            }

            // page size
            if (request.PageSize > 0)
            {
                queryParams.Add("pageSize=" + request.PageSize);
            }

            // join them together
            var querystring = string.Join("&", queryParams.ToArray());

            return await MakeRequest("top-headlines", querystring);
        }

        // ***

        private async Task<ArticlesResult> MakeRequest(string endpoint, string querystring)
        {
            // here's the return obj
            var articlesResult = new ArticlesResult();

            // make the http request
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, BASE_URL + endpoint + "?" + querystring);
            var httpResponse = await HttpClient.SendAsync(httpRequest);            

            var json = await httpResponse.Content?.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(json))
            {
                // convert the json to an obj
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json);
                articlesResult.Status = apiResponse.Status;
                if (articlesResult.Status == Statuses.Ok)
                {
                    articlesResult.TotalResults = apiResponse.TotalResults;
                    articlesResult.Articles = apiResponse.Articles;
                }
                else
                {
                    ErrorCodes errorCode = ErrorCodes.UnknownError;
                    try
                    {
                        errorCode = (ErrorCodes)apiResponse.Code;
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("The API returned an error code that wasn't expected: " + apiResponse.Code);
                    }

                    articlesResult.Error = new Error
                    {
                        Code = errorCode,
                        Message = apiResponse.Message
                    };
                }
            }
            else
            {
                articlesResult.Status = Statuses.Error;
                articlesResult.Error = new Error
                {
                    Code = ErrorCodes.UnexpectedError,
                    Message = "The API returned an empty response. Are you connected to the internet?"
                };
            }

            return articlesResult;
        }
    }
}
