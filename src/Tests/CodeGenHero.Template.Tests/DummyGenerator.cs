using CodeGenHero.Inflector;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.Tests
{
    public class DummyGenerator : BaseGenerator
    {
        public DummyGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }
    }
}