using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;

namespace CodeGenHero.Template.Blazor.Generators
{
    public class BlazorAPIControllerGenerator : BaseBlazorGenerator
    {
        public BlazorAPIControllerGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            List<NameValue> maxRequestPerPageOverrides,
            bool prependSchemaNameIndicator,
            IEntityType entityType)
        {
            var entityName = $"{entityType.ClrType.Name}{namespacePostfix}";
            var humanizedEntityName = $"{Inflector.Humanize(entityType.ClrType.Name)}{namespacePostfix}";
            var className = $"{humanizedEntityName}Controller";

            // Controller variable names
            string interfaceName = $"I{humanizedEntityName}Repository";
            string repositoryName = $"_{entityName}Repository";

            // Set max page size.
            var maxPageSize = 0;
            var maxRequestPerPageOverRideString = maxRequestPerPageOverrides
                .FirstOrDefault(x => x.Name == entityType.ClrType.Name)?.Value;
            if (!string.IsNullOrWhiteSpace(maxRequestPerPageOverRideString))
            {
                int.TryParse(maxRequestPerPageOverRideString, out maxPageSize);
            }

            if (maxPageSize <= 0)
            {
                maxPageSize = 100;
            }

            // Begin Generation.
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine("[AllowAnonymous]");
            sb.AppendLine($"[Route(\"api/[controller]\")]");
            sb.AppendLine("[ApiController]");
            sb.AppendLine($"\tpublic partial class {className} : Controller");
            sb.AppendLine("\t{");

            sb.AppendLine($"\t\tprivate const int maxPageSize = {maxPageSize};");
            sb.AppendLine($"\t\tprivate readonly {interfaceName} {repositoryName};");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateConstructor(className, interfaceName, repositoryName, entityName));

            /// Operations: (TODO: Should probably put in place some fork for Authorized vs Anonymous APIs? tl;dr Anonymous API Controller should only have access to Read)
            // Generate Read operation
            sb.Append(GenerateRead());
            // Generate Create operation

            // Generate Update operation

            // Generate Delete operation

            //Close the class and namespace.
            sb.AppendLine("\t}\r\n}");

            return sb.ToString();
        }

        public string GenerateConstructor(string className, string interfaceName, string repositoryName, string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}({interfaceName} {repositoryName})");
            sb.AppendLine("{");
            sb.AppendLine($"\t{repositoryName} = {entityName}Repository;");
            sb.AppendLine("}");

            return sb.ToString();
        }

        public string GenerateRead() //WIP
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            return sb.ToString();
        }
    }
}
