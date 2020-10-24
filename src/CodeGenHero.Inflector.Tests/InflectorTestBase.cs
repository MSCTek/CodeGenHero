using System.Collections.Generic;

namespace CGH.Inflector.Tests
{
    public abstract class InflectorTestBase
    {
        public readonly Dictionary<string, string> TestData = new Dictionary<string, string>();
        protected ICodeGenHeroInflector Inflector { get; set; } = new CGH.Inflector.CodeGenHeroInflector();
    }
}