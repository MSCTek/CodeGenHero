using CodeGenHero.Core.Metadata.Interfaces;
using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;

namespace CodeGenHero.Template.CSLA.Generators
{
    public class DataAccessInterfaceGenerator : BaseCSLAGenerator
    {
        public DataAccessInterfaceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        internal string GenerateInterface(string baseNamespace, string namespacePostfix, string classNamespace,
            bool InludeRelatedObjects, bool prependSchemaNameIndicator, IList<IEntityNavigation> excludeCircularReferenceNavigationProperties,
            IEntityType entity)
        {
            var sb = new StringBuilder();
            string entityName = Inflector.Pascalize(Inflector.Humanize(entity.ClrType.Name));
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Text;");
            sb.AppendLine($"using System.Threading.Tasks;");
            sb.AppendLine(string.Empty);
            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"namespace {baseNamespace}.DataAccess" : $"namespace {baseNamespace}.DataAccess.{namespacePostfix}");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic partial interface I{entityName}Dal");
            sb.AppendLine($"\t{{");

            ////////////
            // DELETE
            ////////////
            sb.AppendLine($"\t bool Delete({GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey())});");
            sb.AppendLine(string.Empty);
            ////////////
            // EXISTS
            ////////////
            sb.AppendLine($"\t bool Exists({GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey())});");
            sb.AppendLine(string.Empty);
            ////////////
            // SINGLE GET
            ////////////
            sb.AppendLine($"\t {entityName}Entity Get({GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey())});");
            sb.AppendLine(string.Empty);
            ////////////
            // List GET
            ////////////
            sb.AppendLine($"\t List<{entityName}Entity> Get();");
            sb.AppendLine(string.Empty);
            ////////////
            // INSERT
            ////////////
            sb.AppendLine($"\t {entityName}Entity Insert({entityName}Entity {Inflector.ToLowerFirstCharacter(entityName)});");
            sb.AppendLine(string.Empty);
            ////////////
            // UPDATE
            ////////////

            sb.AppendLine($"\t {entityName}Entity Update({entityName}Entity {Inflector.ToLowerFirstCharacter(entityName)});");

            sb.AppendLine(string.Empty);

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }
    }
}