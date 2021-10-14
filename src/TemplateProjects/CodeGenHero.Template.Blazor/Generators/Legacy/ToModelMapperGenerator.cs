using CodeGenHero.Inflector;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.Blazor.Generators
{
    public class ToModelMapperGenerator : BaseBlazorGenerator
    {
        public ToModelMapperGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<string> usings,
            string classNamespace,
            bool prependSchemaNameIndicator,
           IList<IEntityType> entityTypes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateUsings(usings));

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {classNamespace}");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic static class ToModelMappers");
            sb.AppendLine($"\t{{");

            foreach (var entity in entityTypes)
            {
                var k = entity.FindPrimaryKey();
                string entityName = Inflector.Pascalize(entity.ClrType.Name);
                var entityProperties = entity.GetProperties().OrderBy(n => n.Name).ToList();
                bool hasMultiplePrimaryKeys = entity.FindPrimaryKey().Properties.Count > 1;
                string compositePKFieldName = string.Empty;
                string compositePKFieldValue = string.Empty;

                sb.AppendLine($"\t\tpublic static xModel.{entityName} ToModel(this xData.{entityName} obj, cEnums.RecordStatus recordStatus, bool removeSelfReferencingChildEntities = true)");
                sb.AppendLine($"\t\t{{"); //Beginning of main method bracket

                //Null Check
                sb.AppendLine($"\t\t\tif (obj == null || (obj.StatusTypeId & (int)recordStatus) != obj.StatusTypeId)");
                sb.AppendLine("\t\t\treturn null;");

                //Remove self referencing child entities
                sb.AppendLine($"\t\t\tif(removeSelfReferencingChildEntities)");
                sb.AppendLine($"\t\t\t{{  // Avoid circular references from causing infinite loops during serialization.");
                sb.AppendLine($"\t\t\t\t\tvar crr = new Service.CircularReferenceRemover<xData.{entityName}>();");
                sb.AppendLine($"\t\t\t\t\tcrr.RemoveSelfReferencingChildEntities(obj);");
                sb.AppendLine($"\t\t\t}}");

                //Declaration of return mapped value
                sb.AppendLine($"\t\t\tvar retVal = new xModel.{entityName}()");
                sb.AppendLine($"\t\t\t{{");

                //Loop through properties
                for (int i = 0; i < entityProperties.Count(); i++)
                {
                    var property = entityProperties[i];
                    string propertyName = Inflector.Pascalize(property.Name);

                    string ctype = GetCType(property);
                    var simpleType = ConvertToSimpleType(ctype);

                    if (simpleType == "DateTime")
                    {
                        sb.AppendLine($"\t\t\t\t{propertyName} = DateTime.SpecifyKind(obj.{propertyName}, DateTimeKind.Utc),");
                    }
                    else
                    {
                        sb.AppendLine($"\t\t\t\t{propertyName} = obj.{propertyName},");
                    }
                }

                //Loop through navigations
                foreach (var reverseFK in entity.Navigations)
                {
                    string name = reverseFK.DeclaringType.ClrType.Name;
                    string humanCase = Inflector.Humanize(name);
                    string reverseFKName = reverseFK.ForeignKey.PrincipalEntityType.Name;
                    if (!reverseFK.ClrType.Name.Equals("ICollection`1"))
                    {
                        sb.AppendLine($"\t\t\t\t{reverseFK.ClrType.Name} = obj.{reverseFK.ClrType.Name}.ToModel(recordStatus),");
                    }
                    else
                    {
                        sb.AppendLine($"\t\t\t\t{Inflector.Pluralize(humanCase)} = obj.{Inflector.Pluralize(humanCase)}.ToModel<xData.{humanCase}, xModel.{humanCase}>(recordStatus),");
                    }
                }

                sb.AppendLine($"\t\t}};");
                sb.AppendLine("\t\treturn retVal;");
                sb.AppendLine($"}}");

                sb.AppendLine(string.Empty);
            }
            GenerateFooter(sb);
            return sb.ToString();
        }

        private void GenerateFooter(StringBuilder sb)
        {
            sb.AppendLine($"#region Collections");

            sb.AppendLine($"public static IList<M> ToModel<D, M>(this ICollection<D> col, cEnums.RecordStatus recordStatus)");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tif (col == null)");
            sb.AppendLine($"\t\t return null;");

            sb.AppendLine($"\tType returnTypeToFind = typeof(M);");
            sb.AppendLine($"\tMethodInfo methodInfoToUse = typeof(ToModelMappers).GetMethods()");
            sb.AppendLine($"\t\t.Where(mi => mi.Name == \"ToModel\" && mi.ContainsGenericParameters == false && mi.ReturnType == returnTypeToFind).FirstOrDefault();");
            sb.AppendLine($"\tif (methodInfoToUse == null)");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\tthrow new ApplicationException($\"Unable to locate the appropriate method to use in {{nameof(ToModel)}} for type {{returnTypeToFind}}\");");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine($"\tIList<M> retVal = new List<M>();");
            sb.AppendLine($"\tforeach (var item in col)");
            sb.AppendLine($"{{");

            sb.AppendLine($"\t\t// The following can be used for testing to ensure all collection properties in a data class have corresponding ToModel mappers defined:");
            sb.AppendLine($"\t\t//Survey survey = item as DataAccess.Entities.EP.Survey;");
            sb.AppendLine($"\t\t//if (survey != null)");
            sb.AppendLine($"\t\t//{{");
            sb.AppendLine($"\t\t\t//  object test = survey.ToModel();");
            sb.AppendLine($"\t\t //}}");

            sb.AppendLine($"\t\t// If you get an error message of \"Parameter count mismatch.\" -- This is your likely culprit!");
            sb.AppendLine($"\t\t object result = methodInfoToUse.Invoke(null, new object[] {{ item, recordStatus, true }});");
            sb.AppendLine($"\t\t\tif (result != null)");
            sb.AppendLine($"\t\t{{");

            sb.AppendLine($"\t\t\tretVal.Add((M)result);");
            sb.AppendLine($"\t\t}}");
            sb.AppendLine($"}}");

            sb.AppendLine($"\tretVal = retVal.Where(x => x != null).ToList();");
            sb.AppendLine($"\treturn retVal;");
            sb.AppendLine($"}}");

            sb.AppendLine($"public static List<M> ToModel<D, M>(this List<D> col, cEnums.RecordStatus recordStatus)");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tIList<D> colAsIList = (IList<D>)col;");
            sb.AppendLine($"\treturn (List<M>)ToModel<D, M>((IList<D>)colAsIList, recordStatus);");
            sb.AppendLine($"}}");

            sb.AppendLine($"#endregion Collections");
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");
        }
    }
}