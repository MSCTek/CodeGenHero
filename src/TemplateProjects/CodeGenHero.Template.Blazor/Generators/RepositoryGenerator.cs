using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;
using System.Linq;

namespace CodeGenHero.Template.Blazor.Generators
{
    class RepositoryGenerator : BaseBlazorGenerator
    {
        public RepositoryGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IList<IEntityType> entities,
            string className,
            string repositoryInterfaceClassName,
            string dbContextName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic abstract partial class {className} : {repositoryInterfaceClassName}");
            sb.AppendLine("\t{");
            sb.AppendLine($"\tprivate {dbContextName} _ctx;");
            sb.Append(GenerateConstructor(className, dbContextName));

            sb.AppendLine($"\tpublic {dbContextName} {dbContextName} {{ get {{ return _ctx; }} }}");

            sb.Append(GenerateGenericOperations());

            foreach(var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string tableNamePlural = Inflector.Pluralize(entity.ClrType.Name);
                string whereClause = WhereClause(objectInstancePrefix: null, indent: "", entity.Properties, entity.FindPrimaryKey(), useLowerForFirstCharOfPropertyName: true);
                string whereClauseWithObjectInstancePrefix = WhereClause(objectInstancePrefix: "item", indent: "", entity.Properties, entity.FindPrimaryKey(), useLowerForFirstCharOfPropertyName: false);
                string methodParameterSignature = GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey());
                string methodParameterSignatureWithoutFieldTypes = GetSignatureWithoutFieldTypes(string.Empty, entity.FindPrimaryKey(), lowercasePkNameFirstChar: true);

                sb.AppendLine($"\t#region {entityName}");
                sb.AppendLine(string.Empty);

                sb.Append(GenerateInsertOperation(entityName));
                sb.Append(GenerateGetQueryable(entityName));
                sb.Append(GenerateGetFirstOrDefault(entityName, tableNamePlural, methodParameterSignature, methodParameterSignatureWithoutFieldTypes, whereClause, whereClauseWithObjectInstancePrefix));
                sb.Append(GenerateUpdateOperation(entityName, tableNamePlural, whereClauseWithObjectInstancePrefix));
                sb.Append(GenerateDeleteOperations(entityName, tableNamePlural, whereClause, whereClauseWithObjectInstancePrefix, methodParameterSignature));
                sb.Append(GeneratePartialMethodSignatures(entityName, methodParameterSignature));

                sb.AppendLine("\t#endregion");
                sb.AppendLine(string.Empty);
            }

