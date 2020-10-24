using System;
using System.Collections.Generic;

namespace CodeGenHero.Inflector.Cultures
{
	public abstract class CultureRules
	{
		private List<Rule> _plurals = new List<Rule>();
		private List<Rule> _singulars = new List<Rule>();
		private List<string> _uncountables = new List<string>();

		public List<Rule> Plurals { get { return _plurals; } }

		public List<Rule> Singulars { get { return _singulars; } }

		public List<string> Uncountables { get { return _uncountables; } }

		protected void AddIrregular(string singular, string plural)
		{
			AddPlural("(" + singular[0] + ")" + singular.Substring(1) + "$", "$1" + plural.Substring(1));
			AddSingular("(" + plural[0] + ")" + plural.Substring(1) + "$", "$1" + singular.Substring(1));
		}

		protected void AddPlural(string rule, string replacement)
		{
			_plurals.Add(new Rule(rule, replacement));
		}

		protected void AddSingular(string rule, string replacement)
		{
			_singulars.Add(new Rule(rule, replacement));
		}

		protected void AddUncountable(string word)
		{
			_uncountables.Add(word.ToLower());
		}
	}
}