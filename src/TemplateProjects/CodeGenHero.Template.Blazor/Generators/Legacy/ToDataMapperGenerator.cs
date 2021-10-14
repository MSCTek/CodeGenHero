using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.Blazor.Generators
{
    public class ToDataMapperGenerator : BaseBlazorGenerator
    {
        public ToDataMapperGenerator(ICodeGenHeroInflector inflector) : base(inflector)
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
            sb.AppendLine($"\tpublic static class ToDataMappers");
            sb.AppendLine($"\t{{");

            foreach (var entity in entityTypes)
            {
                var k = entity.FindPrimaryKey();
                string entityName = Inflector.Pascalize(entity.ClrType.Name);
                var entityProperties = entity.GetProperties().OrderBy(n => n.Name).ToList();
                bool hasMultiplePrimaryKeys = entity.FindPrimaryKey().Properties.Count > 1;
                string compositePKFieldName = string.Empty;
                string compositePKFieldValue = string.Empty;

                sb.AppendLine($"\t\tpublic static xData.{entityName} ToData(this xModel.{entityName} obj)");
                sb.AppendLine($"\t\t{{"); //Beginning of main method bracket

                //Null Check
                sb.AppendLine($"\t\t\tif (obj == null)");
                sb.AppendLine("\t\t\treturn null;");

                //Declaration of return mapped value
                sb.AppendLine($"\t\t\tvar retVal = new xData.{entityName}()");
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
                        sb.AppendLine($"\t\t\t\t{reverseFK.ClrType.Name} = obj.{reverseFK.ClrType.Name}.ToData(),");
                    }
                    else
                    {
                        sb.AppendLine($"\t\t\t\t{Inflector.Pluralize(humanCase)} = obj.{Inflector.Pluralize(humanCase)}.ToData<xData.{humanCase}, xModel.{humanCase}>(),");
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

            sb.AppendLine($"public static IList<D> ToData<M, D>(this ICollection<M> col)");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tif (col == null)");
            sb.AppendLine($"\t\t return null;");

            sb.AppendLine($"\tType returnTypeToFind = typeof(D);");
            sb.AppendLine($"\tMethodInfo methodInfoToUse = typeof(ToDataMappers).GetMethods()");
            sb.AppendLine($"\t\t.Where(mi => mi.Name == \"ToData\" && mi.ContainsGenericParameters == false && mi.ReturnType == returnTypeToFind).FirstOrDefault();");
            sb.AppendLine($"\tif (methodInfoToUse == null)");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\tthrow new ApplicationException($\"Unable to locate the appropriate method to use in {{nameof(ToData)}} for type {{returnTypeToFind}}\");");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine($"\tIList<D> retVal = new List<D>();");
            sb.AppendLine($"\tforeach (var item in col)");
            sb.AppendLine($"{{");

            sb.AppendLine($"\t\t object result = methodInfoToUse.Invoke(null, new object[] {{ item}});");

            sb.AppendLine($"\t\t\tretVal.Add((D)result);");

            sb.AppendLine($"}}");

            sb.AppendLine($"\treturn retVal;");
            sb.AppendLine($"}}");

            sb.AppendLine($"public static List<D> ToData<M, D>(this List<M> col)");
            sb.AppendLine($"{{");

            sb.AppendLine($"\treturn (List<D>)ToData<M, D>((IList<M>)col);");
            sb.AppendLine($"}}");

            sb.AppendLine($"#endregion Collections");
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");
        }
    }
}