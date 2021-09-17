using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;
using System.Linq;

namespace CodeGenHero.Template.Blazor.Generators
{
    class GenericFactoryGenerator : BaseBlazorGenerator
    {
        public GenericFactoryGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix)
        {
            var className = $"{namespacePostfix}GenericFactory";

            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"public class {className}<TEntity, TDto> : I{className}<TEntity, TDto>");
            sb.AppendLine("{");
            sb.AppendLine("private IMapper _mapper;");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateConstructor(className));
            sb.Append(GenerateImplementations());

            sb.Append(GenerateFooter());
            return sb.ToString();
        }

        private string GenerateConstructor(string className)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}(IMapper mapper)");
            sb.AppendLine("{");

            sb.AppendLine("\t_mapper = mapper;");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateImplementations()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("public TDto Create(TEntity item)");
            sb.AppendLine("{");
            sb.AppendLine("\treturn _mapper.Map<TDto>(item);");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("public TEntity Create(TDto item)");
            sb.AppendLine("{");
            sb.AppendLine("\treturn _mapper.Map<TEntity>(item);");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateCreateDataShapedObject());

            return sb.ToString();
        }

        private string GenerateCreateDataShapedObject()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("public object CreateDataShapedObject(TEntity item, List<string> lstOfFields, bool childrenRequested)");
            sb.AppendLine("{");
            sb.AppendLine("\treturn CreateDataShapedObject(Create(item), lstOfFields, childrenRequested);");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("public virtual object CreateDataShapedObject(object item, List<string> fieldList, bool childrenRequested)");
            sb.AppendLine("{");
            
            sb.AppendLine("\tif (!fieldList.Any() || childrenRequested)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn item;");
            sb.AppendLine("\t}");

            sb.AppendLine("\telse");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tExpandoObject objectToReturn = new ExpandoObject();");
            sb.AppendLine("\t\tvar itemType = item.GetType();");
            sb.AppendLine("\t\tforeach (var field in fieldList)");
            sb.AppendLine("\t\t{");

            sb.AppendLine("\t\t\tvar fieldValue = itemType");
            sb.AppendLine("\t\t\t\t.GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)");
            sb.AppendLine("\t\t\t\t.GetValue(item, null);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t\t((IDictionary<String, Object>)objectToReturn).Add(field, fieldValue);");

            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("return objectToReturn;");
            sb.AppendLine("\t}");

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
