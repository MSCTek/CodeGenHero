using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template.CSLA.Generators
{
    public class CSLAEditBusinessGenerator : BaseCSLAGenerator
    {
        public CSLAEditBusinessGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        internal string GenerateActions(string baseNamespace, string namespacePostfix, string classNamespace,
                    IList<IEntityNavigation> excludeCircularReferenceNavigationProperties, IEntityType entity,
                    bool InludeRelatedObjects, bool prependSchemaNameIndicator)
        {
            var sb = new StringBuilder();
            string entityName = Inflector.Humanize(entity.ClrType.Name);

            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Text;");
            sb.AppendLine($"using Csla; //https://github.com/MarimerLLC/csla");
            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"using {baseNamespace}.DataAccess;" : $"using {baseNamespace}.DataAccess.{namespacePostfix};");
            sb.AppendLine($"");
            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"namespace {baseNamespace}.Shared" : $"namespace {baseNamespace}.Shared.{namespacePostfix}");

            sb.AppendLine($"{{");
            sb.AppendLine("\t[Serializable]");
            sb.AppendLine($"\tpublic class {entityName}Edit : BusinessBase<{entityName}Edit>");
            sb.AppendLine($"\t{{");

            var primaryKeyList = GetPrimaryKeys(entity);
            var entityProperties = entity.GetProperties();

            //////////////////Properties
            foreach (var property in entityProperties)
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;

                sb.AppendLine($"\t\tpublic static readonly PropertyInfo<{simpleType}> {propertyName}Property = RegisterProperty<{simpleType}>(nameof({propertyName}));");

                sb.AppendLine($"\t\tpublic {simpleType} {propertyName}");
                sb.AppendLine("\t\t{");
                sb.AppendLine($"\t\t\tget {{return GetProperty({propertyName}Property);}}");
                sb.AppendLine($"\t\t\tprivate set {{ LoadProperty({propertyName}Property, value);}}");
                sb.AppendLine("\t\t}");
                sb.AppendLine(string.Empty);
            }

            //////////////////AddBusinessRules
            sb.AppendLine("\t\tprotected override void AddBusinessRules()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tbase.AddBusinessRules();");
            sb.AppendLine("\t\t}");

            //////////////////Create
            var k = entity.FindPrimaryKey();
            var kctype = k.Properties[0].ClrType;
            var ksimpleType = ConvertToSimpleType(kctype.Name);
            sb.AppendLine("");
            sb.AppendLine("\t\t[Create]");
            sb.AppendLine("\t\tprivate void Create()");
            sb.AppendLine("\t\t{");

            sb.AppendLine($"\t\t\t{k.Properties[0].Name} = -1;");
            sb.AppendLine("\t\t\tbase.DataPortal_Create();");
            sb.AppendLine("\t\t}");

            //////////////////Fetch
            sb.AppendLine("");
            sb.AppendLine("\t\t[Fetch]");
            sb.AppendLine($"\t\tprivate void Fetch({ksimpleType} {k.Properties[0].Name}, [Inject]I{entityName}Dal dal)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tvar data = dal.Get({k.Properties[0].Name});");
            sb.AppendLine("\t\t\tusing (BypassPropertyChecks)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tCsla.Data.DataMapper.Map(data, this);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tBusinessRules.CheckRules();");
            sb.AppendLine("\t\t}");

            //////////////////Insert
            sb.AppendLine("");
            sb.AppendLine("\t\t[Insert]");
            sb.AppendLine($"\t\tprivate void Insert([Inject]I{entityName}Dal dal)");
            sb.AppendLine("\t\t{");

            sb.AppendLine("\t\t\tusing (BypassPropertyChecks)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tvar data = new PersonEntity");
            sb.AppendLine("\t\t\t\t{");
            foreach (var property in entityProperties)
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;

                if (propertyName != k.Properties[0].Name) //don't assign the key inside this, it is assigned to the return of insert data below
                {
                    sb.AppendLine($"\t\t\t\t\t{propertyName} = {propertyName},");
                }
            }
            sb.AppendLine("\t\t\t\t};");
            sb.AppendLine("\t\t\tvar result = dal.Insert(data);");
            sb.AppendLine($"\t\t\t{k.Properties[0].Name} = result.{k.Properties[0].Name};");

            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            //////////////////Update
            sb.AppendLine("");
            sb.AppendLine("\t\t[Update]");
            sb.AppendLine($"\t\tprivate void Update([Inject]I{entityName}Dal dal)");
            sb.AppendLine("\t\t{");

            sb.AppendLine("\t\t\tusing (BypassPropertyChecks)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tvar data = new PersonEntity");
            sb.AppendLine("\t\t\t\t{");
            foreach (var property in entityProperties)
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;

                if (propertyName != k.Properties[0].Name) //don't assign the key inside this, it is assigned to the return of insert data below
                {
                    sb.AppendLine($"\t\t\t\t\t{propertyName} = {propertyName},");
                }
            }
            sb.AppendLine("\t\t\t\t};");
            sb.AppendLine("\t\t\tvar result = dal.Update(data);");
            sb.AppendLine($"\t\t\t{k.Properties[0].Name} = result.{k.Properties[0].Name};");

            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            //////////////////Delete Self
            sb.AppendLine("");
            sb.AppendLine("\t\t[DeleteSelf]");
            sb.AppendLine($"\t\tprivate void DeleteSelf([Inject]I{entityName}Dal dal)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tDelete(ReadProperty({k.Properties[0].Name}Property), dal);");

            sb.AppendLine("\t\t}");

            //////////////////Delete
            sb.AppendLine("");
            sb.AppendLine("\t\t[Delete]");
            sb.AppendLine($"\t\tprivate void Delete({ksimpleType} {k.Properties[0].Name}, [Inject]I{entityName}Dal dal)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tdal.Delete({k.Properties[0].Name});");
            sb.AppendLine("\t\t}");
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");
            return sb.ToString();
        }
    }
}