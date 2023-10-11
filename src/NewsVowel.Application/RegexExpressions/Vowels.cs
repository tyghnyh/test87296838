using System.Dynamic;
using System.Text.RegularExpressions;

namespace NewsVowel.Application.RegexExpressions
{
    public static class Vowels
    {
        public static Regex RuRegex{ get; } = new Regex(@"([АОИЕУЁЭЮЯЫаоиеуёэюяы])");

        public static Regex EnRegex { get; } = new Regex(@"([AEIOUYaeiouy])");
    }
}