            sb.Append(GenerateFooter());
            return sb.ToString();
        }

        private string GenerateConstructor(string className, string dbContextName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}({dbContextName} ctx)");
            sb.AppendLine("{");

            sb.AppendLine("\t_ctx = ctx;");
            sb.AppendLine("\tctx.ChangeTracker.LazyLoadingEnabled = false;");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGenericOperations()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);
            sb.AppendLine("#region Generic Operations");

            sb.Append(GenerateGenericDelete());
            sb.Append(GenerateGenericGet());
            sb.Append(GenerateGenericInsert());
            sb.Append(GenerateGenericUpdate());
            sb.Append(GenerateGenericPartialMethods());

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #region Generic Operations Generators

        private string GenerateGenericDelete()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            // Using a Verbatim String as this is a large block of static code.
            string verbatim = @"
                private async Task<IRepositoryActionResult<TEntity>> DeleteAsync<TEntity>(TEntity item) where TEntity : class
		        {
			        IRepositoryActionResult<TEntity> retVal = null;

			        try
			        {
				        if (item == null)
				        {
					        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.NotFound);
				        }
				        else
				        {
					        DbSet<TEntity> itemSet = _ctx.Set<TEntity>();
					        itemSet.Remove(item);
					        await _ctx.SaveChangesAsync();
					        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Deleted);
				        }
			        }
			        catch (Exception ex)
			        {
				        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);
			        }

			        return retVal;
		        }";

            sb.Append(verbatim);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGenericGet()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class");
            sb.AppendLine("{");

            sb.AppendLine("\treturn _ctx.Set<TEntity>();");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGenericInsert()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            string verbatim = @"
                public async Task<IRepositoryActionResult<TEntity>> InsertAsync<TEntity>(TEntity item) 
			        where TEntity : class
		        {
			        IRepositoryActionResult<TEntity> retVal = null;

			        try
			        {
				        DbSet<TEntity> itemSet = _ctx.Set<TEntity>();
				        itemSet.Add(item);
				        var result = await _ctx.SaveChangesAsync();
				        RunCustomLogicAfterEveryInsert<TEntity>(item, result);

				        if (result > 0)
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.Created);
				        }
				        else
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NothingModified, null);
				        }
			        }
			        catch (Exception ex)
			        {
				        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);
			        }

			        return retVal;
		        }";

            sb.Append(verbatim);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGenericUpdate()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            string verbatim = @"
                private async Task<IRepositoryActionResult<TEntity>> UpdateAsync<TEntity>(TEntity item, TEntity existingItem) where TEntity : class
		        {
			        IRepositoryActionResult<TEntity> retVal = null;

			        try
			        { // only update when a record already exists for this id
				        if (existingItem == null)
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NotFound);
				        }

				        // change the original entity status to detached; otherwise, we get an error on attach as the entity is already in the dbSet
				        // set original entity state to detached
				        _ctx.Entry(existingItem).State = EntityState.Detached;
				        DbSet<TEntity> itemSet = _ctx.Set<TEntity>();
				        itemSet.Attach(item); // attach & save
				        _ctx.Entry(item).State = EntityState.Modified; // set the updated entity state to modified, so it gets updated.

				        var result = await _ctx.SaveChangesAsync();
				        RunCustomLogicAfterEveryUpdate<TEntity>(newItem: item, oldItem: existingItem, numObjectsWritten: result);

				        if (result > 0)
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.Updated);
				        }
				        else
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NothingModified, null);
				        }
			        }
			        catch (Exception ex)
			        {
				        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);
			        }

			        return retVal;
		        }";

            sb.Append(verbatim);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGenericPartialMethods()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("partial void RunCustomLogicAfterEveryInsert<T>(T item, int numObjectsWritten) where T : class;");
            sb.AppendLine(string.Empty);

            sb.AppendLine("partial void RunCustomLogicAfterEveryUpdate<T>(T newItem, T oldItem, int numObjectsWritten) where T : class;");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        #endregion

        private string GenerateInsertOperation(string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<IRepositoryActionResult<{entityName}>> InsertAsync({entityName} item)");
            sb.AppendLine("{");
            sb.AppendLine($"\tvar result = await InsertAsync<{entityName}>(item);");
            sb.AppendLine($"\tRunCustomLogicAfterInsert_{entityName}(item, result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\treturn result;");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGetQueryable(string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public IQueryable<{entityName}> GetQueryable_{entityName}()");
            sb.AppendLine("{");
            sb.AppendLine($"\treturn _ctx.Set<{entityName}>();");
            sb.AppendLine("}");

            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGetFirstOrDefault(string tableName, string tableNamePlural,
            string methodParameterSignature,
            string methodParameterSignatureWithoutFieldTypes,
            string whereClause, string whereClauseWithObjectInstancePrefix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.Append(GenerateGetFirstOrDefaultByPrimaryKey(tableName, methodParameterSignature, methodParameterSignatureWithoutFieldTypes, whereClause));
            sb.Append(GenerateGetFirstOrDefaultByObject(tableName, tableNamePlural, whereClauseWithObjectInstancePrefix));

            return sb.ToString();
        }

        #region Get First or Default Generators

        private string GenerateGetFirstOrDefaultByPrimaryKey(string tableName, string methodParameterSignature, string methodParameterSignatureWithoutFieldTypes, string whereClause)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<{tableName}> Get_{tableName}Async({methodParameterSignature}, waEnums.RelatedEntitiesType relatedEntitiesType)");
            sb.AppendLine("{");

            sb.AppendLine($"\tvar qryItem = GetQueryable_{tableName}();");
            sb.AppendLine($"\tRunCustomLogicOnGetQueryableByPK_{tableName}(ref qryItem, {methodParameterSignatureWithoutFieldTypes}, relatedEntitiesType);");
            sb.AppendLine("\tqryItem = qryItem.AsNoTracking();");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\tvar dbItem = await qryItem.Where({whereClause}).FirstOrDefaultAsync();");
            sb.AppendLine("\tif (!(dbItem is null))");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tRunCustomLogicOnGetEntityByPK_{tableName}(ref dbItem, {methodParameterSignatureWithoutFieldTypes}, relatedEntitiesType);");
            sb.AppendLine("\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("return dbItem;");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGetFirstOrDefaultByObject(string tableName, string tableNamePlural, string whereClauseWithObjectInstancePrefix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<{tableName}> GetFirstOrDefaultAsync({tableName} item)");
            sb.AppendLine("{");

            sb.AppendLine($"return await _ctx.{tableNamePlural}.Where({whereClauseWithObjectInstancePrefix}).FirstOrDefaultAsync();");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #endregion

        private string GenerateUpdateOperation(string entityName, string tableNamePlural, string whereClauseWithObjectInstancePrefix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<IRepositoryActionResult<{entityName}>> UpdateAsync({entityName} item)");
            sb.AppendLine("{");

            sb.AppendLine($"var oldItem = await _ctx.{tableNamePlural}.FirstOrDefaultAsync({whereClauseWithObjectInstancePrefix});");
            sb.AppendLine($"var result = await UpdateAsync<{entityName}>(item, oldItem);");
            sb.AppendLine($"RunCustomLogicAfterUpdate_{entityName}(newItem: item, oldItem: oldItem, result: result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("return result;");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateDeleteOperations(string entityName, string tableNamePlural,
            string whereClause, string whereClauseWithObjectInstancePrefix, string methodParameterSignature)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.Append(GenerateDeleteByPrimaryKey(entityName, tableNamePlural, whereClause, methodParameterSignature));
            sb.Append(GenerateDeleteByObject(entityName, tableNamePlural, whereClauseWithObjectInstancePrefix));

            return sb.ToString();
        }

        #region Delete Operation Generators

        private string GenerateDeleteByPrimaryKey(string entityName, string tableNamePlural, string whereClause, string methodParameterSignature)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<IRepositoryActionResult<{entityName}>> Delete_{entityName}Async({methodParameterSignature})");
            sb.AppendLine("{");

            sb.AppendLine($"\treturn await DeleteAsync<{entityName}>(_ctx.{tableNamePlural}.Where({whereClause}).FirstOrDefault());");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateDeleteByObject(string entityName, string tableNamePlural, string whereClauseWithObjectInstancePrefix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<IRepositoryActionResult<{entityName}>> DeleteAsync({entityName} item)");
            sb.AppendLine("{");

            sb.AppendLine($"\treturn await DeleteAsync<{entityName}>(_ctx.{tableNamePlural}.Where({whereClauseWithObjectInstancePrefix}).FirstOrDefault());");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #endregion

        private string GeneratePartialMethodSignatures(string tableName, string methodParameterSignature)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"partial void RunCustomLogicAfterInsert_{tableName}({tableName} item, IRepositoryActionResult<{tableName}> result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicAfterUpdate_{tableName}({tableName} newItem, {tableName} oldItem, IRepositoryActionResult<{tableName}> result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicOnGetQueryableByPK_{tableName}(ref IQueryable<{tableName}> qryItem, {methodParameterSignature}, waEnums.RelatedEntitiesType relatedEntitiesType);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicOnGetEntityByPK_{tableName}(ref {tableName} dbItem, {methodParameterSignature}, waEnums.RelatedEntitiesType relatedEntitiesType);");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }
    }
}
