using System.Collections.Generic;
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
            var entityName = $"{namespacePostfix}{entityType.ClrType.Name}";
            var humanizedEntityName = $"{namespacePostfix}{Inflector.Humanize(entityType.ClrType.Name)}";
            var className = $"{humanizedEntityName}Controller";

            // Controller variable names
            string interfaceName = $"I{humanizedEntityName}Repository";
            string repositoryName = $"_{entityName}Repository";

            // Set max page size. // The API shouldn't be responsible for this - Max Page Size logic should belong to the Repo.
            //var maxPageSize = 0;
            //var maxRequestPerPageOverRideString = maxRequestPerPageOverrides
            //    .FirstOrDefault(x => x.Name == entityType.ClrType.Name)?.Value;
            //if (!string.IsNullOrWhiteSpace(maxRequestPerPageOverRideString))
            //{
            //    int.TryParse(maxRequestPerPageOverRideString, out maxPageSize);
            //}

            //if (maxPageSize <= 0)
            //{
            //    maxPageSize = 100;
            //}

            // Begin Generation.
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine("[AllowAnonymous]");
            sb.AppendLine($"[Route(\"api/[controller]\")]");
            sb.AppendLine("[ApiController]");
            sb.AppendLine($"\tpublic partial class {className} : Controller");
            sb.AppendLine("\t{");

            //sb.AppendLine($"\t\tprivate const int maxPageSize = {maxPageSize};");
            sb.AppendLine($"\t\tprivate readonly {interfaceName} {repositoryName};");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateConstructor(className, interfaceName, repositoryName, entityName));

            /// Operations: (TODO: Should probably put in place some fork for Authorized vs Anonymous APIs? tl;dr Anonymous API Controller should only have access to Read/Get)
            // Generate Get operation
            sb.Append(GenerateGet(entityType, repositoryName));

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

            sb.AppendLine($"public {className}({interfaceName} {entityName}Repository)");
            sb.AppendLine("{");
            sb.AppendLine($"\t{repositoryName} = {entityName}Repository;");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GenerateGet(IEntityType entity, string repositoryName) //WIP
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateGetSingle(entity, repositoryName));
            sb.Append(GenerateGetPage(entity, repositoryName));

            return sb.ToString();
        }

        public string GenerateGetPage(IEntityType entity, string repositoryName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("[HttpGet]");
            sb.AppendLine($"[Route(\"GetPage/{{page}}/{{pageSize}}\")]");
            sb.AppendLine($"public IActionResult GetPage(int page, int? pageSize)");
            sb.AppendLine("{");
            sb.AppendLine($"var retVal = {repositoryName}.GetPage(page, pageSize);");
            sb.Append(GenerateGetReturn());
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GenerateGetSingle(IEntityType entity, string repositoryName)
        {
            var apiGetSignature = $"Get({GetSignatureWithFieldTypes("", entity.FindPrimaryKey())})";
            var repoGetSignature = $"{GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true)}";

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("[HttpGet]");
            sb.AppendLine($"[Route(\"Get/{{{repoGetSignature}}}\")]");
            sb.AppendLine($"public IActionResult {apiGetSignature}");
            sb.AppendLine("{");
            sb.AppendLine($"var retVal = {repositoryName}.Get({repoGetSignature});");
            sb.Append(GenerateGetReturn());
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GenerateGetReturn()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(3);

            sb.AppendLine("if (retVal == null)");
            sb.AppendLine("{");
            sb.AppendLine("return NotFound();");
            sb.AppendLine("}");
            sb.AppendLine("else");
            sb.AppendLine("{");
            sb.AppendLine("return Ok(retVal);");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
