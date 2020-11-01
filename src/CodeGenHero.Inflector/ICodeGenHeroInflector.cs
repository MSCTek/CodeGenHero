using CodeGenHero.Inflector.Cultures;
using System.Globalization;

namespace CodeGenHero.Inflector
{
    public interface ICodeGenHeroInflector
    {
        CultureRules CurrentCultureRules { get; }

        string Camelize(string lowercaseAndUnderscoredWord);

        string Capitalize(string word);

        string Dasherize(string underscoredWord);

        string Humanize(string lowercaseAndUnderscoredWord);

        string Ordinalize(int number);

        string Ordinalize(string numberString);

        string Pascalize(string lowercaseAndUnderscoredWord);

        string Pluralize(string word);

        bool SetCurrentCulture(CultureInfo culture);

        string Singularize(string word);

        bool SupportsCulture(CultureInfo culture);

        string Titleize(string word);

        string ToLowerFirstCharacter(string value);

        string Uncapitalize(string word);

        string Underscore(string pascalCasedWord);
    }
}