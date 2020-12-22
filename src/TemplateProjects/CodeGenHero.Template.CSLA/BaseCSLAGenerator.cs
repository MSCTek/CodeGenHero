using CodeGenHero.Inflector;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.CSLA
{
    public abstract class BaseCSLAGenerator : BaseGenerator
    {
        public BaseCSLAGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }
    }
}