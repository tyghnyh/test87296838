namespace NewsVowel.Application.Model
{
    /// <summary>
    /// Результат обработки новостной статьи
    /// </summary>
    /// <param name="NewsContent">Новость</param>
    /// <param name="MaxVowelCountWord">Слово с максимальным количеством гласных</param>
    public record ArticleHandleResult(string NewsContent, string MaxVowelCountWord);
}
