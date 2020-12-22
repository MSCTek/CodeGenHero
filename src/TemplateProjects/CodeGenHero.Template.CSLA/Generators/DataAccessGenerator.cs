using CodeGenHero.Core.Metadata.Interfaces;
using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using System.Linq;

namespace CodeGenHero.Template.CSLA.Generators
{
    public class DataAccessGenerator : BaseCSLAGenerator
    {
        public DataAccessGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        internal string GenerateEntity(string baseNamespace, string namespacePostfix, string classNamespace,
                    IList<IEntityNavigation> excludeCircularReferenceNavigationProperties, IEntityType entity,
                    bool InludeRelatedObjects, bool prependSchemaNameIndicator)
        {
            var sb = new StringBuilder();
            string entityName = Inflector.Humanize(entity.ClrType.Name);

            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Text;");

            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"namespace {baseNamespace}.DataAccess" : $"namespace {baseNamespace}.DataAccess.{namespacePostfix}");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic partial class {entityName}Entity");
            sb.AppendLine($"\t{{");

            var primaryKeyList = GetPrimaryKeys(entity);
            var entityProperties = entity.GetProperties();
            foreach (var property in entityProperties)
            {
                string ctype = GetCType(property);
                string propertyName = property.Name;

                bool isPrimaryKey = primaryKeyList.Any(x => x.Equals(property.Name));

                sb.Append($"\t\tpublic {ctype} {Inflector.Pascalize(propertyName)} {{ get; set; }}");
                if (isPrimaryKey)
                    sb.AppendLine($" // Primary key");
                else
                    sb.AppendLine(string.Empty);
            }

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }
    }
}