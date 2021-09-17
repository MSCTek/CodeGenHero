using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;
using System.Linq;

namespace CodeGenHero.Template.Blazor.Generators
{
    public class APIControllerGenerator : BaseBlazorGenerator
    {
        public APIControllerGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IEntityType entity,
            List<NameValue> maxRequestPerPageOverrides)
        {
            var entityName = $"{entity.ClrType.Name}";
            var humanizedEntityName = $"{Inflector.Humanize(entity.ClrType.Name)}";
            var className = $"{humanizedEntityName}Controller";

            // Set max page size.
            var maxPageSize = GetMaxPageSize(entity, maxRequestPerPageOverrides);

            // Begin Generation.
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic partial class {className} : {namespacePostfix}BaseApiController");
            sb.AppendLine("\t{");

            sb.AppendLine($"\t\tprivate const string GET_LIST_ROUTE_NAME = \"{humanizedEntityName}{namespacePostfix}List\";");
            sb.AppendLine($"\t\tprivate const int maxPageSize = {maxPageSize};");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tprivate I{namespacePostfix}GenericFactory<ent{namespacePostfix}.{humanizedEntityName}, dto{namespacePostfix}.{humanizedEntityName}> _factory;");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateConstructor(className, namespacePostfix, entityName));

            sb.Append(GenerateDelete(entity));
            sb.Append(GenerateGet(entity, namespacePostfix, entityName));
            sb.Append(GeneratePatch(entity, namespacePostfix, entityName));
            sb.Append(GeneratePost(entity, namespacePostfix, entityName));
            sb.Append(GeneratePut(entity, namespacePostfix, entityName));

            sb.Append(GeneratePartialMethodSignatures(namespacePostfix, entityName));

            sb.Append(GenerateFooter());

            return sb.ToString();
        }

        private string GenerateConstructor(string className, string namespacePostfix, string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}(ILogger<{className}> logger,");
            sb.AppendLine("\tIServiceProvider serviceProvider,");
            sb.AppendLine("\tIHttpContextAccessor httpContextAccessor,");
            sb.AppendLine("\tLinkGenerator linkGenerator,");
            sb.AppendLine($"\tI{namespacePostfix}Repository repository,");
            sb.AppendLine($"\tIGenericFactory<ent{namespacePostfix}.{entityName}, dto{namespacePostfix}.{entityName}> factory)");
            sb.AppendLine($"\t: base(logger, serviceProvider, httpContextAccessor, linkGenerator, repository)");
            sb.AppendLine("{");
            sb.AppendLine("\t_factory = factory;");
            sb.AppendLine("\tRunCustomLogicAfterCtor();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateDelete(IEntityType entity)
        {
            var methodSignature = GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true);
            var methodSignatureWithType = GetSignatureWithFieldTypes("", entity.FindPrimaryKey());
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"[HttpDelete(\"{{{methodSignature}}}\")]");
            sb.AppendLine("[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine($"public async Task<IActionResult> Delete({methodSignatureWithType})");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tvar result = await Repo.Delete_ArtworkAsync({methodSignature});");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (result.Status == cghrEnums.RepositoryActionStatus.Deleted)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\treturn NoContent();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t\telse if (result.Status == cghrEnums.RepositoryActionStatus.NotFound)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\treturn PrepareNotFoundResponse();");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\treturn PrepareExpectationFailedResponse(result.Exception);");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGet(IEntityType entity, string namespacePostfix, string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.Append(GenerateGetPages(namespacePostfix, entityName));
            sb.Append(GenerateGetByPK(entity, namespacePostfix, entityName));

            return sb.ToString();
        }

        #region Get Method Generators

