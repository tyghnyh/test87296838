using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using NewsVowel.Application;
using NewsVowel.Application.Model;
using NewsVowel.Application.Services;

namespace NewsVowel.Tests.Unit.Application.Services
{
    public class NewsQueryHandlerTest
    {
        [Theory]
        [InlineAutoData]
        public async Task GetMaxEnVowelWord_OkAsync([Frozen] Mock<INewsProvder> newsApiProviderMock)
        {
            var sut = new NewsQueryHandler(newsApiProviderMock.Object);

            newsApiProviderMock
                .Setup(q => q.GetNewsAsync(It.IsAny<string>(), It.IsAny<ArticleCategories>(), It.IsAny<Languages>()))
                .Returns((new[] { new Article("author", "title", "desctiption", "author title desctiption content \" \" 123 ") }).ToAsyncEnumerable());

            var result = sut.HandleAsync("abc", ArticleCategories.Business);

            var actual = await result.FirstAsync();
            actual.NewsContent.Should().Be("author title desctiption content \" \" 123 ");
            actual.MaxVowelCountWord.Should().Be("desctiption");
        }

        [Theory]
        [InlineAutoData]
        public async Task GetMaxRuVowelWord_OkAsync([Frozen] Mock<INewsProvder> newsApiProviderMock)
        {
            var sut = new NewsQueryHandler(newsApiProviderMock.Object);

            newsApiProviderMock
                .Setup(q => q.GetNewsAsync(It.IsAny<string>(), It.IsAny<ArticleCategories>(), It.IsAny<Languages>()))
                .Returns((new[] { new Article("автор", "тема", "описание", "автор тема описание содержание \" \" 123 ") }).ToAsyncEnumerable());

            var result = sut.HandleAsync("абв", ArticleCategories.Business);

            var actual = await result.FirstAsync();
            actual.NewsContent.Should().Be("автор тема описание содержание \" \" 123 ");
            actual.MaxVowelCountWord.Should().Be("описание");
        }
    }
}