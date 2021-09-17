using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;
using System.Linq;

namespace CodeGenHero.Template.Blazor.Generators
{
    class RepositoryCrudInterfaceGenerator : BaseBlazorGenerator
    {
        public RepositoryCrudInterfaceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IList<IEntityType> entities,
            string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic interface {className}");
            if (entities.Any())
            {
                var i = entities.Count();
                string comma = string.Empty;

                sb.Append(" : ");
                foreach (var entity in entities)
                {
                    var entityName = entity.ClrType.Name;
                    comma = (--i == 0) ? "" : ",";
                    string text = $"\t\tICRUDOperation<{entityName}>{comma}";
                    sb.AppendLine(text);
                }
            }
            sb.AppendLine("\t{");

            foreach(var entity in entities)
            {
                var entityName = entity.ClrType.Name;
                sb.AppendLine($"\t\tIQueryable<{entityName}> GetQueryable_{entityName}();");
            }

            sb.Append(GenerateFooter());
            return sb.ToString();
        }
    }
}