        private string GenerateGetPages(string namespacePostfix, string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("[HttpGet(Name = GET_LIST_ROUTE_NAME)]");
            sb.AppendLine("[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine("public async Task<IActionResult> Get(string sort = null,");
            sb.AppendLine("\tstring fields = null, string filter = null, int page = 1, int pageSize = maxPageSize)");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar fieldList = GetListByDelimiter(fields);");
            sb.AppendLine("\t\tbool childrenRequested = fields == null ? false : fields.Contains('.');");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar filterList = GetListByDelimiter(filter);");
            sb.AppendLine($"\t\tvar dbItems = Repo.GetQueryable_{entityName}().AsNoTracking();");
            sb.AppendLine($"\t\tRunCustomLogicAfterGetQueryableList(ref dbItems, ref filterList);");
            sb.AppendLine("\t\tdbItems = dbItems.ApplyFilter(filterList);");
            sb.AppendLine($"\t\tdbItems = dbItems.ApplySort(sort ?? (typeof(ent{namespacePostfix}.{entityName}).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)).First().Name);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (pageSize > maxPageSize)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tpageSize = maxPageSize;");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tPageData paginationHeader = BuildPaginationHeader(GET_LIST_ROUTE_NAME, page: page, totalCount: dbItems.Count(), pageSize: pageSize, sort: sort);");
            sb.AppendLine("\t\tHttpContextAccessor.HttpContext.Response.Headers.Add(\"X-Pagination\", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\treturn Ok(dbItems");
            sb.AppendLine("\t\t.Skip(pageSize * (page - 1))");
            sb.AppendLine("\t\t.Take(pageSize)");
            sb.AppendLine("\t\t.ToList()");
            sb.AppendLine("\t\t.Select(x => _factory.CreateDataShapedObject(x, fieldList, childrenRequested)));");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");
            
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGetByPK(IEntityType entity, string namespacePostfix, string entityName)
        {
            var methodSignature = GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true);
            var methodSignatureWithType = GetSignatureWithFieldTypes("", entity.FindPrimaryKey());

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"[HttpGet(template: \"{{{methodSignature}}}/{{relatedEntitiesType:relatedEntitiesType=None}}\")]");
            sb.AppendLine("[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine($"public async Task<IActionResult> Get({methodSignatureWithType}, waEnums.RelatedEntitiesType relatedEntitiesType)");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tvar dbItem = await Repo.Get_{entityName}Async({methodSignature}, relatedEntitiesType);");
            sb.AppendLine($"\t\tif (dbItem == null)");
            sb.AppendLine("\t\t\treturn PrepareNotFoundResponse();");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tRunCustomLogicOnGetEntityByPK(ref dbItem, {methodSignature}, relatedEntitiesType);");
            sb.AppendLine("\t\treturn Ok(_factory.Create(dbItem));");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #endregion

        private string GeneratePatch(IEntityType entity, string namespacePostfix, string entityName)
        {
            var methodSignature = GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true);
            var methodSignatureWithType = GetSignatureWithFieldTypes("", entity.FindPrimaryKey());
            var pascalizedMethodSignature = Inflector.Pascalize(methodSignature);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"[HttpPatch(\"{{{methodSignature}}}\")]");
            sb.AppendLine("[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine($"public async Task<IActionResult> Patch({methodSignatureWithType}, [FromBody] JsonPatchDocument<dto{namespacePostfix}.{entityName}> patchDocument)");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (patchDocument == null)");
            sb.AppendLine("\t\t\treturn BadRequest();");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tvar dbItem = await Repo.Get_{entityName}Async({methodSignature});");
            sb.AppendLine("\t\tif (dbItem == null)");
            sb.AppendLine("\t\t\treturn NotFound();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar dtoItem = _factory.Create(dbItem); // Map the DB Entity to a DTO.");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t// Apply changes to the DTO");
            sb.AppendLine("\t\tpatchDocument.ApplyTo(dtoItem);");
            sb.AppendLine($"\t\tdtoItem.{pascalizedMethodSignature} = {methodSignature};");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t// Map the DTO with applied changes back to the DB Entity and perform the update.");
            sb.AppendLine("\t\tvar updatedDBItem = _factory.Create(dtoItem); // Map the DTO to a DB Entity.");
            sb.AppendLine("\t\tvar result = await Repo.UpdateAsync(updatedDBItem);");
            sb.AppendLine("\t\tRunCustomLogicAfterUpdatePatch(ref updatedDBItem, ref result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (result.Status == cghrEnums.RepositoryActionStatus.Updated)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tvar patchedDTOItem = _factory.Create(result.Entity); // Map the updated DB Entity to a DTO.");
            sb.AppendLine("\t\t\treturn Ok(patchedDTOItem);");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\treturn PrepareExpectationFailedResponse(result.Exception);");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");
            
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GeneratePost(IEntityType entity, string namespacePostfix, string entityName)
        {
            var methodSignature = GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true);
            var pascalizedMethodSignature = Inflector.Pascalize(methodSignature);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("[HttpPost]");
            sb.AppendLine("[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine($"public async Task<IActionResult> Post([FromBody] dto{namespacePostfix}.{entityName} dtoItem)");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (dtoItem == null)");
            sb.AppendLine("\t\t\treturn BadRequest();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar newDBItem = _factory.Create(dtoItem); // Map incoming DTO to DB Entity");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar result = await Repo.InsertAsync(newDBItem);");
            sb.AppendLine("\t\tRunCustomLogicAfterInsert(ref newDBItem, ref result);");

