using System.Text.RegularExpressions;
using NewsVowel.Application.Model;
using NewsVowel.Application.RegexExpressions;

namespace NewsVowel.Application.Services
{
    public class NewsQueryHandler : INewsQueryHandler
    {
        private readonly INewsProvder _newsProvder;
        private readonly Regex WordsSplitRegex = new Regex(@"(\W)");

        public NewsQueryHandler(INewsProvder newsProvder)
        {
            _newsProvder = newsProvder;
        }

        public async IAsyncEnumerable<ArticleHandleResult> HandleAsync(string topic, ArticleCategories category)
        {
            Languages language = Languages.EN;
            Regex vowelRegex = Vowels.EnRegex;

            if (Vowels.RuRegex.Matches(topic).Count > 0)
            {
                language = Languages.RU;
                vowelRegex = Vowels.RuRegex;
            }

            var articles = _newsProvder.GetNewsAsync(topic, category, language);

            await foreach (var a in articles)
            {
                var text = a.Content ?? a.Description ?? a.Title;
                yield return new ArticleHandleResult(text, GetMaxVowelWord(text, vowelRegex));
            }

        }

        private string GetMaxVowelWord(string text, Regex vowelRegex)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string[] words = Regex.Split(text, @"[\W]+", RegexOptions.IgnorePatternWhitespace);

            int maxVowels = 0;
            var word = string.Empty;

            foreach (var w in words)
            {
                var currentVowels = vowelRegex.Matches(w).Count();

                if (maxVowels < currentVowels)
                {
                    maxVowels = currentVowels;
                    word = w;
                }
            }

            return word;
        }
    }
}
