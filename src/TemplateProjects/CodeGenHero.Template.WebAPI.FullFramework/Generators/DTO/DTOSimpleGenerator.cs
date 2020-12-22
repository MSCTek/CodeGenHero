using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using System.Linq;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.DTO
{
    public class DTOSimpleGenerator : BaseAPIFFGenerator
    {
        private const string EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION = " -- Excluded navigation property per configuration.";

        public DTOSimpleGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateConstructor(
            bool prependSchemaNameIndicator,
            bool dtoIncludeRelatedObjects,
            IEntityType entity,
            IList<IEntityNavigation> excludedNavigationProperties)
        {
            StringBuilder sb = new StringBuilder();
            string entityName = Inflector.Pascalize(entity.ClrType.Name);

            // Constructor .ctor code
            sb.AppendLine($"\t\tpublic {entityName}()");
            sb.AppendLine("\t\t{");

            if (dtoIncludeRelatedObjects)
            {
                #region Collection Initialization

                // Commenting out default initialization of collections to better align with being able to detect/lazy load data in the model classes.
                //foreach (var reverseFK in table.ReverseNavigations)
                //{
                //	string name = reverseFK.RefTableHumanCase;
                //	bool excludeCircularReferenceNavigationIndicator = reverseFK.ExcludeCircularReferenceNavigationIndicator(excludedNavigationProperties);

                //	sb.Append($"\t\t\t");
                //	if (excludeCircularReferenceNavigationIndicator)
                //	{   // Include the line, but comment it out.
                //		sb.Append($"// ");
                //	}

                //	if (!(reverseFK.Relationship == Library.Enums.Relationship.OneToOne))
                //	{
                //		sb.Append($"{name} = new System.Collections.Generic.List<{reverseFK.FullRefTableName}>();");
                //	}

                //	if (excludeCircularReferenceNavigationIndicator)
                //	{
                //		sb.AppendLine(EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION);
                //	}
                //	else
                //	{
                //		sb.AppendLine(string.Empty);
                //	}
                //}

                //if (table.ReverseNavigations.Count > 0)
                //{
                //	sb.AppendLine(string.Empty);
                //}

                #endregion Collection Initialization
            }

            sb.AppendLine("\t\t\tInitializePartial();");
            sb.AppendLine("\t\t}");

            return sb.ToString();
        }

        public string GenerateDTO(
            IList<IEntityNavigation> excludedNavigationProperties,
            IEntityType entity, string namespacePostfix,
            string baseNamespace, bool dtoIncludeRelatedObjects, bool prependSchemaNameIndicator, string dtoNamespace
        )
        {
            var sb = new StringBuilder();
            string entityName = Inflector.Pascalize(entity.ClrType.Name);

            string useDTONamespace = dtoNamespace.Replace("baseNamespace", baseNamespace).Replace("namespacePostfix", namespacePostfix);

            sb.AppendLine($"using System;");
            sb.AppendLine($"namespace {useDTONamespace}");
            //sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"namespace {baseNamespace}.DTO" : $"namespace {baseNamespace}.DTO.{namespacePostfix}");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic partial class {entityName}");
            sb.AppendLine($"\t{{");
            sb.AppendLine(GenerateConstructor(
                prependSchemaNameIndicator: prependSchemaNameIndicator,
                dtoIncludeRelatedObjects: dtoIncludeRelatedObjects,
                entity: entity,
                excludedNavigationProperties: excludedNavigationProperties));

            //var keys = entity.Keys;
            var primaryKeyList = GetPrimaryKeys(entity);

            var entityProperties = entity.GetProperties();
            foreach (var property in entityProperties)
            {
                // string ctype = property.ClrType.Name;
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;
                //string ctype = property.Name; //.CType();
                // property.Key seems to be inaccessible.
                //bool isPrimaryKey = keys.Where(x => x.Key[0].Name.Equals(property.Key));
                bool isPrimaryKey = primaryKeyList.Any(x => x.Equals(property.Name));

                sb.Append($"\t\tpublic {simpleType} {Inflector.Pascalize(propertyName)} {{ get; set; }}"); // was NameCamelCase
                if (isPrimaryKey)
                    sb.AppendLine($" // Primary key");
                else
                    sb.AppendLine(string.Empty);
            }

            sb.Append(GenerateReverseNav(
                prependSchemaNameIndicator: prependSchemaNameIndicator,
                dtoIncludeRelatedObjects: dtoIncludeRelatedObjects,
                entity: entity,
                excludedNavigationProperties: excludedNavigationProperties));

            sb.AppendLine(string.Empty);
            sb.AppendLine("\t\tpartial void InitializePartial();");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }

        public string GenerateReverseNav(bool prependSchemaNameIndicator,
            bool dtoIncludeRelatedObjects,
            IEntityType entity,
            IList<IEntityNavigation> excludedNavigationProperties)
        {
            StringBuilder sb = new StringBuilder();
            SortedSet<IForeignKey> fklist = entity.ForeignKeys;
            // CGHInflector myInflector = new CGHInflector();

            if (dtoIncludeRelatedObjects)
            {
                foreach (var reverseFK in entity.Navigations)
                {
                    string name = reverseFK.DeclaringType.ClrType.Name;
                    string humanCase = Inflector.Humanize(name);

                    sb.Append("\t\t");
                    //bool excludeCircularReferenceNavigationIndicator = reverseFK.ExcludeCircularReferenceNavigationIndicator(excludedNavigationProperties);
                    bool excludeCircularReferenceNavigationIndicator = IsEntityInExcludedReferenceNavigionationProperties(excludedNavigationProperties, name);
                    if (excludeCircularReferenceNavigationIndicator)
                    {   // Include the line, but comment it out.
                        sb.Append("// ");
                    }

                    string reverseFKName = reverseFK.ForeignKey.PrincipalEntityType.ClrType.Name;

                    // We don't have the same Relationship.OneToOne status as from the reverse poco type.  EntityFramework just gives us the type of ICollection'1 or the field name
                    //if (reverseFK.ClrType == Library.Enums.Relationship.OneToOne)
                    if (!reverseFK.ClrType.Name.Equals("ICollection`1"))
                    {
                        sb.Append($"public virtual {reverseFKName} {Inflector.Pascalize(reverseFKName)} {{ get; set; }} // One to One mapping"); // Foreign Key
                    }
                    else
                    {
                        sb.Append($"public virtual System.Collections.Generic.ICollection<{reverseFK.ForeignKey.DeclaringEntityType.ClrType.Name}> {reverseFK.Name} {{ get; set; }} // Many to many mapping"); // Foreign Key
                    }

                    if (excludeCircularReferenceNavigationIndicator)
                    {
                        sb.AppendLine(EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION);
                    }
                    else
                    {
                        sb.AppendLine(string.Empty);
                    }
                }

                // With Reverse POCO, we had to go forward and backward.  However, by using entity.Navigations, the following seems duplicative.
                //var orderedFKs = entity.ForeignKeys.OrderBy(x => x.DependentToPrincipal.Name).ToList();
                //foreach (var fk in orderedFKs)
                //{
                //    string principalEntityTypeName = fk.PrincipalEntityType.ClrType.Name;
                //    string propertyName = "";
                //    if (string.IsNullOrEmpty(principalEntityTypeName))
                //    {
                //        sb.AppendLine($"// ERROR: {entity.Name} foreign key for {fk.PrincipalEntityType.Name}: A PrincipalEntityType.Name is null");
                //    }
                //    else
                //    {
                //        propertyName = principalEntityTypeName.Humanize(); // Humanize the name
                //    }
                //    sb.Append("\t\t");

                //    // TODO: Not sure if this is correct.  Does the excludedNavigationProperties exclude by table/ref or just by table? Or just by column?
                //    //bool excludeCircularReferenceNavigationIndicator = fk.ExcludeCircularReferenceNavigationIndicator(excludedNavigationProperties);
                //    bool excludeCircularReferenceNavigationIndicator = IsEntityInExcludedReferenceNavigionationProperties(excludedNavigationProperties, propertyName);

                //    if (excludeCircularReferenceNavigationIndicator)
                //    {   // Include the line, but comment it out.
                //        sb.Append("// ");
                //    }

                //    sb.Append($"public virtual {principalEntityTypeName} {propertyName} {{ get; set; }} "); // Foreign Key

                //    if (excludeCircularReferenceNavigationIndicator)
                //    {
                //        sb.AppendLine(EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION);
                //    }
                //    else
                //    {
                //        sb.AppendLine(string.Empty);
                //    }
                //}
            }

            return sb.ToString();
        }
    }
}