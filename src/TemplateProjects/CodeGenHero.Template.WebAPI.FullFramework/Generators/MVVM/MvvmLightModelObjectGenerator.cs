using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.MVVM
{
    public class MvvmLightModelObjectGenerator : BaseAPIFFGenerator
    {
        public MvvmLightModelObjectGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateConstructor(bool prependSchemaNameIndicator,
            IList<IEntityNavigation> excludedNavigationProperties,
            IEntityType entity)
        {
            StringBuilder sb = new StringBuilder();
            SortedSet<IForeignKey> fklist = entity.ForeignKeys;

            // Constructor .ctor code
            sb.AppendLine($"\t\tpublic {Inflector.Pascalize(entity.ClrType.Name)}()");
            sb.AppendLine("\t\t{");

            foreach (var navigation in entity.Navigations)
            {
                // Really, we only want to exclude these if the referenced Foreign Key table does not exist in our metadata (was excluded by regex).
                //bool excludeCircularReferenceNavigationIndicator = reverseFK.ExcludeCircularReferenceNavigationIndicator(excludedNavigationProperties);
                //if (excludeCircularReferenceNavigationIndicator)
                //{
                //	sb.AppendLine($"\t\t\t// Excluding '{name}' per configuration setting.");
                //	continue;
                //}

                if (navigation.ClrType.Name.Equals("ICollection`1"))
                {
                    //Collection
                }
                else if (navigation.ClrType.Name.Equals("List`1"))
                {
                    //List
                    string fkNamePl = Inflector.Pluralize(navigation.Name);
                    string fkName = Inflector.Singularize(navigation.Name);

                    sb.AppendLine($"\t\t\t{fkNamePl} = new System.Collections.Generic.List<{fkName}>(); // Reverse Navigation");
                }
            }

            if (entity.Navigations.Count > 0)
            {
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("\t\t\tInitializePartial();");
            sb.AppendLine("\t\t}");

            return sb.ToString();
        }

        public string GenerateFooter()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }

        public string GenerateHeader(string classNamespace, string baseAuditEditNamespace, bool containsAuditEditFields)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using GalaSoft.MvvmLight;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System;");
            if (containsAuditEditFields)
            {
                if (!string.IsNullOrEmpty(baseAuditEditNamespace)
                    && !baseAuditEditNamespace.Equals(classNamespace, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    sb.AppendLine($"using {baseAuditEditNamespace};");
                }
            }

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {classNamespace}");
            sb.AppendLine($"{{");

            return sb.ToString();
        }

        public string GenerateMvvmLightModelObject(
            string classNamespace,
            string baseAuditEditNamespace,
            bool prependSchemaNameIndicator,
            IList<IEntityNavigation> excludedEntityNavigations,
            IEntityType entity)
        {
            string entityName = Inflector.Pascalize(entity.ClrType.Name);
            StringBuilder sb = new StringBuilder();
            var entityProperties = entity.GetProperties();
            bool containsAuditEditFields = ContainsAuditEditFields(entityProperties);

            sb.Append(GenerateHeader(classNamespace, baseAuditEditNamespace, containsAuditEditFields));

            if (containsAuditEditFields)
            {
                sb.AppendLine($"\tpublic partial class {entityName} : BaseAuditEdit");
            }
            else
            {
                sb.AppendLine($"\tpublic partial class {entityName} : ObservableObject");
            }
            sb.AppendLine("\t{");
            sb.AppendLine(GenerateConstructor(prependSchemaNameIndicator: prependSchemaNameIndicator,
                excludedNavigationProperties: excludedEntityNavigations, entity: entity));

            // Add the properties
            foreach (var property in entityProperties)
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;
                if ((!containsAuditEditFields || !IsColumnAnAuditEditField(property))
                    && !IsUnknownType(property))
                {
                    sb.AppendLine($"\t\tprivate {simpleType} {GetNameModuleLevelVariable(propertyName)};");
                }
            }

            sb.AppendLine(string.Empty);

            // For each field in properties, add a public method
            foreach (var property in entityProperties.OrderBy(x => x.Name).ToList())
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;
                if ((!containsAuditEditFields || !IsColumnAnAuditEditField(property))
                    && !IsUnknownType(property))
                {
                    sb.AppendLine(string.Empty);
                    sb.AppendLine($"\t\tpublic {simpleType} {propertyName}");
                    sb.AppendLine("\t\t{");
                    sb.AppendLine("\t\t\tget { return " + GetNameModuleLevelVariable(propertyName) + "; }");
                    sb.AppendLine("\t\t\tset");
                    sb.AppendLine("\t\t\t{");
                    sb.AppendLine("\t\t\t\tSet<" + simpleType + ">(() => " + propertyName + ", ref " + GetNameModuleLevelVariable(propertyName) + ", value);");
                    sb.AppendLine($"\t\t\t\tRunCustomLogicSet{propertyName}(value);");
                    sb.AppendLine("\t\t\t}");
                    sb.AppendLine("\t\t}");
                }
            }

            sb.AppendLine(string.Empty);
            sb.AppendLine(GenerateNavigationProperties(prependSchemaNameIndicator: prependSchemaNameIndicator,
                excludedNavigationProperties: excludedEntityNavigations, entity: entity));
            sb.AppendLine(string.Empty);
            sb.AppendLine("\t\tpartial void InitializePartial();");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#region RunCustomLogicSet"); // For each field in columns, add a partial method to run custom logic in each property settter.
            sb.AppendLine(string.Empty);
            foreach (var property in entityProperties.OrderBy(x => x.Name).ToList())
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;
                if ((!containsAuditEditFields || !IsColumnAnAuditEditField(property))
                    && !IsUnknownType(property))
                {
                    sb.AppendLine($"\t\tpartial void RunCustomLogicSet{propertyName}({simpleType} value);");
                }
            }
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#endregion RunCustomLogicSet"); // For each field in columns, add a partial method to run custom logic in each property settter.

            sb.Append(GenerateFooter());

            return sb.ToString();
        }

        public string GenerateNavigationProperties(bool prependSchemaNameIndicator,
            IList<IEntityNavigation> excludedNavigationProperties,
            IEntityType entity)
        {
            StringBuilder sb = new StringBuilder();
            SortedSet<IForeignKey> fklist = entity.ForeignKeys;

            foreach (var navigation in entity.Navigations)
            {
                bool navigationIsExcluded = excludedNavigationProperties.Any(x => x.Navigation == navigation);
                if (navigationIsExcluded)
                {
                    continue;
                }

                string fkNamePl = Inflector.Pluralize(navigation.Name);
                string fkName = Inflector.Singularize(navigation.Name);
                //Really, we only want to exclude these if the referenced Foreign Key table does not exist in our metadata (was excluded by regex).
                //bool excludeCircularReferenceNavigationIndicator = reverseFK.ExcludeCircularReferenceNavigationIndicator(excludedNavigationProperties);
                //if (excludeCircularReferenceNavigationIndicator)
                //{
                //	sb.AppendLine($"\t\t// Excluding '{name}' per configuration setting.");
                //	continue;
                //}

                if (navigation.ClrType.Name.Equals("ICollection`1"))
                {
                    //Collection
                    sb.AppendLine($"\t\tpublic virtual IList<{fkName}> {fkNamePl} {{ get; set; }} = new List<{fkName}>();"); // Many to many mapping
                }
                else if (navigation.ClrType.Name.Equals("List`1"))
                {
                    //List
                    sb.AppendLine($"\t\tpublic virtual IList<{fkName}> {fkNamePl} {{ get; set; }} = new List<{fkName}>(); "); // Many to many mapping
                }
                else
                {
                    sb.AppendLine($"\t\tpublic virtual {navigation.ClrType.Name} {navigation.ClrType.Name} {{ get; set; }} "); // Foreign Key
                }
            }

            return sb.ToString();
        }
    }
}