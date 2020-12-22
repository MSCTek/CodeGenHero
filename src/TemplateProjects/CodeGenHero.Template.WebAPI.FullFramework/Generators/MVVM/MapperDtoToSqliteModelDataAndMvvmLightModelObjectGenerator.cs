using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using System.Linq;
using System;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.MVVM
{
    public class MapperDtoToSqliteModelDataAndMvvmLightModelObjectGenerator : BaseAPIFFGenerator
    {
        public MapperDtoToSqliteModelDataAndMvvmLightModelObjectGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateMapperDtoTosqliteModelDataAndMvvmLightModelObject(
            List<string> usings,
            string classNamespace,
            string className,
            string modelObjNamespacePrefix,
            string modelDataNamespacePrefix,
            string modelDtoNamespacePrefix,
            bool prependSchemaNameIndicator,
             IList<IEntityType> entityTypes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateUsings(usings));

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {classNamespace}");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic static partial class {className}");
            sb.AppendLine($"\t{{");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#region ModelObj to ModelDto");
            sb.AppendLine(string.Empty);

            string generatedCode = GenerateModelMapperExtensionMethods(entityTypes, "ToDto", modelDtoNamespacePrefix, modelObjNamespacePrefix, prependSchemaNameIndicator);
            sb.Append(generatedCode);
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#endregion ModelObj to ModelDto");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#region ModelData to ModelDto");
            sb.AppendLine(string.Empty);

            generatedCode = GenerateModelMapperExtensionMethods(entityTypes,
                methodName: "ToDto",
                returnNamespacePrefix: modelDtoNamespacePrefix,
                fromNamespacePrefix: modelDataNamespacePrefix,
                prependSchemaNameIndicator: prependSchemaNameIndicator);
            sb.Append(generatedCode);

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#endregion ModelData to ModelDto");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#region ModelDto to ModelObj");
            sb.AppendLine(string.Empty);

            generatedCode = GenerateModelMapperExtensionMethods(entityTypes,
                methodName: "ToModelObj",
                returnNamespacePrefix: modelObjNamespacePrefix,
                fromNamespacePrefix: modelDtoNamespacePrefix,
                prependSchemaNameIndicator: prependSchemaNameIndicator);
            sb.Append(generatedCode);

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#endregion ModelDto to ModelObj");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#region ModelDto to ModelData");
            sb.AppendLine(string.Empty);

            generatedCode = GenerateModelMapperExtensionMethods(entityTypes: entityTypes,
                methodName: "ToModelData",
                returnNamespacePrefix: modelDataNamespacePrefix,
                fromNamespacePrefix: modelDtoNamespacePrefix,
                prependSchemaNameIndicator: prependSchemaNameIndicator);
            sb.Append(generatedCode);

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#endregion ModelDto to ModelData");

            sb.Append(GenerateFooter());

            return sb.ToString();
        }

        private string GenerateModelMapperExtensionMethods(IList<IEntityType> entityTypes,
            string methodName, string returnNamespacePrefix, string fromNamespacePrefix, bool prependSchemaNameIndicator)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entity in entityTypes)
            {
                var k = entity.FindPrimaryKey();
                string entityName = Inflector.Pascalize(entity.ClrType.Name);
                var entityProperties = entity.GetProperties().OrderBy(n => n.Name).ToList();
                bool hasMultiplePrimaryKeys = entity.FindPrimaryKey().Properties.Count > 1;
                string compositePKFieldName = string.Empty;
                string compositePKFieldValue = string.Empty;
                sb.AppendLine($"\t\tpublic static {returnNamespacePrefix}.{entityName} {methodName}(this {fromNamespacePrefix}.{entityName} source)");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\treturn new {returnNamespacePrefix}.{entityName}()");
                sb.AppendLine($"\t\t\t{{");
                var primaryKey = entity.FindPrimaryKey();
                for (int i = 0; i < entityProperties.Count(); i++)
                {
                    var property = entityProperties[i];
                    string propertyName = Inflector.Pascalize(property.Name);

                    string ctype = GetCType(property);
                    var simpleType = ConvertToSimpleType(ctype);

                    if (!IsUnknownType(property))
                    {
                        sb.AppendLine($"\t\t\t\t{propertyName} = source.{propertyName},");
                    }

                    if (primaryKey.Properties.Where(x => x.Name == propertyName).Any())
                    {
                        if (hasMultiplePrimaryKeys)
                        {
                            compositePKFieldName += propertyName;
                            compositePKFieldValue += $"{{source.{propertyName}}}";
                        }
                    }
                }

                // Create an extra line to handle a limitation in SQLite when dealing with tables that use composite primary keys
                //   i.e. VehicleIdVehicleFeatureTypeId = $"{source.VehicleId}{source.VehicleFeatureTypeId}"
                if (hasMultiplePrimaryKeys && methodName.ToLowerInvariant() == "tomodeldata")
                {
                    sb.AppendLine($"{Environment.NewLine}\t\t\t\t// Create an extra line to handle a limitation in SQLite when dealing with tables that use composite primary keys");
                    sb.AppendLine($"\t\t\t\t{compositePKFieldName} = $\"{compositePKFieldValue}\"");
                }

                sb.AppendLine($"\t\t\t}};");
                sb.AppendLine($"\t\t}}");
                sb.AppendLine(string.Empty);
            }
            return sb.ToString();
        }
    }
}