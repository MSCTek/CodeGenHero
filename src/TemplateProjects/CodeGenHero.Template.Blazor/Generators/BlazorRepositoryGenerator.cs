using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;
using System.Linq;

namespace CodeGenHero.Template.Blazor.Generators
{
    public class BlazorRepositoryGenerator : BaseBlazorGenerator
    {
        public BlazorRepositoryGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
            
        }

        #region Base Repository

        public string GenerateBaseClass(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            string entitiesNamespace,
            string dbContextClassName,
            bool prependSchemaNameIndicator)
        {
            var className = "BaseRepository";
            var camelDbContextClassName = Inflector.Camelize(dbContextClassName);

            // Being Generation
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));
            sb.AppendLine($"\tpublic abstract class {className}");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tprotected readonly {dbContextClassName} _{camelDbContextClassName};");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateBaseClassConstructor(className, dbContextClassName, camelDbContextClassName));

            //Close the class and namespace.
            sb.AppendLine("\t}\r\n}");

            return sb.ToString();
        }

        public string GenerateBaseClassConstructor(string className, string dbContextClassName, string camelDbContextClassName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}({dbContextClassName} {camelDbContextClassName})");
            sb.AppendLine("{");
            sb.AppendLine($"\t_{camelDbContextClassName} = {camelDbContextClassName};");
            sb.AppendLine("}");

            return sb.ToString();
        }

        #endregion

        /// Generate Interface
        #region Interface

        public string GenerateInterface(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            string entitiesNamespace,
            string dbContextClassName,
            bool prependSchemaNameIndicator,
            IEntityType entityType)
        {
            var entityName = $"{namespacePostfix}{entityType.ClrType.Name}";
            var humanizedEntityName = $"{namespacePostfix}{Inflector.Humanize(entityType.ClrType.Name)}";
            var className = $"I{humanizedEntityName}Repository";
            var modelName = entityType.ClrType.Name;
            var modelPkSignature = GetSignatureWithFieldTypes(string.Empty, entityType.FindPrimaryKey());

            // Begin Generation
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));
            sb.AppendLine($"\tpublic partial interface {className}");
            sb.AppendLine("\t{");

            sb.AppendLine($"\t{modelName} Get({modelPkSignature});");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\tIEnumerable<{modelName}> GetPage({modelPkSignature}, int pageSize);");

            //Close the class and namespace.
            sb.AppendLine("\t}\r\n}");

            return sb.ToString();
        }

        #endregion

        /// Generate Implementation
        #region Implementation

        public string GenerateImplementation(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            List<NameValue> maxRequestPerPageOverrides,
            List<NameValue> childIncludes,
            string entitiesNamespace,
            string dbContextClassName,
            bool prependSchemaNameIndicator,
            IEntityType entityType)
        {
            var entityName = $"{namespacePostfix}{entityType.ClrType.Name}";
            var humanizedEntityName = $"{namespacePostfix}{Inflector.Humanize(entityType.ClrType.Name)}";
            var className = $"{humanizedEntityName}Repository";
            var interfaceName = $"I{className}";
            var modelName = entityType.ClrType.Name;
            var modelPkSignature = GetSignatureWithFieldTypes(string.Empty, entityType.FindPrimaryKey());

            var camelDbContextClassName = Inflector.Camelize(dbContextClassName);

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

            // Parse Includes
            var includes = new List<string>();

            // Begin Generation
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));
            sb.AppendLine($"public partial class {className} : BaseRepository, {interfaceName}");
            sb.AppendLine("\t{");
            sb.Append(GenerateImplementationConstructor(className, dbContextClassName, camelDbContextClassName));
            sb.AppendLine(string.Empty);

            sb.Append(GenerateImplementationGet(entityType, modelName, camelDbContextClassName, maxPageSize, includes));

            sb.Append(GenerateImplementationPartialMethods(modelName));

            //Close the class and namespace.
            sb.AppendLine("\t}\r\n}");

            return sb.ToString();
        }

        public string GenerateImplementationConstructor(string className, string dbContextClassName, string camelDbContextClassName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}({dbContextClassName} {camelDbContextClassName})");
            sb.AppendLine($"\t: base({camelDbContextClassName})");
            sb.AppendLine("{");
            sb.AppendLine("}");

            return sb.ToString();
        }

        public string GenerateImplementationGet(IEntityType entity, string modelName, string camelDbContextClassName, int maxPageSize, List<string> childIncludes)
        {
            string dbcontext = $"_{camelDbContextClassName}";

            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateImplementationGetSingle(entity, modelName, dbcontext, childIncludes));
            sb.Append(GenerateImplementationGetPage(entity, modelName, dbcontext, maxPageSize, childIncludes));

            return sb.ToString();
        }

        public string GenerateImplementationGetPage(IEntityType entity, string modelName, string context, int maxPageSize, List<string> childIncludes)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public IEnumerable<{modelName}> GetPage(int page, int pageSize)");
            sb.AppendLine("{");

            sb.AppendLine("\tif (page <= 0)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tthrow new ArgumentException($\"{nameof(page)} must be greater than zero.\");");
            sb.AppendLine("\t}");
            sb.AppendLine($"\tif (pageSize > {maxPageSize} || pageSize <= 0)");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpageSize = {maxPageSize};");
            sb.AppendLine("\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\tvar qry = {context}.{Inflector.Pluralize(modelName)}");
            sb.Append(GenerateImplementationIncludes(childIncludes));
            sb.AppendLine("\t.AsQueryable();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t// Implement this Partial Method to inject custom query logic.");
            sb.AppendLine($"\tGetPageCustomLogic(ref qry);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("var retVal = qry.ToList();");
            sb.AppendLine("\treturn retVal;");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GenerateImplementationGetSingle(IEntityType entity, string modelName, string context, List<string> childIncludes)
        {
            var signature = $"Get({GetSignatureWithFieldTypes("", entity.FindPrimaryKey())})";
            var pk = $"{GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true)}";

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {modelName} {signature}");
            sb.AppendLine("{");
            sb.AppendLine($"\treturn {context}.{Inflector.Pluralize(modelName)}");
            sb.AppendLine($"\t\t.FirstOrDefault(x => x.{pk} == {pk});");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GenerateImplementationIncludes(List<string> childIncludes)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(3);

            foreach(var include in childIncludes)
            {
                sb.AppendLine($".Include(x => x.{include})");
            }

            return sb.ToString();
        }

        public string GenerateImplementationPartialMethods(string modelName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(1);

            sb.AppendLine($"partial void GetPageCustomLogic(ref IQueryable<{modelName}> qry);");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        #endregion
    }
}
