using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Server
{
    public class WebApiControllerGenerator : BaseAPIFFGenerator
    {
        private static Type parent = typeof(BaseAPIFFGenerator);

        public WebApiControllerGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public static string GenerateConstructors(string className, string repositoryInterfaceName,
            string efEntityNamespacePrefix, string dtoNamespacePrefix, string tableName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);
            //sb.AppendLine($"public {className}() : base()");
            //sb.AppendLine("{");
            //sb.AppendLine("}");
            //sb.AppendLine(string.Empty);

            sb.AppendLine($"public {className}(ILogger<{className}> log, {repositoryInterfaceName} repository, IMapper mapper)");
            sb.AppendLine($": base(log, repository)");
            sb.AppendLine("{");
            sb.AppendLine($"_factory = new GenericFactory<{efEntityNamespacePrefix}.{tableName}, {dtoNamespacePrefix}.{tableName}>(mapper);");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GenerateDelete(IEntityType entity)
        {
            string keylist = string.Join("}/{", entity.FindPrimaryKey());
            string classmodel = MakeModel(entity.FindPrimaryKey(), "", "", ", ", false, false, lowercaseVariableName: false);
            //string signature = table.Signature("");
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("[HttpDelete]");
            sb.AppendLine($"[VersionedRoute(template: \"{Inflector.Pluralize(entity.ClrType.Name)}/{{{GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true)}}}\", allowedVersion: 1)]");
            sb.AppendLine($"public async Task<IHttpActionResult> Delete({GetSignatureWithFieldTypes("", entity.FindPrimaryKey())})");
            sb.AppendLine("{");
            sb.AppendLine("try");
            sb.AppendLine("{");
            sb.AppendLine("if (!base.OnActionExecuting(out HttpStatusCode httpStatusCode, out string message)) { return Content(httpStatusCode, message); }");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"var result = await Repo.Delete_{entity.ClrType.Name}Async({GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true)});");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"if (result.Status == cghEnums.RepositoryActionStatus.Deleted)");
            sb.AppendLine("{");
            sb.AppendLine("return StatusCode(HttpStatusCode.NoContent);");
            sb.AppendLine("}");
            sb.AppendLine($"else if (result.Status == cghEnums.RepositoryActionStatus.NotFound)");
            sb.AppendLine("{");
            sb.AppendLine("return NotFound();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Log.LogWarning(eventId: (int)coreEnums.EventId.Warn_WebApi,");
            sb.AppendLine("\texception: result.Exception,");
            sb.AppendLine("\tmessage: \"Unable to delete object via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return BadRequest();");
            sb.AppendLine("}");
            sb.AppendLine("catch (Exception ex)");
            sb.AppendLine("{");
            sb.AppendLine("Log.LogError(eventId: (int)coreEnums.EventId.Exception_WebApi,");
            sb.AppendLine("\texception: ex,");
            sb.AppendLine("\tmessage: \"Unable to delete object via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (System.Diagnostics.Debugger.IsAttached)");
            sb.AppendLine("\tSystem.Diagnostics.Debugger.Break();");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return InternalServerError();");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        public string GenerateFooter(string dbContextName, IEntityType entity,
            IList<IEntityNavigation> excludedNavigationProperties, string entityTypeName)
        {
            StringBuilder sb = new StringBuilder();
            string signature = GetSignatureWithFieldTypes("", entity.FindPrimaryKey());

            sb.AppendLine($"\t\tpartial void RunCustomLogicAfterInsert(ref {entityTypeName} newDBItem, ref IRepositoryActionResult<{entityTypeName}> result);");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tpartial void RunCustomLogicAfterUpdatePatch(ref {entityTypeName} updatedDBItem, ref IRepositoryActionResult<{entityTypeName}> result);");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tpartial void RunCustomLogicAfterUpdatePut(ref {entityTypeName} updatedDBItem, ref IRepositoryActionResult<{entityTypeName}> result);");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tpartial void RunCustomLogicBeforeUpdatePut(ref {entityTypeName} updatedDBItem, {signature});");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tpartial void RunCustomLogicOnGetEntityByPK(ref {entityTypeName} dbItem, {GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey())}, int numChildLevels);");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tpartial void RunCustomLogicAfterGetQueryableList(ref IQueryable<{entityTypeName}> dbItems, ref List<string> filterList);");

            return sb.ToString();
        }

        public string GenerateGet(IEntityType entity, string tableName, string entityTypeName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);
            sb.AppendLine("[HttpGet]");
            sb.AppendLine($"[VersionedRoute(template: \"{Inflector.Pluralize(entity.ClrType.Name)}\", allowedVersion: 1, Name = GET_LIST_ROUTE_NAME)]");
            sb.AppendLine("#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously");
            sb.AppendLine("public async Task<IHttpActionResult> Get(string sort = null,");
            sb.AppendLine("string fields = null, string filter = null, int page = 1, int pageSize = maxPageSize)");
            sb.AppendLine("#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously");
            sb.AppendLine("{");
            sb.AppendLine("try");
            sb.AppendLine("{");
            sb.AppendLine("if (!base.OnActionExecuting(out HttpStatusCode httpStatusCode, out string message)) { return Content(httpStatusCode, message); }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("var fieldList = GetListByDelimiter(fields);");
            sb.AppendLine("bool childrenRequested = false; // TODO: set this based upon actual fields requested.");
            sb.AppendLine(string.Empty);
            sb.AppendLine("var filterList = GetListByDelimiter(filter);");
            sb.AppendLine($"var dbItems = Repo.GetQueryable_{tableName}().AsNoTracking();");
            sb.AppendLine($"RunCustomLogicAfterGetQueryableList(ref dbItems, ref filterList);");
            sb.AppendLine($"dbItems = dbItems.ApplyFilter(filterList);");
            sb.AppendLine($"dbItems = dbItems.ApplySort(sort ?? (typeof({entityTypeName}).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)).First().Name);");
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (pageSize > maxPageSize)");
            sb.AppendLine("{ // ensure the page size isn't larger than the maximum.");
            sb.AppendLine("pageSize = maxPageSize;");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("var urlHelper = new UrlHelper(Request);");
            sb.AppendLine("PageData paginationHeader = BuildPaginationHeader(urlHelper, GET_LIST_ROUTE_NAME, page: page, totalCount: dbItems.Count(), pageSize: pageSize, sort: sort);");
            sb.AppendLine("HttpContext.Current.Response.Headers.Add(\"X-Pagination\", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));");
            sb.AppendLine(string.Empty);
            sb.AppendLine("// return result");
            sb.AppendLine("return Ok(dbItems");
            sb.AppendLine(".Skip(pageSize * (page - 1))");
            sb.AppendLine(".Take(pageSize)");
            sb.AppendLine(".ToList()");
            sb.AppendLine(".Select(x => _factory.CreateDataShapedObject(x, fieldList, childrenRequested)));");
            sb.AppendLine("}");
            sb.AppendLine("catch (Exception ex)");
            sb.AppendLine("{");
            sb.AppendLine("Log.LogError(eventId: (int)coreEnums.EventId.Exception_WebApi,");
            sb.AppendLine("\texception: ex,");
            sb.AppendLine("\tmessage: \"Unable to get object via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (System.Diagnostics.Debugger.IsAttached)");
            sb.AppendLine("\tSystem.Diagnostics.Debugger.Break();");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return InternalServerError();");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        public string GenerateGetByPK(IEntityType entity, string tableName)
        {
            string keylist = string.Join("}/{", entity.GetKeys());
            string makeModel = MakeModel(entity.FindPrimaryKey(), "", "", ", ", false, false, lowercaseVariableName: false);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("[HttpGet]");
            sb.AppendLine($"[VersionedRoute(template: \"{Inflector.Pluralize(entity.ClrType.Name)}/{{{GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true)}}}/{{numChildLevels:int=0}}\", allowedVersion: 1)]");
            sb.AppendLine($"public async Task<IHttpActionResult> Get({GetSignatureWithFieldTypes("", entity.FindPrimaryKey())}, int numChildLevels)");
            sb.AppendLine($"{{");
            sb.AppendLine($"try");
            sb.AppendLine($"{{");
            sb.AppendLine("if (!base.OnActionExecuting(out HttpStatusCode httpStatusCode, out string message)) { return Content(httpStatusCode, message); }");
            sb.AppendLine($"var dbItem = await Repo.Get_{tableName}Async({GetSignatureWithoutFieldTypes(string.Empty, entity.FindPrimaryKey(), lowercasePkNameFirstChar: true)}, numChildLevels);");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"if (dbItem == null)");
            sb.AppendLine($"{{");
            sb.AppendLine("Log.LogWarning(eventId: (int)coreEnums.EventId.Warn_WebApi,");
            sb.AppendLine("\tmessage: \"Unable to get object via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"return NotFound();");
            sb.AppendLine($"}}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"RunCustomLogicOnGetEntityByPK(ref dbItem, {GetSignatureWithoutFieldTypes(string.Empty, entity.FindPrimaryKey(), lowercasePkNameFirstChar: true)}, numChildLevels);");
            //sb.AppendLine($"base.Repo.{dbContextName}.Entry(dbItem).State = EntityState.Detached;"); // Setting an entity to the Detached state used to be important before the Entity Framework supported POCO objects. Prior to POCO support, your entities would have references to the context that was tracking them. These references would cause issues when trying to attach an entity to a second context. Setting an entity to the Detached state clears out all the references to the context and also clears the navigation properties of the entity—so that it no longer references any entities being tracked by the context. Now that you can use POCO objects that don’t contain references to the context that is tracking them, there is rarely a need to move an entity to the Detached state.
            sb.AppendLine($"return Ok(_factory.Create(dbItem));");
            sb.AppendLine($"}}");
            sb.AppendLine($"catch (Exception ex)");
            sb.AppendLine($"{{");
            sb.AppendLine("Log.LogError(eventId: (int)coreEnums.EventId.Exception_WebApi,");
            sb.AppendLine("\texception: ex,");
            sb.AppendLine("\tmessage: \"Unable to get object by PK via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"if (System.Diagnostics.Debugger.IsAttached)");
            sb.AppendLine($"\tSystem.Diagnostics.Debugger.Break();");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"return InternalServerError();");
            sb.AppendLine($"}}");
            sb.AppendLine($"}}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GeneratePatch(IEntityType entity, string tableName, string dtoNamespacePrefix,
            string entityTypeName)
        {
            string keylist = string.Join("}/{", entity.GetKeys());
            IndentingStringBuilder sb = new IndentingStringBuilder(2);
            string classmodel = MakeModel(entity.FindPrimaryKey(), "", "", ", ", false, false, lowercaseVariableName: false);

            sb.AppendLine("[HttpPatch]");
            sb.AppendLine($"[VersionedRoute(template: \"{Inflector.Pluralize(entity.ClrType.Name)}/{{{GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true)}}}\", allowedVersion: 1)]");
            sb.AppendLine($"public async Task<IHttpActionResult> Patch({GetSignatureWithFieldTypes("", entity.FindPrimaryKey())}, [FromBody] JsonPatchDocument<{dtoNamespacePrefix}.{tableName}> patchDocument)");
            sb.AppendLine("{");
            sb.AppendLine("try");
            sb.AppendLine("{");
            sb.AppendLine("if (!base.OnActionExecuting(out HttpStatusCode httpStatusCode, out string message)) { return Content(httpStatusCode, message); }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (patchDocument == null)");
            sb.AppendLine("{");
            sb.AppendLine("return BadRequest();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"var dbItem = await Repo.Get_{tableName}Async({GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true)}, numChildLevels: 0);");
            sb.AppendLine("if (dbItem == null)");
            sb.AppendLine("{");
            sb.AppendLine("return NotFound();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("var dtoItem = _factory.Create(dbItem); // map");
            sb.AppendLine(string.Empty);
            sb.AppendLine("// apply changes to the DTO");
            sb.AppendLine("patchDocument.ApplyTo(dtoItem);");
            sb.AppendLine($"{MakeModel(entity.FindPrimaryKey(), sb.CurrentIndentString, "dtoItem.", ";", true, true, lowercaseVariableName: true)}");
            sb.AppendLine("// map the DTO with applied changes to the entity, & update");
            sb.AppendLine("var updatedDBItem = _factory.Create(dtoItem); // map");
            sb.AppendLine("var result = await Repo.UpdateAsync(updatedDBItem);");
            sb.AppendLine($"RunCustomLogicAfterUpdatePatch(ref updatedDBItem, ref result);");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"if (result.Status == cghEnums.RepositoryActionStatus.Updated)");
            sb.AppendLine("{");
            sb.AppendLine("// map to dto");
            sb.AppendLine("var patchedDTOItem = _factory.Create(result.Entity);");
            sb.AppendLine("return Ok(patchedDTOItem);");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Log.LogWarning(eventId: (int)coreEnums.EventId.Warn_WebApi,");
            sb.AppendLine("\texception: result.Exception,");
            sb.AppendLine("\tmessage: \"Unable to patch object via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return BadRequest();");
            sb.AppendLine("}");
            sb.AppendLine("catch (Exception ex)");
            sb.AppendLine("{");
            sb.AppendLine("Log.LogError(eventId: (int)coreEnums.EventId.Exception_WebApi,");
            sb.AppendLine("\texception: ex,");
            sb.AppendLine("\tmessage: \"Unable to patch object via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (System.Diagnostics.Debugger.IsAttached)");
            sb.AppendLine("\tSystem.Diagnostics.Debugger.Break();");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return InternalServerError();");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GeneratePost(IEntityType entity, string tableName, string dtoNamespacePrefix)
        {
            if (entity is null)
            {
                throw new System.ArgumentNullException(nameof(entity));
            }
            //take the primary keys array and convert it to a string in the format of "newDTOItem.key1}/{newDTOItem.key2}/{newDTOItem.key3"
            //no need to add the {} brackets at the start and end.
            string classuri = "newDTOItem." + string.Join("}/{newDTOItem.", GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: false));

            //string classuri = table.MakeModelUri("", "newDTOItem.", "&", false);
            IndentingStringBuilder sb = new IndentingStringBuilder(2);
            sb.AppendLine("[HttpPost]");
            sb.AppendLine($"[VersionedRoute(template: \"{Inflector.Pluralize(entity.ClrType.Name)}\", allowedVersion: 1)]");
            sb.AppendLine($"public async Task<IHttpActionResult> Post([FromBody] {dtoNamespacePrefix}.{tableName} dtoItem)");
            sb.AppendLine("{");
            sb.AppendLine("try");
            sb.AppendLine("{");
            sb.AppendLine("if (!base.OnActionExecuting(out HttpStatusCode httpStatusCode, out string message)) { return Content(httpStatusCode, message); }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (dtoItem == null)");
            sb.AppendLine("{");
            sb.AppendLine("return BadRequest();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("// try mapping & saving");
            sb.AppendLine("var newDBItem = _factory.Create(dtoItem);");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"var result = await Repo.InsertAsync(newDBItem);");
            sb.AppendLine($"RunCustomLogicAfterInsert(ref newDBItem, ref result);");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"if (result.Status == cghEnums.RepositoryActionStatus.Created)");
            sb.AppendLine("{   // map to dto");
            sb.AppendLine("var newDTOItem = _factory.Create(result.Entity);");
            sb.AppendLine("var uriFormatted = Request.RequestUri.ToString().EndsWith(\"/\") == true ? Request.RequestUri.ToString().Substring(0, Request.RequestUri.ToString().Length - 1) : Request.RequestUri.ToString();");
            sb.AppendLine($"return Created($\"{{uriFormatted}}/{{{classuri}}}\", newDTOItem);");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Log.LogWarning(eventId: (int)coreEnums.EventId.Warn_WebApi,");
            sb.AppendLine("\texception: result.Exception,");
            sb.AppendLine("\tmessage: \"Unable to create object via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return BadRequest();");
            sb.AppendLine("}");
            sb.AppendLine("catch (Exception ex)");
            sb.AppendLine("{");
            sb.AppendLine("Log.LogError(eventId: (int)coreEnums.EventId.Exception_WebApi,");
            sb.AppendLine("\texception: ex,");
            sb.AppendLine("\tmessage: \"Unable to create object via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (System.Diagnostics.Debugger.IsAttached)");
            sb.AppendLine("\tSystem.Diagnostics.Debugger.Break();");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return InternalServerError();");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GeneratePut(IEntityType entity, string tableName, string dtoNamespacePrefix,
            string entityTypeName)
        {
            string keylist = string.Join("}/{", entity.GetKeys());
            string signatureWithFieldTypes = GetSignatureWithFieldTypes("", entity.FindPrimaryKey());
            string signatureWithoutFieldTypes = GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);
            sb.AppendLine("[HttpPut]");
            sb.AppendLine($"[VersionedRoute(template: \"{Inflector.Pluralize(entity.ClrType.Name)}/{{{GetSignatureWithoutFieldTypes("", entity.FindPrimaryKey(), lowercasePkNameFirstChar: true)}}}\", allowedVersion: 1)]");
            sb.AppendLine($"public async Task<IHttpActionResult> Put({signatureWithFieldTypes}, [FromBody] {dtoNamespacePrefix}.{tableName} dtoItem)");
            sb.AppendLine("{");
            sb.AppendLine("try");
            sb.AppendLine("{");
            sb.AppendLine("if (!base.OnActionExecuting(out HttpStatusCode httpStatusCode, out string message)) { return Content(httpStatusCode, message); }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (dtoItem == null)");
            sb.AppendLine("{");
            sb.AppendLine("return BadRequest();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"{MakeModel(entity.FindPrimaryKey(), sb.CurrentIndentString, "dtoItem.", ";", true, true, lowercaseVariableName: true)}");
            sb.AppendLine("var updatedDBItem = _factory.Create(dtoItem); // map");
            sb.AppendLine($"RunCustomLogicBeforeUpdatePut(ref updatedDBItem, {signatureWithoutFieldTypes});");
            sb.AppendLine("var result = await Repo.UpdateAsync(updatedDBItem);");
            sb.AppendLine($"RunCustomLogicAfterUpdatePut(ref updatedDBItem, ref result);");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"if (result.Status == cghEnums.RepositoryActionStatus.Updated)");
            sb.AppendLine("{");
            sb.AppendLine("// map to dto");
            sb.AppendLine("var updatedDTOItem = _factory.Create(result.Entity);");
            sb.AppendLine("return Ok(updatedDTOItem);");
            sb.AppendLine("}");
            sb.AppendLine($"else if (result.Status == cghEnums.RepositoryActionStatus.NotFound)");
            sb.AppendLine("{");
            sb.AppendLine("return NotFound();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Log.LogWarning(eventId: (int)coreEnums.EventId.Warn_WebApi,");
            sb.AppendLine("\texception: result.Exception,");
            sb.AppendLine("\tmessage: \"Unable to update object via Web API for RequestUri {{RequestUri}}\", ");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return BadRequest();");
            sb.AppendLine("}");
            sb.AppendLine("catch (Exception ex)");
            sb.AppendLine("{");
            sb.AppendLine("Log.LogError(eventId: (int)coreEnums.EventId.Exception_WebApi,");
            sb.AppendLine("\texception: ex,");
            sb.AppendLine("\tmessage: \"Unable to update object via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (System.Diagnostics.Debugger.IsAttached)");
            sb.AppendLine("\tSystem.Diagnostics.Debugger.Break();");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return InternalServerError();");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GenerateWebApiController(
            string baseNamespace,
            string namespacePostfix,
            string classNamespace,
            string baseControllerName,
            string apiHelpersNamespace,
            string iLoggingServiceNamespace,
            string repositoryNamespace,
            string factoryNamespace,
            string efEntityNamespacePrefix,
            string efEntityNamespace,
            string dtoNamespacePrefix,
            string dtoNamespace,
            string enumNamespace,
            string repositoryInterfaceName,
            string dbContextName,
            List<NameValue> maxRequestPerPageOverrides,
            bool prependSchemaNameIndicator,
            IEntityType entity,
            IList<IEntityNavigation> excludedEntityNavigations)
        {
            var className = $"{Inflector.Pluralize(entity.ClrType.Name)}{namespacePostfix}Controller";
            string tableName = Inflector.Humanize(entity.ClrType.Name);
            string tablenameSchema = Inflector.Humanize(entity.ClrType.Name);
            string tablenameNoSchema = tablenameSchema;
            string tablenamePlural = Inflector.Pluralize(entity.ClrType.Name);
            // TODO:  find equivalent
            //if (entity.TableSchema != null && !entity.TableSchema.Equals("dbo", System.StringComparison.OrdinalIgnoreCase))
            //{
            //    tablenameSchema = $"{entity.TableSchema}_{tablenameNoSchema}";
            //    tablenamePlural = $"{entity.TableSchema}_{tablenamePlural}";
            //}

            string entityTypeName = $"{efEntityNamespacePrefix}.{tablenameSchema}";

            //set maxpage size
            var maxPageSize = 0;
            var maxRequestPerPageOverRideString = maxRequestPerPageOverrides
                .FirstOrDefault(x => x.Name == entity.ClrType.Name)?.Value;
            if (!string.IsNullOrWhiteSpace(maxRequestPerPageOverRideString))
            {
                int.TryParse(maxRequestPerPageOverRideString, out maxPageSize);
            }

            if (maxPageSize <= 0)
            {
                maxPageSize = 100;
            }

            var sb = new IndentingStringBuilder();

            var usings = new List<NamespaceItem>
            {
                new NamespaceItem("AutoMapper"),
                new NamespaceItem("System"),
                new NamespaceItem("System.Collections.Generic"),
                new NamespaceItem("Microsoft.EntityFrameworkCore"),
                new NamespaceItem("Microsoft.Extensions.Logging"),
                new NamespaceItem("System.Linq"),
                new NamespaceItem("System.Net"),
                new NamespaceItem("System.Threading.Tasks"),
                new NamespaceItem("System.Web"),
                new NamespaceItem("System.Web.Http"),
                new NamespaceItem("System.Web.Http.Routing"),
                new NamespaceItem("Marvin.JsonPatch"),
                new NamespaceItem($"cghEnums", "CodeGenHero.Repository.Enums"),
                new NamespaceItem($"coreEnums", "CodeGenHero.Core.Enums"),
                new NamespaceItem("CodeGenHero.DataService"),
                new NamespaceItem("CodeGenHero.Repository"),
                new NamespaceItem("CodeGenHero.Repository.AutoMapper"),
                new NamespaceItem("CodeGenHero.WebApi"),
                new NamespaceItem($"{baseNamespace}.Repository.Interface"),
                new NamespaceItem($"dto{namespacePostfix}", string.IsNullOrWhiteSpace(namespacePostfix) ? $"{baseNamespace}.DTO" : $"{baseNamespace}.DTO.{namespacePostfix}"),
                new NamespaceItem($"ent{namespacePostfix}", efEntityNamespace)
            };

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic partial class {className} : {baseControllerName}");
            sb.AppendLine("\t{");

            sb.AppendLine($"\t\tprivate const string GET_LIST_ROUTE_NAME = \"{Inflector.Pluralize(entity.ClrType.Name)}{namespacePostfix}List\";");
            sb.AppendLine($"\t\tprivate const int maxPageSize = {maxPageSize};");
            sb.AppendLine($"\t\tprivate GenericFactory<{efEntityNamespacePrefix}.{tableName}, {dtoNamespacePrefix}.{tableName}> _factory;");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateConstructors(className: className, repositoryInterfaceName: repositoryInterfaceName,
                efEntityNamespacePrefix: efEntityNamespacePrefix, dtoNamespacePrefix: dtoNamespacePrefix, tableName: tableName));

            sb.Append(GenerateDelete(entity: entity));

            sb.Append(GenerateGet(entity: entity, tableName: tableName, entityTypeName: entityTypeName));

            sb.Append(GenerateGetByPK(entity: entity, tableName: tableName));

            sb.Append(GeneratePatch(entity: entity, tableName: tableName, dtoNamespacePrefix: dtoNamespacePrefix,
                entityTypeName: entityTypeName));

            sb.Append(GeneratePost(entity: entity, tableName: tableName, dtoNamespacePrefix: dtoNamespacePrefix));

            sb.Append(GeneratePut(entity: entity, tableName: tableName, dtoNamespacePrefix: dtoNamespacePrefix,
                entityTypeName: entityTypeName));

            sb.Append(GenerateFooter(dbContextName: dbContextName, entity: entity,
                excludedNavigationProperties: excludedEntityNavigations,
                entityTypeName: entityTypeName));

            sb.Append("\t}\r\n}"); // Close the class, close the namespace
            return sb.ToString();
        }
    }
}