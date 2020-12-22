using System.Text;
using System.Collections.Generic;
using System;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Server
{
    public class RepositoryBaseGenerator : BaseAPIFFGenerator
    {
        public RepositoryBaseGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateDeleteOperations(string entityName, string tableNamePlural,
            string whereClause, string whereClauseWithObjectInstancePrefix, string methodParameterSignature)
        {
            StringBuilder sb = new StringBuilder();

            // Delete by PK signature
            sb.AppendLine($"\t\t\tpublic async Task<IRepositoryActionResult<{entityName}>> Delete_{entityName}Async({methodParameterSignature})");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\treturn await DeleteAsync<{entityName}>(DbContext.{tableNamePlural}.Where({whereClause}).FirstOrDefault());");
            sb.AppendLine("\t\t\t}");

            // Delete by object
            sb.AppendLine($"\t\t\tpublic async Task<IRepositoryActionResult<{entityName}>> DeleteAsync({entityName} item)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\treturn await DeleteAsync<{entityName}>(DbContext.{tableNamePlural}.Where({whereClauseWithObjectInstancePrefix}).FirstOrDefault());");
            sb.AppendLine("\t\t\t}");

            return sb.ToString();
        }

        public string GenerateGenericOperations()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"\t\t#region Generic Operations");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tprivate async Task<IRepositoryActionResult<TEntity>> DeleteAsync<TEntity>(TEntity item) where TEntity : class");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\ttry");
            sb.AppendLine($"\t\t\t{{");
            sb.AppendLine($"\t\t\t\tif (item == null)");
            sb.AppendLine($"\t\t\t\t{{");
            sb.AppendLine($"\t\t\t\t\treturn new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.NotFound);");
            sb.AppendLine($"\t\t\t\t}}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t\tDbSet<TEntity> itemSet = DbContext.Set<TEntity>();");
            sb.AppendLine($"\t\t\t\titemSet.Remove(item);");
            sb.AppendLine($"\t\t\t\tawait DbContext.SaveChangesAsync();");
            sb.AppendLine($"\t\t\t\treturn new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Deleted);");
            sb.AppendLine($"\t\t\t}}");
            sb.AppendLine($"\t\t\tcatch(Exception ex)");
            sb.AppendLine($"\t\t\t{{");
            sb.AppendLine($"\t\t\t\treturn new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);");
            sb.AppendLine($"\t\t\t}}");
            sb.AppendLine($"\t\t}}");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tpublic IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\treturn DbContext.Set<TEntity>();");
            sb.AppendLine($"\t\t}}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tpublic async Task<IRepositoryActionResult<TEntity>> InsertAsync<TEntity>(TEntity item) where TEntity : class");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\ttry");
            sb.AppendLine($"\t\t\t{{");
            sb.AppendLine($"\t\t\t\tDbSet<TEntity> itemSet = DbContext.Set<TEntity>();");
            sb.AppendLine($"\t\t\t\titemSet.Add(item);");
            sb.AppendLine($"\t\t\t\tvar result = await DbContext.SaveChangesAsync();");
            sb.AppendLine($"\t\t\t\tRunCustomLogicAfterEveryInsert<TEntity>(item, result);{Environment.NewLine}");

            sb.AppendLine($"\t\t\t\tif (result > 0)");
            sb.AppendLine($"\t\t\t\t{{");
            sb.AppendLine($"\t\t\t\t\treturn new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.Created);");
            sb.AppendLine($"\t\t\t\t}}");
            sb.AppendLine($"\t\t\t\telse");
            sb.AppendLine($"\t\t\t\t{{");
            sb.AppendLine($"\t\t\t\t\treturn new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NothingModified, null);");
            sb.AppendLine($"\t\t\t\t}}");
            sb.AppendLine($"\t\t\t}}");
            sb.AppendLine($"\t\t\tcatch(Exception ex)");
            sb.AppendLine($"\t\t\t{{");
            sb.AppendLine($"\t\t\t\treturn new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);");
            sb.AppendLine($"\t\t\t}}");
            sb.AppendLine($"\t\t}}");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tprivate async Task<IRepositoryActionResult<TEntity>> UpdateAsync<TEntity>(TEntity item, TEntity existingItem) where TEntity : class");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\ttry");
            sb.AppendLine($"\t\t\t{{ // only update when a record already exists for this id");
            sb.AppendLine($"\t\t\t\tif (existingItem == null)");
            sb.AppendLine($"\t\t\t\t{{");
            sb.AppendLine($"\t\t\t\t\treturn new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NotFound);");
            sb.AppendLine($"\t\t\t\t}}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t\t// change the original entity status to detached; otherwise, we get an error on attach as the entity is already in the dbSet");
            sb.AppendLine($"\t\t\t\t// set original entity state to detached");
            sb.AppendLine($"\t\t\t\tDbContext.Entry(existingItem).State = EntityState.Detached;");
            sb.AppendLine($"\t\t\t\tDbSet<TEntity> itemSet = DbContext.Set<TEntity>();");
            sb.AppendLine($"\t\t\t\titemSet.Attach(item); // attach & save");
            sb.AppendLine($"\t\t\t\tDbContext.Entry(item).State = EntityState.Modified; // set the updated entity state to modified, so it gets updated.");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t\tvar result = await DbContext.SaveChangesAsync();");
            sb.AppendLine($"\t\t\t\tRunCustomLogicAfterEveryUpdate<TEntity>(newItem: item, oldItem: existingItem, numObjectsWritten: result);{Environment.NewLine}");
            sb.AppendLine($"\t\t\t\tif (result > 0)");
            sb.AppendLine($"\t\t\t\t{{");
            sb.AppendLine($"\t\t\t\t\treturn new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.Updated);");
            sb.AppendLine($"\t\t\t\t}}");
            sb.AppendLine($"\t\t\t\telse");
            sb.AppendLine($"\t\t\t\t{{");
            sb.AppendLine($"\t\t\t\t\treturn new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NothingModified, null);");
            sb.AppendLine($"\t\t\t\t}}");
            sb.AppendLine($"\t\t\t}}");
            sb.AppendLine($"\t\t\tcatch (Exception ex)");
            sb.AppendLine($"\t\t\t{{");
            sb.AppendLine($"\t\t\t\treturn new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);");
            sb.AppendLine($"\t\t\t}}");
            sb.AppendLine($"\t\t}}{Environment.NewLine}");

            sb.AppendLine($"\t\tpartial void RunCustomLogicAfterEveryInsert<T>(T item, int numObjectsWritten) where T : class;{Environment.NewLine}");
            sb.AppendLine($"\t\tpartial void RunCustomLogicAfterEveryUpdate<T>(T newItem, T oldItem, int numObjectsWritten) where T : class;{Environment.NewLine}");

            sb.AppendLine($"\t\t#endregion Generic Operations");

            return sb.ToString();
        }

        public string GenerateGetFirstOrDefault(string tableName, string tableNamePlural,
            string methodParameterSignature,
            string methodParameterSignatureWithoutFieldTypes,
            string whereClause, string whereClauseWithObjectInstancePrefix)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"\t\t\tpublic async Task<{tableName}> Get_{tableName}Async({methodParameterSignature}");
            if (!string.IsNullOrEmpty(methodParameterSignature))
            {
                sb.Append($", ");
            }
            sb.Append($"int numChildLevels){Environment.NewLine}");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tvar qryItem = GetQueryable_{tableName}().AsNoTracking();");
            sb.AppendLine($"\t\t\t\tRunCustomLogicOnGetQueryableByPK_{tableName}(ref qryItem, {methodParameterSignatureWithoutFieldTypes}, numChildLevels);{Environment.NewLine}");

            sb.AppendLine($"\t\t\t\tvar dbItem = await qryItem.Where({whereClause}).FirstOrDefaultAsync();");
            sb.AppendLine($"\t\t\t\tif (!(dbItem is null))");
            sb.AppendLine($"\t\t\t\t{{");
            sb.AppendLine($"\t\t\t\t\tRunCustomLogicOnGetEntityByPK_{tableName}(ref dbItem, {methodParameterSignatureWithoutFieldTypes}, numChildLevels);");
            sb.AppendLine($"\t\t\t\t}}{Environment.NewLine}");

            sb.AppendLine($"\t\t\t\treturn dbItem;");
            sb.AppendLine($"\t\t\t}}{Environment.NewLine}");

            // GetFirstOrDefault By Object
            sb.AppendLine($"\t\t\tpublic async Task<{tableName}> GetFirstOrDefaultAsync({tableName} item)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\treturn await DbContext.{tableNamePlural}.Where({whereClauseWithObjectInstancePrefix}).FirstOrDefaultAsync();");
            sb.AppendLine($"\t\t\t}}{Environment.NewLine}");

            return sb.ToString();
        }

        public string GenerateGetQueryable(string entityName)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"\t\tpublic IQueryable<{entityName}> GetQueryable_{entityName}()");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\treturn DbContext.Set<{entityName}>();");
            sb.AppendLine($"\t\t}}");

            return sb.ToString();
        }

        public string GenerateInsertOperations(string entityName)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"\t\tpublic async Task<IRepositoryActionResult<{entityName}>> InsertAsync({entityName} item)");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\tvar result = await InsertAsync<{entityName}>(item);");
            sb.AppendLine($"\t\t\tRunCustomLogicAfterInsert_{entityName}(item, result);{Environment.NewLine}");

            sb.AppendLine($"\t\t\treturn result;");
            sb.AppendLine($"\t\t}}{Environment.NewLine}");

            return sb.ToString();
        }

        public string GenerateRepositoryBase(
            string repositoryInterfaceNamespace,
            string repositoryEntitiesNamespace,
            string dbContextName,
            string namespacePostfix,
            string baseNamespace,
            IList<IEntityType> entityTypes)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"using System;");
            sb.AppendLine($"using Microsoft.EntityFrameworkCore;");
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine($"using System.Threading.Tasks;");
            sb.AppendLine($"using {repositoryEntitiesNamespace};");
            sb.AppendLine($"using {repositoryInterfaceNamespace};");
            sb.AppendLine($"using CodeGenHero.Repository;");
            sb.AppendLine($"using cghEnums = CodeGenHero.Repository.Enums;");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {baseNamespace}.Repository");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic abstract partial class {namespacePostfix}RepositoryBase : I{namespacePostfix}RepositoryCrud");
            sb.AppendLine($"\t{{");
            sb.AppendLine($"\t\tprotected {dbContextName} DbContext {{ get; set; }}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tpublic {namespacePostfix}RepositoryBase({dbContextName} ctx)");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\tDbContext = ctx;");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t// Disable lazy loading - if not, related properties are auto-loaded when");
            sb.AppendLine($"\t\t\t// they are accessed for the first time, which means they'll be included when");
            sb.AppendLine($"\t\t\t// we serialize (b/c the serialization process accesses those properties).");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t// We don't want that, so we turn it off.  We want to eagerly load them (using Include) manually.");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t// ctx.Configuration.LazyLoadingEnabled = false;");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t// if (System.Diagnostics.Debugger.IsAttached)");
            sb.AppendLine($"\t\t\t// {{   // Write EF queries to the output console.");
            sb.AppendLine($"\t\t\t\t// ctx.Database.Log = x => System.Diagnostics.Debug.WriteLine(x);");
            sb.AppendLine($"\t\t\t// }}");
            sb.AppendLine($"\t\t}}");
            sb.AppendLine(string.Empty);

            sb.AppendLine(GenerateGenericOperations());

            foreach (var entity in entityTypes)
            {
                string entityName = entity.ClrType.Name;
                string tableNamePlural = Inflector.Pluralize(entity.ClrType.Name);
                string whereClause = WhereClause(objectInstancePrefix: null, indent: "\r\n\t\t\t\t\t\t", entity.Properties, entity.FindPrimaryKey(), useLowerForFirstCharOfPropertyName: true);

                string whereClauseWithObjectInstancePrefix = WhereClause(objectInstancePrefix: "item", indent: "\r\n\t\t\t\t\t\t", entity.Properties, entity.FindPrimaryKey(), useLowerForFirstCharOfPropertyName: false);
                string methodParameterSignature = GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey());
                string methodParameterSignatureWithoutFieldTypes = GetSignatureWithoutFieldTypes(string.Empty, entity.FindPrimaryKey(), lowercasePkNameFirstChar: true);

                sb.AppendLine($"\t\t#region {entityName}{Environment.NewLine}");

                sb.AppendLine(GenerateInsertOperations(entityName));
                sb.AppendLine(GenerateGetQueryable(entityName));
                sb.AppendLine(GenerateGetFirstOrDefault(tableName: entityName, tableNamePlural: tableNamePlural,
                    methodParameterSignature: methodParameterSignature, methodParameterSignatureWithoutFieldTypes: methodParameterSignatureWithoutFieldTypes,
                    whereClause: whereClause, whereClauseWithObjectInstancePrefix: whereClauseWithObjectInstancePrefix));
                sb.AppendLine(GenerateUpdateOperations(entityName: entityName, tableNamePlural: tableNamePlural, whereClauseWithObjectInstancePrefix: whereClauseWithObjectInstancePrefix));
                sb.AppendLine(GenerateDeleteOperations(entityName: entityName, tableNamePlural: tableNamePlural,
                    whereClause: whereClause, whereClauseWithObjectInstancePrefix: whereClauseWithObjectInstancePrefix, methodParameterSignature: methodParameterSignature));
                sb.AppendLine(GenerateCustomLogicPartialMethods(tableName: entityName, methodParameterSignature: methodParameterSignature));

                sb.AppendLine($"{Environment.NewLine}\t\t#endregion {entityName}{Environment.NewLine}");
            }

            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }

        public string GenerateUpdateOperations(string entityName, string tableNamePlural, string whereClauseWithObjectInstancePrefix)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"\t\tpublic async Task<IRepositoryActionResult<{entityName}>> UpdateAsync({entityName} item)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tvar oldItem = await DbContext.{tableNamePlural}.FirstOrDefaultAsync({whereClauseWithObjectInstancePrefix});");
            sb.AppendLine($"\t\t\tvar result = await UpdateAsync<{entityName}>(item, oldItem);");
            sb.AppendLine($"\t\t\tRunCustomLogicAfterUpdate_{entityName}(newItem: item, oldItem: oldItem, result: result);{Environment.NewLine}");
            sb.AppendLine($"\t\t\treturn result;");
            sb.AppendLine("\t\t}");

            return sb.ToString();
        }

        private string GenerateCustomLogicPartialMethods(string tableName, string methodParameterSignature)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"\t\tpartial void RunCustomLogicAfterInsert_{tableName}({tableName} item, IRepositoryActionResult<{tableName}> result);{Environment.NewLine}");
            sb.AppendLine($"\t\tpartial void RunCustomLogicAfterUpdate_{tableName}({tableName} newItem, {tableName} oldItem, IRepositoryActionResult<{tableName}> result);{Environment.NewLine}");
            sb.AppendLine($"\t\tpartial void RunCustomLogicOnGetQueryableByPK_{tableName}(ref IQueryable<{tableName}> qryItem, {methodParameterSignature}, int numChildLevels);{Environment.NewLine}");
            sb.AppendLine($"\t\tpartial void RunCustomLogicOnGetEntityByPK_{tableName}(ref {tableName} dbItem, {methodParameterSignature}, int numChildLevels);{Environment.NewLine}");

            return sb.ToString();
        }
    }
}