using System.Text.RegularExpressions;

namespace CodeGenHero.Inflector
{
	public class Rule
	{
		private readonly Regex _regex;
		private readonly string _replacement;

		public Rule(string pattern, string replacement)
		{
			_regex = new Regex(pattern, RegexOptions.IgnoreCase);
			_replacement = replacement;
		}

		public string Apply(string word)
		{
			if (!_regex.IsMatch(word))
			{
				return null;
			}

			return _regex.Replace(word, _replacement);
		}
	}
}