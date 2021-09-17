using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;
using System.Linq;

namespace CodeGenHero.Template.Blazor.Generators
{
    class RepositoryInterfaceGenerator : BaseBlazorGenerator
    {
        public RepositoryInterfaceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IList<IEntityType> entities,
            string className,
            string repositoryCrudInterfaceName,
            string dbContextName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic partial interface {className} : {repositoryCrudInterfaceName}");
            sb.AppendLine("{");

            sb.AppendLine($"\t\t{dbContextName} {dbContextName} {{ get; }}");
            sb.AppendLine(string.Empty);

            foreach(var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string methodParameterSignature = GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey());

                sb.AppendLine($"Task<IRepositoryActionResult<ent{namespacePostfix}.{entityName}>> Delete_{entityName}Async({methodParameterSignature});");
                sb.AppendLine($"Task<ent{namespacePostfix}.{entityName}> Get_{entityName}Async({methodParameterSignature}, waEnums.RelatedEntitiesType relatedEntitiesType = waEnums.RelatedEntitiesType.None);");
                sb.AppendLine(string.Empty);
            }

            sb.Append(GenerateFooter());
            return sb.ToString();
        }
    }
}
