using CodeGenHero.Core.Metadata.Interfaces;
using System.Text;
using CodeGenHero.Inflector;

namespace CodeGenHero.Template.CSLA.Generators
{
    public class DataAccessEFMockGenerator : BaseCSLAGenerator
    {
        public DataAccessEFMockGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateDataAccessMock(
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
            //  sb.AppendLine($"\t\tprivate readonly {dbContextName} _context;");
            //sb.AppendLine("");

            sb.AppendLine($"\t\tpublic static readonly List<{entityName}Entity> _{entityNameCamelized}Table = new List<{entityName}Entity>");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\tnew {entityName}Entity {{ Id = 1, Name = \"Andy\"}},");
            sb.AppendLine($"\t\tnew {entityName}Entity {{ Id = 3, Name = \"Buzz\"}}");
            sb.AppendLine("\t\t};");

            /*
               private static readonly List<PersonEntity> _personTable = new List<PersonEntity>
    {
      new PersonEntity { Id = 1, Name = "Andy"},
      new PersonEntity { Id = 3, Name = "Buzz"}
    };
             */

            sb.AppendLine(string.Empty);

            string whereClause = BuildWhereClause("p", entity);

            // DELETE
            sb.AppendLine($"\t\tpublic bool Delete({signature})"); //TODO: We probably need to get the primary keys and and calculate the signature
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tvar {entityNameCamelized} = _{entityNameCamelized}Table.FirstOrDefault(p => {whereClause});");
            sb.AppendLine($"\t\t\tif ({entityNameCamelized} != null)");
            sb.AppendLine($"\t\t\t{{");
            sb.AppendLine($"\t\t\t\tlock (_{entityNameCamelized}Table)");
            sb.AppendLine($"\t\t\t\t_{entityNameCamelized}Table.Remove({entityNameCamelized});");
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
            sb.AppendLine($"\t\t\tvar {entityNameCamelized} = _{entityNameCamelized}Table.FirstOrDefault(p => {whereClause});");
            sb.AppendLine($"\t\t\treturn !({entityNameCamelized} == null);");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine(string.Empty);

            // GET
            sb.AppendLine($"\t\tpublic {entityName}Entity Get({signature})");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tvar {entityNameCamelized} = _{entityNameCamelized}Table.FirstOrDefault(p => {whereClause});");
            sb.AppendLine($"\t\t\tif ({entityNameCamelized} != null)");
            sb.AppendLine($"\t\t\t\treturn {entityNameCamelized};");
            sb.AppendLine("\t\t\telse");
            sb.AppendLine($"\t\t\t\tthrow new KeyNotFoundException($\"Id {{id}}\");");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine(string.Empty);

            // GET (all)
            sb.AppendLine($"\t\tpublic List<{entityName}Entity> Get()");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\treturn _{entityNameCamelized}Table.Where(r => true).ToList();");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine(string.Empty);

            // INSERT
            sb.AppendLine($"\t\tpublic {entityName}Entity Insert({entityName}Entity {entityNameCamelized})");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tif (Exists({entityNameCamelized}.Id))");
            sb.AppendLine($"\t\t\t\tthrow new InvalidOperationException($\"Key exists {{{entityNameCamelized}.Id}}\");");
            sb.AppendLine($"\t\t\tlock (_{entityNameCamelized}Table)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tint lastId = _{entityNameCamelized}Table.Max(m => m.Id);");
            sb.AppendLine($"\t\t\t\t{entityNameCamelized}.Id = ++lastId;"); // Not sure how we want to handle this if the key is an Identity field, or not called Id, or using several primary keys
            sb.AppendLine($"\t\t\t\t_{entityNameCamelized}Table.Add({entityNameCamelized});");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine($"\t\t\treturn {entityNameCamelized};");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine(string.Empty);

            // UPDATE
            var properties = GetPropertiesExcludingKeys(entity);

            sb.AppendLine($"\t\tpublic {entityName}Entity Update({entityName}Entity {entityNameCamelized})");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tlock (_{entityNameCamelized}Table)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tvar old = Get({entityNameCamelized}.Id);");
            foreach (var propertyName in properties)
            {
                sb.AppendLine($"\t\t\t\told.{propertyName} = {entityNameCamelized}.{propertyName};");
            }
            sb.AppendLine($"\t\t\t\treturn old;");
            sb.AppendLine($"\t\t\t}}");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine("\t}"); // Close off the class
            sb.AppendLine("}"); // Close off the namespace

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