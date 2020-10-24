namespace CodeGenHero.Inflector
{
    public interface ICodeGenHeroInflector
    {
        string Camelize(string lowercaseAndUnderscoredWord);

        string Capitalize(string word);

        string Dasherize(string underscoredWord);

        string Humanize(string lowercaseAndUnderscoredWord);

        string Ordinalize(string numberString);

        string Ordinalize(int number);

        string Pascalize(string lowercaseAndUnderscoredWord);

        string Pluralize(string word);

        string Singularize(string word);

        string Titleize(string word);

        string ToLowerFirstCharacter(string value);

        string Uncapitalize(string word);

        string Underscore(string pascalCasedWord);
    }
}