            sb.AppendLine("\t\tif (result.Status == cghrEnums.RepositoryActionStatus.Created)");
            sb.AppendLine("\t\t{");

            sb.AppendLine("\t\t\tvar newDTOItem = _factory.Create(result.Entity); // Map created DB Entity to a DTO");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t\tvar uriFormatted = LinkGenerator.GetUriByAction(");
            sb.AppendLine("\t\t\t\thttpContext: HttpContextAccessor.HttpContext,");
            sb.AppendLine("\t\t\t\taction: nameof(Get),");
            sb.AppendLine("\t\t\t\tcontroller: null, // Stay in this controller");
            sb.AppendLine($"\t\t\t\tvalues: new {{ newDTOItem.{pascalizedMethodSignature} }}");
            sb.AppendLine("\t\t\t\t);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t\treturn Created(uriFormatted, newDTOItem);");

            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\treturn PrepareExpectationFailedResponse(result.Exception);");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GeneratePut(IEntityType entity, string namespacePostfix, string entityName)
        {
            var methodSignature = GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true);
            var methodSignatureWithType = GetSignatureWithFieldTypes("", entity.FindPrimaryKey());
            var pascalizedMethodSignature = Inflector.Pascalize(methodSignature);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"[HttpPut(\"{{{methodSignature}}}\")]");
            sb.AppendLine($"[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine($"public async Task<IActionResult> Put({methodSignatureWithType}, [FromBody] dto{namespacePostfix}.{entityName} dtoItem)");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (dtoItem == null)");
            sb.AppendLine("\t\t\treturn BadRequest();");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tdtoItem.{pascalizedMethodSignature} = {methodSignature}; // Ensure that we update the record with a matching Primary Key.");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar updatedDBItem = _factory.Create(dtoItem); // Map the incoming DTO to a DB entity.");
            sb.AppendLine($"\t\tRunCustomLogicBeforeUpdatePut(ref updatedDBItem, {methodSignature});");
            sb.AppendLine("\t\tvar result = await Repo.UpdateAsync(updatedDBItem);");
            sb.AppendLine("\t\tRunCustomLogicAfterUpdatePut(ref updatedDBItem, ref result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (result.Status == cghrEnums.RepositoryActionStatus.Updated)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tvar updatedDTOItem = _factory.Create(result.Entity); // Map the updated DB Entity to a DTO");
            sb.AppendLine("\t\t\treturn Ok(updatedDTOItem);");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t\telse if (result.Status == cghrEnums.RepositoryActionStatus.NotFound)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\treturn NotFound();");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\treturn PrepareExpectationFailedResponse(result.Exception);");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GeneratePartialMethodSignatures(string namespacePostfix, string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"partial void RunCustomLogicAfterGetQueryableList(ref IQueryable<ent{namespacePostfix}.{entityName}> dbItems, ref List<string> filterList);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicAfterInsert(ref ent{namespacePostfix}.{entityName} newDBItem, ref IRepositoryActionResult<ent{namespacePostfix}.{entityName}> result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicAfterUpdatePatch(ref ent{namespacePostfix}.{entityName} updatedDBItem, ref IRepositoryActionResult<ent{namespacePostfix}.{entityName}> result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicAfterUpdatePut(ref ent{namespacePostfix}.{entityName} updatedDBItem, ref IRepositoryActionResult<ent{namespacePostfix}.{entityName}> result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicBeforeUpdatePut(ref ent{namespacePostfix}.{entityName} updatedDBItem, int announcementId);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicOnGetEntityByPK(ref ent{namespacePostfix}.{entityName} dbItem, int artworkId, waEnums.RelatedEntitiesType relatedEntitiesType);");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private int GetMaxPageSize(IEntityType entity, List<NameValue> maxRequestPerPageOverrides)
        {
            int retVal = -1;
            var maxRequestPerPageOverRideString = maxRequestPerPageOverrides
                .FirstOrDefault(x => x.Name == entity.ClrType.Name)?.Value;
            if (!string.IsNullOrWhiteSpace(maxRequestPerPageOverRideString))
            {
                int.TryParse(maxRequestPerPageOverRideString, out retVal);
            }

            if (retVal <= 0)
            {
                retVal = 100;
            }

            return retVal;
        }
    }
}
