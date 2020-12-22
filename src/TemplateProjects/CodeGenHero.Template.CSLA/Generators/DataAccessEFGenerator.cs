using CodeGenHero.Core.Metadata.Interfaces;
using System.Text;
using CodeGenHero.Inflector;

namespace CodeGenHero.Template.CSLA.Generators
{
    public class DataAccessEFGenerator : BaseCSLAGenerator
    {
        public DataAccessEFGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateDataAccessEF(
            IEntityType entity,
            string useNamespace,
            string dbContextName)
        {
            var sb = new StringBuilder();
            string entityName = Inflector.Humanize(entity.ClrType.Name);
            string entityNamePluralized = Inflector.Pluralize(entityName);
            string entityNameCamelized = Inflector.Camelize(entityName);

            string signature = GetSignature(entity);

            sb.AppendLine(GetUsings());

            sb.AppendLine($"namespace {useNamespace}");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic class {entityName}EFDal: I{entityName}Dal");
            sb.AppendLine($"\t{{");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tprivate readonly {dbContextName} _context;");
            sb.AppendLine("");

            sb.AppendLine($"\t\tpublic {entityName}EFDal({dbContextName} context)");
            sb.AppendLine("\t\t{");

            sb.AppendLine("\t\t\t_context = context;");
            sb.AppendLine("\t\t}");

            sb.AppendLine(string.Empty);

            string whereClause = BuildWhereClause("p", entity);

            // DELETE
            sb.AppendLine($"\t\tpublic bool Delete({signature})"); //TODO: We probably need to get the primary keys and and calculate the signature
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tvar {entityNameCamelized} = _context.{entityNamePluralized}.FirstOrDefault(p => {whereClause});");
            sb.AppendLine($"\t\t\tif ({entityNameCamelized} != null)");
            sb.AppendLine($"\t\t\t{{");
            sb.AppendLine($"\t\t\t\t_context.{entityNamePluralized}.Remove({entityNameCamelized});");
            sb.AppendLine($"\t\t\t\t_context.SaveChanges();");
            sb.AppendLine($"\t\t\t\treturn true;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\telse");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\treturn false;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine(string.Empty);

            // EXISTS
            sb.AppendLine($"\t\tpublic bool Exists({signature})");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tvar {entityNameCamelized} = _context.{entityNamePluralized}.FirstOrDefault(p => {whereClause});");
            sb.AppendLine($"\t\t\treturn !({entityNameCamelized} == null);");
            sb.AppendLine($"\t\t}}");
            sb.AppendLine(string.Empty);

            sb.AppendLine(string.Empty);

            // GET
            sb.AppendLine($"\t\tpublic {entityName}Entity Get({signature})");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tvar {entityNameCamelized} = _context.{entityNamePluralized}.FirstOrDefault(p => {whereClause});");
            sb.AppendLine($"\t\t\tif ({entityNameCamelized} != null)");
            sb.AppendLine($"\t\t\t{{");
            sb.AppendLine($"\t\t\t\treturn {entityNameCamelized};");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\telse");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tthrow new KeyNotFoundException($\"Id {{id}}\");");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine(string.Empty);

            // GET (all)
            sb.AppendLine($"\t\tpublic List<{entityName}Entity> Get()");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\treturn _context.{entityNamePluralized}.Where(r => true).ToList();");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine(string.Empty);

            // INSERT
            sb.AppendLine($"\t\tpublic {entityName}Entity Insert({entityName}Entity {entityNameCamelized})");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tif (Exists({entityNameCamelized}.Id))");
            sb.AppendLine($"\t\t\t{{");
            sb.AppendLine($"\t\t\t\tthrow new InvalidOperationException($\"Key exists {{ {entityNameCamelized}.Id}}\");");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\telse");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\tint lastId = 0;");
            sb.AppendLine($"\t\t\t\ttry");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\tlastId = _context.Person.Max(m => m.Id);");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t\tcatch(Exception ex)");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine(string.Empty);
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine($"\t\t\t\t{entityNameCamelized}.Id = ++lastId;"); // Not sure how we want to handle this if the key is an Identity field, or not called Id, or using several primary keys
            sb.AppendLine($"\t\t\t\t_context.{entityNamePluralized}.Add({entityNameCamelized});");
            sb.AppendLine("\t\t\t\t_context.SaveChanges();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine($"\t\t\treturn {entityNameCamelized};");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine(string.Empty);

            // UPDATE
            var properties = GetPropertiesExcludingKeys(entity);

            sb.AppendLine($"\t\tpublic {entityName}Entity Update({entityName}Entity {entityNameCamelized})");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tvar old = Get({entityNameCamelized}.Id);");
            foreach (var propertyName in properties)
            {
                sb.AppendLine($"\t\t\told.{propertyName} = {entityNameCamelized}.{propertyName};");
            }

            sb.AppendLine("\t\t\t_context.SaveChanges();");
            sb.AppendLine($"\t\t\treturn old;");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine("\t}");
            sb.AppendLine("}}");

            return sb.ToString();
        }

        private string GetUsings()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");

            return sb.ToString();
        }
    }
}