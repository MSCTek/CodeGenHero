using CodeGenHero.Core.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.DTO
{
    public class ModelsBackedByDtoGenerator : BaseAPIFFGenerator
    {
        public ModelsBackedByDtoGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateConstructor(bool prependSchemaNameIndicator,
            string webApiDataServiceInterfaceClassName, IEntityType entity)
        {
            StringBuilder sb = new StringBuilder();
            //List<ForeignKey> fklist = table.ForeignKeys;
            SortedSet<IForeignKey> fklist = entity.ForeignKeys;
            //CGHInflector myInflector = new CGHInflector();

            // First Constructor .ctor code

            string entityName = Inflector.Pascalize(entity.ClrType.Name);

            // Prepended the entity name with the schema, if required
            string dtoEntityName = entityName;
            //TODO: Currently we don't have the schema name in the entity, I believe. We need a metadatasource with schemanames to test it
            //if (prependSchemaNameIndicator && !string.IsNullOrEmpty(entity.SchemaName) && !entity.SchemaName.ToLower().Equals("dbo"))
            //{
            //	dtoEntityName = $"{entity.SchemaName}.{entityName}";
            //}

            sb.AppendLine($"\t\tpublic event EventHandler<LoadRequest{entityName}> OnLazyLoadRequest = delegate {{ }}; // Empty delegate. Thus we are sure that value is always != null because no one outside of the class can change it.");
            sb.AppendLine($"\t\tprivate xDTO.{dtoEntityName} _dto = null;");
            sb.AppendLine(string.Empty);

            string signature = $"ILoggingService log, IDataService<{webApiDataServiceInterfaceClassName}> dataService";
            sb.AppendLine($"\t\tpublic {entityName}({signature}) : base(log, dataService)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\t_dto = new xDTO.{dtoEntityName}();");
            sb.AppendLine($"\t\t\tOnLazyLoadRequest += HandleLazyLoadRequest;");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            signature = $"ILoggingService log, IDataService<{webApiDataServiceInterfaceClassName}> dataService, xDTO.{dtoEntityName} dto";
            sb.AppendLine($"\t\tpublic {entityName}({signature}) : this(log, dataService)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\t_dto = dto;");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

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

        public string GenerateHeader(string baseNamespace, string namespacePostfix,
            string classNamespace,
            string modelsBackedByDtoInterfaceNamespace,
            string webApiDataServiceInterfaceNamespace,
            string webApiDataServiceInterfaceClassName,
            IEntityType entity)
        {
            string entityName = Inflector.Pascalize(entity.ClrType.Name);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using CodeGenHero.Logging;");
            sb.AppendLine($"using CodeGenHero.DataService;");
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine($"using {webApiDataServiceInterfaceNamespace};");
            sb.AppendLine($"using {modelsBackedByDtoInterfaceNamespace};");
            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"using xDTO = { baseNamespace }.DTO;" : $"using xDTO = {baseNamespace}.DTO.{namespacePostfix};");

            sb.AppendLine(string.Empty);
            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"namespace {classNamespace}" : $"namespace {classNamespace}.{namespacePostfix}");
            sb.AppendLine($"{{");

            sb.AppendLine(string.Empty);
            sb.AppendLine(GenerateLoadRequestEventArgs(entity: entity));
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\tpublic partial class {entityName} : BaseModel<{webApiDataServiceInterfaceClassName}>, I{entityName}");

            sb.AppendLine("\t{");

            return sb.ToString();
        }

        public string GenerateModelsBackedByDto(
            string baseNamespace,
            string namespacePostfix,
            string classNamespace,
            bool prependSchemaNameIndicator,
            string modelsBackedByDtoInterfaceNamespace,
            string webApiDataServiceInterfaceNamespace,
            string webApiDataServiceInterfaceClassName,
            IList<IEntityNavigation> excludedNavigationProperties,
            IEntityType entity)
        {
            string entityName = Inflector.Pascalize(entity.ClrType.Name);
            string dtoTableName = entityName;
            //if (prependSchemaNameIndicator && !string.IsNullOrEmpty(entity.SchemaName) && !entity.SchemaName.ToLower().Equals("dbo"))
            //{
            //	dtoEntityName = $"{entity.SchemaName}.{entityName}";
            //}

            StringBuilder sb = new StringBuilder();
            var entityProperties = entity.GetProperties();
            //var tabcols = table.Columns.OrderBy(x => x.Name).ToList();

            sb.Append(GenerateHeader(baseNamespace: baseNamespace, namespacePostfix: namespacePostfix,
                classNamespace: classNamespace, modelsBackedByDtoInterfaceNamespace: modelsBackedByDtoInterfaceNamespace,
                webApiDataServiceInterfaceNamespace: webApiDataServiceInterfaceNamespace,
                webApiDataServiceInterfaceClassName: webApiDataServiceInterfaceClassName, entity: entity));

            sb.AppendLine(GenerateConstructor(prependSchemaNameIndicator: prependSchemaNameIndicator,
                webApiDataServiceInterfaceClassName: webApiDataServiceInterfaceClassName, entity: entity));

            // Add the columns.  These only have a get on _dto.name
            foreach (var property in entityProperties)
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;

                //string columnName = col.GetNameModuleLevelVariable();
                //string columnName = property.Name;
                sb.AppendLine($"\t\tpublic virtual {simpleType} {propertyName} {{ get {{ return _dto.{propertyName}; }} }}");
            }

            sb.AppendLine(string.Empty);

            //StringBuilder sbPropertyGets = new StringBuilder();
            StringBuilder sbPublicProperties = new StringBuilder();
            StringBuilder sbPrivateProperties = new StringBuilder();
            // Add the ForeignKeys
            //foreach (var foreignKey in table.ForeignKeys)
            var orderedFKs = entity.ForeignKeys.OrderBy(x => x.DependentToPrincipal.Name).ToList();
            foreach (var fk in orderedFKs)
            {
                BuildForwardNavigation(
                    excludedNavigationProperties: excludedNavigationProperties,
                    entity: entity,
                    foreignKey: fk,
                    sbPrivateProperties: ref sbPrivateProperties,
                    sbPublicProperties: ref sbPublicProperties);
            }

            // Add the Reverse Navigation keys
            //foreach (var reverseNavigation in table.ReverseNavigations)
            foreach (var reverseFK in entity.Navigations)
            {
                BuildReverseNavigation(
                    excludedNavigationProperties: excludedNavigationProperties,
                    entity: entity,
                    foreignKey: reverseFK,
                    sbPrivateProperties: ref sbPrivateProperties,
                    sbPublicProperties: ref sbPublicProperties);
            }

            //Append private properties built in BuildForwardNavigation and BuildReverseNavigation methods
            sb.AppendLine(sbPrivateProperties.ToString());
            sb.AppendLine(string.Empty);
            //Append public properties built in BuildForwardNavigation and BuildReverseNavigation methods
            sb.AppendLine(sbPublicProperties.ToString());

            sb.Append(GenerateFooter());

            return sb.ToString();

            // Add the foreign key data for the private get and as the public get
            // Only used by this method
        }

        private void BuildForwardNavigation(IList<IEntityNavigation> excludedNavigationProperties,
            IEntityType entity, IForeignKey foreignKey,
            ref StringBuilder sbPrivateProperties, ref StringBuilder sbPublicProperties)
        {
            string entityName = Inflector.Humanize(entity.ClrType.Name);
            string name = foreignKey.PrincipalEntityType.ClrType.Name;
            string nameHumanized = name;
            if (nameHumanized.Length >= 2)
            {
                nameHumanized = Inflector.Humanize(nameHumanized);
            }

            bool excludeCircularReferenceNavigationIndicator = IsEntityInExcludedReferenceNavigionationProperties(excludedNavigationProperties, name);
            if (excludeCircularReferenceNavigationIndicator)
            {
                sbPrivateProperties.AppendLine($"\t\t// Excluding '{name}' per configuration setting.");
                return;
            }

            //TODO: Not sure if we have the schema name available. If we do, then we want to remove it from the start of the foreign key name below
            string publicPropertyName = name; // foreignKey.RefTableHumanCase.Replace($"{foreignKey.RefTableSchema}_", string.Empty);
            string privateVariableName = $"_{publicPropertyName.Substring(0, 1).ToLower()}{publicPropertyName.Substring(1)}";

            bool useCollection = (foreignKey.PrincipalEntityType.ClrType.Name.Equals("ICollection`1"));
            //bool useCollection = foreignKey.Relationship == Library.Enums.Relationship.ManyToOne;

            string modelTypeName = name; // foreignKey.RefTable;
            string objectType;

            if (excludeCircularReferenceNavigationIndicator)
            {   // This item is configured to be excluded from navigation properties.
                sbPrivateProperties.Append($"\t\t// ");
            }
            else
            {
                sbPrivateProperties.Append($"\t\t");
            }

            if (useCollection)
            {
                //publicPropertyName = foreignKey.RefTableHumanCase.Replace($"{foreignKey.RefTableSchema}_", string.Empty);
                objectType = $"List<I{modelTypeName}>";
                sbPrivateProperties.AppendLine($"private {objectType} {privateVariableName} = null; // Foreign Key");
            }
            else
            {
                objectType = $"I{modelTypeName}";
                sbPrivateProperties.AppendLine($"private {objectType} {privateVariableName} = null; // Foreign Key");
            }

            string dtoName = nameHumanized;
            string initializer = useCollection ? "()" : "";

            if (entityName == publicPropertyName)
            {
                publicPropertyName = $"{entityName}1";
            }

            sbPublicProperties.AppendLine($"\t\tpublic virtual {objectType} {publicPropertyName}");
            sbPublicProperties.AppendLine($"\t\t{{");
            sbPublicProperties.AppendLine($"\t\t\tget");
            sbPublicProperties.AppendLine($"\t\t\t{{");

            if (excludeCircularReferenceNavigationIndicator)
            {
                sbPublicProperties.AppendLine($"\t\t\t\tif ({privateVariableName} == null)");
                sbPublicProperties.AppendLine($"\t\t\t\t{{");
                sbPublicProperties.AppendLine($"\t\t\t\t\tOnLazyLoadRequest(this, new LoadRequest{entityName}(nameof({publicPropertyName})));");
            }
            else
            {
                sbPublicProperties.AppendLine($"\t\t\t\tif ({privateVariableName} == null && _dto != null && _dto.{dtoName} != null)");
                sbPublicProperties.AppendLine($"\t\t\t\t{{");

                if (useCollection)
                {
                    sbPublicProperties.AppendLine($"\t\t\t\t\t{privateVariableName} = new {objectType}();");
                    sbPublicProperties.AppendLine($"\t\t\t\t\tforeach (var dtoItem in _dto.{name})");
                    sbPublicProperties.AppendLine($"\t\t\t\t\t{{");
                    sbPublicProperties.AppendLine($"\t\t\t\t\t\t{privateVariableName}.Add(new {modelTypeName}(Log, DataService, dtoItem));");
                    sbPublicProperties.AppendLine($"\t\t\t\t\t}}");
                }
                else
                {
                    sbPublicProperties.AppendLine($"\t\t\t\t\t{privateVariableName} = new {modelTypeName}(Log, DataService, _dto.{dtoName});");
                }
            }

            sbPublicProperties.AppendLine($"\t\t\t\t}}");
            sbPublicProperties.AppendLine(string.Empty);
            sbPublicProperties.AppendLine($"\t\t\t\treturn {privateVariableName};");
            sbPublicProperties.AppendLine($"\t\t\t}}");
            sbPublicProperties.AppendLine($"\t\t}}");
            sbPublicProperties.AppendLine(string.Empty);
        }

        private void BuildReverseNavigation(IList<IEntityNavigation> excludedNavigationProperties,
            IEntityType entity, INavigation foreignKey,
            ref StringBuilder sbPrivateProperties, ref StringBuilder sbPublicProperties)
        {
            //string tableName = table.GetNameHumanCaseSingular(false);
            string entityName = foreignKey.Name;
            string name = foreignKey.ClrType.Name;
            string nameHumanized = name;
            if (nameHumanized.Length >= 2)
            {
                nameHumanized = Inflector.Humanize(nameHumanized);
            }
            bool useCollection = (name.Equals("ICollection`1"));

            //if (foreignKey.Relationship == Relationship.ManyToOne)
            if (useCollection)
            {
                name = Inflector.Humanize(foreignKey.Name); // foreignKey.RefTableHumanCase;
            }

            //bool excludeCircularReferenceNavigationIndicator = foreignKey.ExcludeCircularReferenceNavigationIndicator(excludedNavigationProperties);
            bool excludeCircularReferenceNavigationIndicator = IsEntityInExcludedReferenceNavigionationProperties(excludedNavigationProperties, name);

            if (excludeCircularReferenceNavigationIndicator)
            {
                sbPrivateProperties.AppendLine($"\t\t// Excluding '{name}' per configuration setting.");
                return;
            }

            //string publicPropertyName = foreignKey.RefTableHumanCase.Replace($"{foreignKey.TableSchema}_", string.Empty);
            string publicPropertyName = name; // foreignKey.RefTableHumanCase.Replace($"{foreignKey.RefTableSchema}_", string.Empty);
            string privateVariableName = $"_{publicPropertyName.Substring(0, 1).ToLower()}{publicPropertyName.Substring(1)}";

            //bool useCollection = foreignKey.Relationship == Library.Enums.Relationship.ManyToOne;
            //string modelTypeName = modelTypeName = foreignKey.FullTableName.Replace($"{foreignKey.TableSchema}_", string.Empty);
            string modelTypeName = foreignKey.ForeignKey.PrincipalEntityType.ClrType.Name;
            string objectType;

            if (excludeCircularReferenceNavigationIndicator)
            {   // This item is configured to be excluded from navigation properties.
                sbPrivateProperties.Append($"\t\t// ");
            }
            else
            {
                sbPrivateProperties.Append($"\t\t");
            }

            if (useCollection)
            {
                objectType = $"List<I{foreignKey.ForeignKey.DeclaringEntityType.ClrType.Name}>";
                sbPrivateProperties.AppendLine($"private {objectType} {privateVariableName} = null; // Reverse Navigation");
            }
            else
            {
                objectType = $"I{modelTypeName}";
                //objectType = foreignKey.FullTableName.Replace($"{foreignKey.TableSchema}_", string.Empty);
                sbPrivateProperties.AppendLine($"private {modelTypeName} {privateVariableName} = null; // Reverse Navigation");
            }

            string dtoName = nameHumanized;
            string initializer = useCollection ? "()" : "";

            if (entityName == publicPropertyName)
            {
                publicPropertyName = $"{entityName}1";
            }

            sbPublicProperties.AppendLine($"\t\tpublic virtual {objectType} {publicPropertyName}");
            sbPublicProperties.AppendLine($"\t\t{{");
            sbPublicProperties.AppendLine($"\t\t\tget");
            sbPublicProperties.AppendLine($"\t\t\t{{");

            if (excludeCircularReferenceNavigationIndicator)
            {
                sbPublicProperties.AppendLine($"\t\t\t\tif ({privateVariableName} == null)");
                sbPublicProperties.AppendLine($"\t\t\t\t{{");
                sbPublicProperties.AppendLine($"\t\t\t\t\tOnLazyLoadRequest(this, new LoadRequest{entityName}(nameof({publicPropertyName})));");
                sbPublicProperties.AppendLine($"\t\t\t\t}}");
            }
            else
            {
                sbPublicProperties.AppendLine($"\t\t\t\tif ({privateVariableName} == null && _dto != null)");
                sbPublicProperties.AppendLine($"\t\t\t\t{{\t// The core DTO object is loaded, but this property is not loaded.");
                sbPublicProperties.AppendLine($"\t\t\t\t\tif (_dto.{dtoName} != null)");
                sbPublicProperties.AppendLine($"\t\t\t\t\t{{\t// The core DTO object has data for this property, load it into the model.");

                if (useCollection)
                {
                    sbPublicProperties.AppendLine($"\t\t\t\t\t\t{privateVariableName} = new {objectType}();");
                    sbPublicProperties.AppendLine($"\t\t\t\t\t\tforeach (var dtoItem in _dto.{name})");
                    sbPublicProperties.AppendLine($"\t\t\t\t\t\t{{");
                    sbPublicProperties.AppendLine($"\t\t\t\t\t\t\t{privateVariableName}.Add(new {modelTypeName}(Log, DataService, dtoItem));");
                    sbPublicProperties.AppendLine($"\t\t\t\t\t\t}}");
                }
                else
                {
                    sbPublicProperties.AppendLine($"\t\t\t\t\t\t{privateVariableName} = new {modelTypeName}(Log, DataService, _dto.{dtoName});");
                }

                sbPublicProperties.AppendLine($"\t\t\t\t\t}}");
                sbPublicProperties.AppendLine($"\t\t\t\t\telse");
                sbPublicProperties.AppendLine($"\t\t\t\t\t{{\t// Trigger the load data request - The core DTO object is loaded and does not have data for this property.");
                sbPublicProperties.AppendLine($"\t\t\t\t\t\tOnLazyLoadRequest(this, new LoadRequest{entityName}(nameof({publicPropertyName})));");
                sbPublicProperties.AppendLine($"\t\t\t\t\t}}");
                sbPublicProperties.AppendLine($"\t\t\t\t}}");
            }

            sbPublicProperties.AppendLine(string.Empty);
            sbPublicProperties.AppendLine($"\t\t\t\treturn {privateVariableName};");
            sbPublicProperties.AppendLine($"\t\t\t}}");
            sbPublicProperties.AppendLine($"\t\t}}");
            sbPublicProperties.AppendLine(string.Empty);
        }

        private string GenerateLoadRequestEventArgs(IEntityType entity)
        {
            StringBuilder sb = new StringBuilder();

            //string tableName = table.GetNameHumanCaseSingular(false);
            string entityName = Inflector.Pascalize(entity.ClrType.Name); // GetNameHumanCaseSingular(prependSchemaNameIndicator);

            sb.AppendLine($"\tpublic class LoadRequest{entityName} : EventArgs");
            sb.AppendLine($"\t{{");
            sb.AppendLine($"\t\tpublic LoadRequest{entityName}(string propertyNameRequestingLoad)");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\tPropertyNameRequestingLoad = propertyNameRequestingLoad;");
            sb.AppendLine($"\t\t}}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tpublic string PropertyNameRequestingLoad {{ get; set; }}");
            sb.AppendLine($"\t}}");

            return sb.ToString();
        }
    }
}