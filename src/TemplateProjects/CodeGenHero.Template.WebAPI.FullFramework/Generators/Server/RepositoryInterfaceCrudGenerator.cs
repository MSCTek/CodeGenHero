using System.Text;
using System.Collections.Generic;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Server
{
    public class RepositoryInterfaceCrudGenerator : BaseAPIFFGenerator
    {
        public RepositoryInterfaceCrudGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateRepositoryInterfaceCrud(
            string repositoryInterfaceNamespace,
            string namespacePostfix,
            string repositoryEntitiesNamespace,
             IList<IEntityType> EntityTypes)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"using CodeGenHero.Repository;");
            sb.AppendLine($"using {repositoryEntitiesNamespace};");
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"namespace {repositoryInterfaceNamespace}");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic interface I{namespacePostfix}RepositoryCrud : ");

            int i = EntityTypes.Count;
            string comma = ",";
            foreach (var entityType in EntityTypes)
            {
                string entityName = entityType.ClrType.Name;

                comma = (--i == 0) ? "" : ",";
                string text = $"\t\tICRUDOperation<{entityName}>{comma}";
                sb.AppendLine(text);
            }

            sb.AppendLine($"\t{{");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t#region GetQueryable");
            sb.AppendLine(string.Empty);

            foreach (var entityType in EntityTypes)
            {
                string entityName = entityType.ClrType.Name;
                sb.AppendLine($"\t\tIQueryable<{entityName}> GetQueryable_{entityName}();");
            }

            sb.AppendLine(string.Empty);
            sb.AppendLine("\t\t#endregion");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }
    }
}