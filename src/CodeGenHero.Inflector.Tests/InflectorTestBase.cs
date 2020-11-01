using System.Collections.Generic;
using CodeGenHero.Inflector;

namespace CGH.Inflector.Tests
{
    public abstract class InflectorTestBase
    {
        public readonly Dictionary<string, string> TestData = new Dictionary<string, string>();
        protected ICodeGenHeroInflector Inflector { get; set; } = new CodeGenHero.Inflector.CodeGenHeroInflector();
    }
}