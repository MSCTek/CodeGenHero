using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using System.Linq;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.MVVM
{
    public class MapperSqliteModelDataToMvvmLightModelObjectGenerator : BaseAPIFFGenerator
    {
        public MapperSqliteModelDataToMvvmLightModelObjectGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateHeader(
            string classNamespace, string className,
            string modelObjNamespacePrefix, string modelObjNamespace,
            string modelDataNamespacePrefix, string modelDataNamespace)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Collections.ObjectModel;");
            sb.AppendLine($"using System.Threading.Tasks;");
            sb.Append($"using ");
            if (!string.IsNullOrEmpty(modelObjNamespacePrefix))
            {
                sb.Append($"{modelObjNamespacePrefix} = ");
            }
            sb.AppendLine($"{modelObjNamespace};");

            sb.Append($"using ");
            if (!string.IsNullOrEmpty(modelDataNamespacePrefix))
            {
                sb.Append($"{modelDataNamespacePrefix} = ");
            }
            sb.AppendLine($"{modelDataNamespace};");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {classNamespace}");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic static partial class {className}");
            sb.AppendLine($"\t{{");

            return sb.ToString();
        }

        public string GenerateMapperSqliteModelDataToMvvmLightModelObject(
            string classNamespace,
            string className,
            string modelObjNamespacePrefix,
            string modelObjNamespace,
            string modelDataNamespacePrefix,
            string modelDataNamespace,
            bool prependSchemaNameIndicator,
             IList<IEntityType> entityTypes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(classNamespace, className,
                modelObjNamespacePrefix, modelObjNamespace,
                modelDataNamespacePrefix, modelDataNamespace));

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#region ModelObj to ModelData");
            sb.AppendLine(string.Empty);

            foreach (var entity in entityTypes)
            {
                string entityName = Inflector.Pascalize(entity.ClrType.Name);
                sb.AppendLine($"\t\tpublic static {modelDataNamespacePrefix}.{entityName} ToModelData(this {modelObjNamespacePrefix}.{entityName} source)");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\treturn new {modelDataNamespacePrefix}.{entityName}()");
                sb.AppendLine($"\t\t\t{{");

                var entityProperties = entity.GetProperties().OrderBy(n => n.Name);

                foreach (var property in entityProperties)
                {
                    string propertyName = Inflector.Pascalize(property.Name);
                    if (!IsUnknownType(property))
                    {
                        sb.AppendLine($"\t\t\t\t{propertyName} = source.{propertyName},");
                    }
                }

                sb.AppendLine($"\t\t\t}};");
                sb.AppendLine($"\t\t}}");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#endregion ModelObj to ModelData");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#region ModelData to ModelObj");
            sb.AppendLine(string.Empty);

            foreach (var entity in entityTypes)
            {
                string entityName = Inflector.Pascalize(entity.ClrType.Name);
                var entityProperties = entity.GetProperties().OrderBy(n => n.Name);
                sb.AppendLine($"\t\tpublic static {modelObjNamespacePrefix}.{entityName} ToModelObj(this {modelDataNamespacePrefix}.{entityName} source)");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\treturn new {modelObjNamespacePrefix}.{entityName}()");
                sb.AppendLine($"\t\t\t{{");

                foreach (var property in entityProperties)
                {
                    string propertyName = Inflector.Pascalize(property.Name);
                    if (!IsUnknownType(property))
                    {
                        sb.AppendLine($"\t\t\t\t{propertyName} = source.{propertyName},");
                    }
                }

                sb.AppendLine($"\t\t\t}};");
                sb.AppendLine($"\t\t}}");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t#endregion ModelData to ModelObj");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateFooter());

            return sb.ToString();
        }
    }
}