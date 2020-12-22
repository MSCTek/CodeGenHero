using System.Text;
using CodeGenHero.Inflector;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Server
{
    public class RepositoryGenerator : BaseAPIFFGenerator
    {
        public RepositoryGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateRepository(
            string baseNamespace,
            string namespacePostfix,
            string repositoryEntitiesNamespace,
            string dbContextName,
            string repositoryInterfaceNamespace)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"using {repositoryEntitiesNamespace};");
            sb.AppendLine($"using {repositoryInterfaceNamespace};");
            sb.AppendLine($"using System;");
            sb.AppendLine($"using Microsoft.EntityFrameworkCore;");
            sb.AppendLine($"using System.Linq;");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {baseNamespace}.Repository");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic partial class {namespacePostfix}Repository : {namespacePostfix}RepositoryBase, I{namespacePostfix}Repository");
            sb.AppendLine($"\t{{");
            sb.AppendLine($"\t\tpublic {namespacePostfix}Repository({dbContextName} ctx) : base(ctx)");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t}}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tpublic {dbContextName} {dbContextName} {{ get {{ return DbContext; }} }}");
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");
            return sb.ToString();
        }
    }
}