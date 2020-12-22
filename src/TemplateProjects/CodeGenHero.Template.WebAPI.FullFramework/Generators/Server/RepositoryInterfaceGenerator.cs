using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using System;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Server
{
    public class RepositoryInterfaceGenerator : BaseAPIFFGenerator
    {
        public RepositoryInterfaceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateRepositoryInterface(
            string namespacePostfix,
            string baseNamespace,
            string repositoryEntitiesNamespace,
            string dbContextName,
             IList<IEntityType> EntityTypes)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using CodeGenHero.Repository;");
            sb.AppendLine($"using {repositoryEntitiesNamespace};");
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Threading.Tasks;");

            sb.AppendLine(string.Empty);

            sb.AppendLine($"namespace {baseNamespace}.Repository.Interface");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic partial interface I{namespacePostfix}Repository : I{namespacePostfix}RepositoryCrud");
            sb.AppendLine($"\t{{");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t{dbContextName} {dbContextName} {{ get; }}");
            sb.AppendLine(string.Empty);

            foreach (var entity in EntityTypes)
            {
                string tableName = entity.ClrType.Name;

                string methodParameterSignature = GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey());

                // Note, the ICRUDOperation takes care of most items.  We just need to add a couple additional items:
                //sb.AppendLine($"\t\t#region {tableName}{Environment.NewLine}");
                sb.AppendLine($"\t\tTask<IRepositoryActionResult<{tableName}>> Delete_{tableName}Async({methodParameterSignature});");

                sb.Append($"\t\tTask<{tableName}> Get_{tableName}Async({methodParameterSignature}");
                if (!string.IsNullOrEmpty(methodParameterSignature))
                {
                    sb.Append($", ");
                }
                sb.AppendLine($"int numChildLevels);{Environment.NewLine}");
                //sb.AppendLine($"{Environment.NewLine}\t\t#endregion {tableName}{Environment.NewLine}");
            }

            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }
    }
}