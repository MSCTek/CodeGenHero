using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;
using System.Linq;

namespace CodeGenHero.Template.Blazor.Generators
{
    class DTOGenerator : BaseBlazorGenerator
    {
        public DTOGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IEntityType entity,
            IList<IEntityNavigation> excludedEntityNavigations,
            bool dtoIncludeRelatedObjects,
            string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));



            sb.Append(GenerateFooter());
            return sb.ToString();
        }
    }
}
