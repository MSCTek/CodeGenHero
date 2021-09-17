using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;
using System.Linq;

namespace CodeGenHero.Template.Blazor.Generators
{
    class GenericFactoryInterfaceGenerator : BaseBlazorGenerator
    {
        public GenericFactoryInterfaceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"public interface {className}<TEntity, TDto>");
            sb.AppendLine("{");

            sb.AppendLine("\tTEntity Create(TDto item);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tTDto Create(TEntity item);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tobject CreateDataShapedObject(object item, List<string> fieldList, bool childrenRequested);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tobject CreateDataShapedObject(TEntity item, List<string> lstOfFields, bool childrenRequested);");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateFooter());
            return sb.ToString();
        }
    }
}
