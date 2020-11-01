using CodeGenHero.Inflector.Cultures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CodeGenHero.Inflector
{
    public partial class CodeGenHeroInflector : ICodeGenHeroInflector
    //, IPluralizer
    {
        private Dictionary<string, Lazy<CultureRules>> _localizedRules;
        private CultureInfo _currentCulture;

        public CodeGenHeroInflector()
        {
            _localizedRules = new Dictionary<string, Lazy<CultureRules>>
            {
                {"en", new Lazy<CultureRules>(() => new EnglishCultureRules())}
            };

            _currentCulture = new CultureInfo("en"); // Ensure we have a default value.
        }

        /// <summary>
        /// Returns true if able to set the current culture to the value provided.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public bool SetCurrentCulture(CultureInfo culture)
        {
            bool retVal = false;

            if (culture == null) throw new ArgumentNullException(nameof(culture));

            _currentCulture = GetMostSimilarCulture(culture);

            if (_currentCulture != null)
            {
                retVal = true;
            }
            else
            {
                if (SetDefaultCultureFunc != null)
                {
                    Debug.Write("Falling back to default culture.", "WARN");
                    _currentCulture = GetMostSimilarCulture(SetDefaultCultureFunc());
                }

                if (_currentCulture == null)
                {
                    throw new NotSupportedException($"The specificed culture '{culture.Name}' is not supported.");
                }
            }

            return retVal;
        }

        public bool SupportsCulture(CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));

            var mostSimilarCulture = GetMostSimilarCulture(culture);

            return mostSimilarCulture != null;
        }

        private CultureInfo GetMostSimilarCulture(CultureInfo culture)
        {
            var cultureName = culture.Name.ToLowerInvariant();
            if (_localizedRules.ContainsKey(cultureName))
            {
                return culture;
            }
            if (cultureName.Length > 2)
            {
                cultureName = cultureName.Substring(0, 2);
                if (_localizedRules.ContainsKey(cultureName))
                {
                    return new CultureInfo(cultureName);
                }
            }

            return null;
        }

        public Func<CultureInfo> SetDefaultCultureFunc = () => new CultureInfo("en");

        public CultureRules CurrentCultureRules => _localizedRules[_currentCulture.Name.ToLowerInvariant()]?.Value;

        private CultureRules GetCurrentRules()
        {
            return _localizedRules[_currentCulture.Name.ToLowerInvariant()].Value;
        }

        public string Pluralize(string word)
        {
            return ApplyRules(GetCurrentRules().Plurals, word);
        }

        public string Singularize(string word)
        {
            return ApplyRules(GetCurrentRules().Singulars, word);
        }

#if NET45 || NETFX_CORE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

        private string ApplyRules(List<Rule> rules, string word)
        {
            string result = null;

            if (!GetCurrentRules().Uncountables.Contains(word.ToLower()))
            {
                for (int i = rules.Count - 1; i >= 0; i--)
                {
                    if ((result = rules[i].Apply(word)) != null)
                    {
                        break;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = word;
            }

            return result;
        }

        public string Titleize(string word)
        {
            return Regex.Replace(Humanize(Underscore(word)), @"\b([a-z])",
                                 delegate (Match match)
                                 {
                                     return match.Captures[0].Value.ToUpper();
                                 });
        }

        public string ToLowerFirstCharacter(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return char.ToLower(value[0]) + value.Substring(1);
        }

        public string Humanize(string lowercaseAndUnderscoredWord)
        {
            return Capitalize(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
        }

        public string Pascalize(string lowercaseAndUnderscoredWord)
        {
            return Regex.Replace(lowercaseAndUnderscoredWord, "(?:^|_)(.)",
                                 delegate (Match match)
                                 {
                                     return match.Groups[1].Value.ToUpper();
                                 });
        }

        public string Camelize(string lowercaseAndUnderscoredWord)
        {
            return Uncapitalize(Pascalize(lowercaseAndUnderscoredWord));
        }

        public string Underscore(string pascalCasedWord)
        {
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
                    "$1_$2"), @"[-\s]", "_").ToLower();
        }

        public string Capitalize(string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }

        public string Uncapitalize(string word)
        {
            return word.Substring(0, 1).ToLower() + word.Substring(1);
        }

        public string Ordinalize(string numberString)
        {
            return Ordanize(int.Parse(numberString), numberString);
        }

        public string Ordinalize(int number)
        {
            return Ordanize(number, number.ToString());
        }

#if NET45 || NETFX_CORE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

        private string Ordanize(int number, string numberString)
        {
            int nMod100 = number % 100;

            if (nMod100 >= 11 && nMod100 <= 13)
            {
                return numberString + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return numberString + "st";

                case 2:
                    return numberString + "nd";

                case 3:
                    return numberString + "rd";

                default:
                    return numberString + "th";
            }
        }

        public string Dasherize(string underscoredWord)
        {
            return underscoredWord.Replace('_', '-');
        }
    }
}