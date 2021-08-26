using System.Text.RegularExpressions;

namespace Alifery.Aligo.Search.Core
{
    public static class MatchCriteria
    {
        public static bool ExactMatch(this string input, string match)
        {
            return Regex.IsMatch(input.ToLower(), string.Format(@"\b{0}\b", Regex.Escape(match.ToLower())));
        }
    }
}