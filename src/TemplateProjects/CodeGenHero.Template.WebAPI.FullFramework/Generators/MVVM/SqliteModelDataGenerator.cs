using System.Text;
using CodeGenHero.Inflector;
using System.Linq;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.MVVM
{
    public class SqliteModelDataGenerator : BaseAPIFFGenerator
    {
        public SqliteModelDataGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateHeader(string classNamespace, string baseAuditEditNamespace, bool containsAuditEditFields)
        {
            StringBuilder sb = new StringBuilder();

            if (containsAuditEditFields)
            {
                if (!string.IsNullOrEmpty(baseAuditEditNamespace)
                    && !baseAuditEditNamespace.Equals(classNamespace, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    sb.AppendLine($"using {baseAuditEditNamespace};"); //CodeGenHero.EAMVCXamPOCO.Xam.ModelData
                }
            }

            sb.AppendLine($"using SQLite;");
            sb.AppendLine($"using System;");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {classNamespace}");
            sb.AppendLine($"{{");

            return sb.ToString();
        }

        public string GenerateSqliteModelData(
            string classNamespace,
            string baseAuditEditNamespace,
            bool prependSchemaNameIndicator,
            IEntityType entity)
        {
            string tableName = Inflector.Pascalize(entity.ClrType.Name);
            StringBuilder sb = new StringBuilder();
            var entityProperties = entity.GetProperties().OrderBy(n => n.Name).ToList();
            bool containsAuditEditFields = ContainsAuditEditFields(entityProperties);

            sb.Append(GenerateHeader(classNamespace, baseAuditEditNamespace, containsAuditEditFields));

            sb.AppendLine($"\t[Table(\"{entity.ClrType.Name}\")]");
            if (containsAuditEditFields)
            {
                sb.AppendLine($"\tpublic partial class {tableName} : BaseAuditEdit");
            }
            else
            {
                sb.AppendLine($"\tpublic partial class {tableName}");
            }

            sb.AppendLine("\t{");
            int pknum = 1;
            string pkstring = string.Empty;
            string compositePKFieldName = string.Empty;
            bool hasMultiplePrimaryKeys = entity.FindPrimaryKey().Properties.Count > 1;
            var primaryKey = entity.FindPrimaryKey();
            for (int i = 0; i < entityProperties.Count; i++)
            {
                var property = entityProperties[i];
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;

                if ((!containsAuditEditFields || !IsColumnAnAuditEditField(property))
                    && !IsUnknownType(property))
                {
                    if (primaryKey.Properties.Where(x => x.Name == propertyName).Any())
                    {
                        sb.AppendLine(string.Empty);
                        if (hasMultiplePrimaryKeys)
                        {
                            //pkstring = $"\t\t// Mutiple primary keys - composite PK used instead [Indexed(Name = \"{tableName}\", Order = {pknum++}, Unique = true)]";
                            pkstring = $"\t\t// Mutiple primary keys - composite PK used instead ";
                            compositePKFieldName += propertyName;
                        }
                        else
                        {
                            pkstring = "\t\t[PrimaryKey]";
                        }

                        sb.AppendLine(pkstring);
                    }
                    sb.AppendLine($"\t\tpublic {simpleType} {propertyName} {{ get; set; }}");

                    if (propertyName == primaryKey.Properties[0].Name)
                    {
                        sb.AppendLine(string.Empty);  // Blank line after the primary key(s) for visual effect only.
                    }
                }
            }

            if (hasMultiplePrimaryKeys)
            {
                sb.AppendLine($"\t\t[PrimaryKey]\n\t\tpublic string {compositePKFieldName} {{ get; set; }}");
            }

            sb.Append(GenerateFooter());

            return sb.ToString();
        }
    }
}