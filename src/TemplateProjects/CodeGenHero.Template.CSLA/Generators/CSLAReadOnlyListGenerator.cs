using CodeGenHero.Inflector;
using System.Collections.Generic;
using System.Text;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.CSLA.Generators
{
    public class CSLAReadOnlyListGenerator : BaseCSLAGenerator
    {
        public CSLAReadOnlyListGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        internal string GenerateActions(string baseNamespace, string namespacePostfix, string classNamespace,
                    IList<IEntityNavigation> excludeCircularReferenceNavigationProperties, IEntityType entity,
                    bool InludeRelatedObjects, bool prependSchemaNameIndicator)
        {
            var sb = new StringBuilder();
            string entityName = Inflector.Humanize(entity.ClrType.Name);

            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Text;");
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine($"using Csla; //https://github.com/MarimerLLC/csla");
            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"using {baseNamespace}.DataAccess;" : $"using {baseNamespace}.DataAccess.{namespacePostfix};");
            sb.AppendLine($"");
            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"namespace {baseNamespace}.Shared" : $"namespace {baseNamespace}.Shared.{namespacePostfix}");

            sb.AppendLine($"{{");
            sb.AppendLine("\t[Serializable]");
            sb.AppendLine($"\tpublic class {entityName}List : ReadOnlyListBase<{entityName}List, {entityName}Info>");
            sb.AppendLine($"\t{{");

            sb.AppendLine("\t\t[Fetch]");
            sb.AppendLine($"\t\tprivate void Fetch([Inject]I{entityName}Dal dal)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tIsReadOnly = false;");
            sb.AppendLine("\t\t\tvar data = dal.Get().Select(d => DataPortal.FetchChild<PersonInfo>(d));");
            sb.AppendLine("\t\t\tAddRange(data);");
            sb.AppendLine("\t\t\tIsReadOnly = true;");

            sb.AppendLine("\t\t}");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }
    }
}