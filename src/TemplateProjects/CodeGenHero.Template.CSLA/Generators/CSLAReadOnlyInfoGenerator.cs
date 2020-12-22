using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template.CSLA.Generators
{
    public class CSLAReadOnlyInfoGenerator : BaseCSLAGenerator
    {
        public CSLAReadOnlyInfoGenerator(ICodeGenHeroInflector inflector) : base(inflector)
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
            sb.AppendLine($"using Csla; //https://github.com/MarimerLLC/csla");
            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"using {baseNamespace}.DataAccess;" : $"using {baseNamespace}.DataAccess.{namespacePostfix};");
            sb.AppendLine($"");
            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"namespace {baseNamespace}.Shared" : $"namespace {baseNamespace}.Shared.{namespacePostfix}");

            sb.AppendLine($"{{");
            sb.AppendLine("\t[Serializable]");
            sb.AppendLine($"\tpublic class {entityName}Info : ReadOnlyBase<{entityName}Info>");
            sb.AppendLine($"\t{{");

            var primaryKeyList = GetPrimaryKeys(entity);
            var entityProperties = entity.GetProperties();
            foreach (var property in entityProperties)
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;

                sb.AppendLine($"\t public static readonly PropertyInfo<{simpleType}> {propertyName}Property = RegisterProperty<{simpleType}>(nameof({propertyName}));");

                sb.AppendLine($"\t public {simpleType} {propertyName}");
                sb.AppendLine("\t {");
                sb.AppendLine($"\t\t get {{return GetProperty({propertyName}Property);}}");
                sb.AppendLine($"\t\t private set {{ LoadProperty({propertyName}Property, value);}}");
                sb.AppendLine("\t }");
                sb.AppendLine(string.Empty);
            }

            //////////////////FETCH CHILD
            sb.AppendLine("\t [FetchChild]");
            sb.AppendLine($"\t private void Fetch({entityName}Entity data)");
            sb.AppendLine("\t {");
            foreach (var property in entityProperties)
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;
                sb.AppendLine($"\t\t {propertyName} = data.{propertyName};");
            }

            sb.AppendLine("\t }");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }
    }
}