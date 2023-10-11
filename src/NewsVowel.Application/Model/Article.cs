namespace NewsVowel.Application.Model
{
    public class Article
    {
        public Article(string? author, string? title, string? description, string? content)
        {
            Author = author;
            Title = title;
            Description = description;
            Content = content;
        }

        public string? Author { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Content { get; set; }
    }
}
