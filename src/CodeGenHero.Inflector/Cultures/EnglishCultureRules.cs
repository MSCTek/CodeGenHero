using System.Globalization;

namespace CodeGenHero.Inflector.Cultures
{
	public class EnglishCultureRules : CultureRules
	{
		public EnglishCultureRules()
		{
			AddPlural("$", "s");
			AddPlural("s$", "s");
			AddPlural("(ax|test)is$", "$1es");
			AddPlural("(octop|vir|alumn|fung)us$", "$1i");
			AddPlural("(statu)$", "$1ses"); // EF has a habit of calling tables named "AbcStatus" "AbcStatu" in the singular form.
			AddPlural("(alias|status)$", "$1es");

			AddPlural("(bu)s$", "$1ses");
			AddPlural("(buffal|tomat|volcan)o$", "$1oes");
			AddPlural("([ti])um$", "$1a");
			AddPlural("sis$", "ses");
			AddPlural("(?:([^f])fe|([lr])f)$", "$1$2ves");
			AddPlural("(hive)$", "$1s");
			AddPlural("([^aeiouy]|qu)y$", "$1ies");
			AddPlural("(x|ch|ss|sh)$", "$1es");
			AddPlural("(matr|vert|ind)ix|ex$", "$1ices");
			AddPlural("([m|l])ouse$", "$1ice");
			AddPlural("^(ox)$", "$1en");
			AddPlural("(quiz)$", "$1zes");

			AddSingular("s$", "");
			AddSingular("(address)$", "$1");
			AddSingular("(n)ews$", "$1ews");
			AddSingular("([ti])a$", "$1um");
			AddSingular("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
			AddSingular("(^analy)ses$", "$1sis");
			AddSingular("([^f])ves$", "$1fe");
			AddSingular("(hive)s$", "$1");
			AddSingular("(tive)s$", "$1");
			AddSingular("([lr])ves$", "$1f");
			AddSingular("([^aeiouy]|qu)ies$", "$1y");
			AddSingular("(s)eries$", "$1eries");
			AddSingular("(m)ovies$", "$1ovie");
			AddSingular("(x|ch|ss|sh)es$", "$1");
			AddSingular("([m|l])ice$", "$1ouse");
			AddSingular("(bus)es$", "$1");
			AddSingular("(o)es$", "$1");
			AddSingular("(shoe)s$", "$1");
			AddSingular("(cris|ax|test)es$", "$1is");
			AddSingular("(octop|vir|alumn|fung)i$", "$1us");
			AddSingular("(alias|status)es$", "$1");
			AddSingular("^(ox)en", "$1");
			AddSingular("(vert|ind)ices$", "$1ex");
			AddSingular("(matr)ices$", "$1ix");
			AddSingular("(quiz)zes$", "$1");

			// AddIrregular("person", "people"); -- "Both people and persons are acceptable plurals of person. But people is much more commonly used; persons is usually only used in legal contexts".
			// Going with 'peoples' just because I do think developers might want/expect to see "Persons" instead of "People".

			AddIrregular("man", "men");
			AddIrregular("child", "children");
			AddIrregular("sex", "sexes");
			AddIrregular("move", "moves");
			AddIrregular("goose", "geese");
			AddIrregular("alumna", "alumnae");

			AddUncountable("equipment");
			AddUncountable("information");
			AddUncountable("rice");
			AddUncountable("money");
			AddUncountable("species");
			AddUncountable("series");
			AddUncountable("fish");
			AddUncountable("sheep");
			AddUncountable("deer");
			AddUncountable("aircraft");
		}
	}
}