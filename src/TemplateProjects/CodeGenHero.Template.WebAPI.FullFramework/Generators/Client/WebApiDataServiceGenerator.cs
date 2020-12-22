using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using System.Linq;
using System;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Client
{
    public class WebApiDataServiceGenerator : BaseAPIFFGenerator
    {
        public WebApiDataServiceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateHeader(
            string iLoggingServiceNamespace,
            string serializationHelperNamespace,
            string webApiDataServiceInterfaceNamespace,
            string webApiDataServiceNamespace,
            string dtoNamespacePrefix,
            string dtoNamespace,
            string namespacePostfix,
            string baseNamespace)
        {
            var sb = new StringBuilder();

            var className = $"WebApiDataService{namespacePostfix}";

            //sb.AppendLine($"using System;");
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Net.Http;");
            sb.AppendLine($"using System.Threading.Tasks;");
            sb.AppendLine($"using CodeGenHero.DataService;");
            sb.AppendLine($"using {webApiDataServiceInterfaceNamespace};");
            sb.AppendLine($"using {dtoNamespacePrefix} = {dtoNamespace};");
            sb.AppendLine($"using Microsoft.Extensions.Logging;");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {webApiDataServiceNamespace}");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic partial class {className} : WebApiDataServiceBase, I{className}");
            sb.AppendLine($"\t{{");

            sb.AppendLine($"\t\tpublic {className}(ILogger log, HttpClient httpClient) : base(log, httpClient, isServiceOnlineRelativeUrl: \"{namespacePostfix}/APIStatus\")");
            sb.AppendLine($"\t\t{{ }}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tprivate {className}() : base(log: null, httpclient: null)");
            sb.AppendLine($"\t\t{{ }}");

            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GenerateSerializeCallFor(string namespacePostfix, string dtoNamespacePrefix, string httpVerb,
            bool prependSchemaNameIndicator, IEntityType entity)
        {
            StringBuilder sb = new StringBuilder();
            string clientMethodCallPrefix = httpVerb.ToLowerInvariant() == "post" ? "Create" : "Update";
            string serializeCallPostfix = httpVerb.ToLowerInvariant() == "post" ? "Post" : "Put";
            string entityName = Inflector.Pascalize(entity.ClrType.Name);
            string tablenamePlural = Inflector.Pluralize(entity.ClrType.Name);
            string keylist = "item." + string.Join("}/{item.", entity.GetKeys().Select(k => k.Properties[0].Name));

            sb.AppendLine($"\t\t\tpublic async Task<IHttpCallResultCGHT<{dtoNamespacePrefix}.{entityName}>> {clientMethodCallPrefix}{entityName}Async({dtoNamespacePrefix}.{entityName} item)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tvar retVal = await SerializationHelper.Instance.SerializeCallResults{serializeCallPostfix}<{dtoNamespacePrefix}.{entityName}>(");
            sb.AppendLine("\t\t\t\t\tLog, HttpClient,");
            sb.Append($"\t\t\t\t\t$\"{namespacePostfix}/{tablenamePlural}/");
            if (httpVerb.ToLowerInvariant() != "post")
            {
                sb.Append($"{{{keylist}}}");
            }
            sb.AppendLine($"\", item);");

            sb.AppendLine("\t\t\t\treturn retVal;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        public string GenerateWebApiDataService(
            string iLoggingServiceNamespace,
            string serializationHelperNamespace,
            string webApiDataServiceInterfaceNamespace,
            string webApiDataServiceNamespace,
            string dtoNamespacePrefix,
            string dtoNamespace,
            string classNamespace,
            string namespacePostfix,
            string defaultCriteria,
            bool prependSchemaNameIndicator,
            IList<IEntityType> entityTypes,
            string baseNamespace)
        {
            var entitiesSortedByName = entityTypes.OrderBy(f => f.ClrType.Name);

            var sb = new StringBuilder();
            var sbGetAllPages = new StringBuilder();
            var sbGetOnePage = new StringBuilder();
            var sbGetByPK = new StringBuilder();
            var sbCreate = new StringBuilder();
            var sbUpdate = new StringBuilder();
            var sbDelete = new StringBuilder();

            sb.Append(GenerateHeader(
                iLoggingServiceNamespace: iLoggingServiceNamespace,
                serializationHelperNamespace: serializationHelperNamespace,
                webApiDataServiceInterfaceNamespace: webApiDataServiceInterfaceNamespace,
                webApiDataServiceNamespace: webApiDataServiceNamespace,
                dtoNamespacePrefix: dtoNamespacePrefix,
                dtoNamespace: dtoNamespace,
                namespacePostfix: namespacePostfix, baseNamespace: baseNamespace));

            sbGetAllPages.AppendLine($"{Environment.NewLine}\t\t#region GetAllPages{Environment.NewLine}");
            sbGetOnePage.AppendLine($"{Environment.NewLine}\t\t#region GetOnePage{Environment.NewLine}");
            sbGetByPK.AppendLine($"{Environment.NewLine}\t\t#region Get By PK{Environment.NewLine}");
            sbCreate.AppendLine($"{Environment.NewLine}\t\t#region Create{Environment.NewLine}");
            sbUpdate.AppendLine($"{Environment.NewLine}\t\t#region Update{Environment.NewLine}");
            sbDelete.AppendLine($"{Environment.NewLine}\t\t#region Delete{Environment.NewLine}");

            foreach (var entity in entitiesSortedByName)
            {
                string defaultCriteriaTableNameSubstituted = defaultCriteria?.Replace("[tablename]", Inflector.Humanize(entity.ClrType.Name));
                var defaultCriteriaAsTypeAndFieldNameList = GetDefaultCriteriaListForWebApi(defaultCriteriaTableNameSubstituted);
                string entityName = Inflector.Pascalize(entity.ClrType.Name);
                var criteriaFieldsThatExistInTable = FilterFieldTypeAndNamesByExistingColumns(entity, defaultCriteriaAsTypeAndFieldNameList);
                var criteriaTypeAndFieldNamesConcatenated = GetFieldTypeAndNamesConcatenated(criteriaFieldsThatExistInTable);
                var criteriaFieldNamesConcatenated = GetFieldNamesConcatenated(criteriaFieldsThatExistInTable);
                string keylist = string.Join("}/{", entity.GetKeys().Select(n => Inflector.ToLowerFirstCharacter(n.Properties[0].Name)));

                // Get All Pages
                // Begin method signature
                sbGetAllPages.AppendLine($"\t\tpublic async Task<IList<{dtoNamespacePrefix}.{entityName}>> GetAllPages{Inflector.Pluralize(entityName)}Async(");
                sbGetAllPages.Append($"\t\t\t");
                if (!string.IsNullOrEmpty(criteriaTypeAndFieldNamesConcatenated))
                {
                    sbGetAllPages.Append($"{criteriaTypeAndFieldNamesConcatenated}, ");
                }
                sbGetAllPages.AppendLine($"string sort = null)");
                // End method signature

                sbGetAllPages.AppendLine($"\t\t{{");
                BuildFilterCriteria(ref sbGetAllPages, criteriaFieldsThatExistInTable);

                sbGetAllPages.AppendLine($"\t\t\tIPageDataRequest pageDataRequest = new PageDataRequest(filterCriteria: filterCriteria, sort: sort, page: 1, pageSize: 100);");
                //sbGetAllPages.AppendLine($"\t\t\treturn await GetAllPageDataResultsAsync<{dtoNamespacePrefix}.{tableName}>(pageDataRequest, Get{table.NamePlural}Async);");
                sbGetAllPages.AppendLine($"\t\t\treturn await GetAllPageDataResultsAsync(pageDataRequest, Get{Inflector.Pluralize(entityName)}Async);");
                sbGetAllPages.AppendLine($"\t\t}}");
                sbGetAllPages.AppendLine(string.Empty);

                // Get One Page - Part 1
                sbGetOnePage.AppendLine($"\t\tpublic async Task<IHttpCallResultCGHT<IPageDataT<IList<{dtoNamespacePrefix}.{entityName}>>>> Get{Inflector.Pluralize(entityName)}Async(IPageDataRequest pageDataRequest)");
                sbGetOnePage.AppendLine($"\t\t{{");
                sbGetOnePage.AppendLine($"\t\t\tList<string> filter = BuildFilter(pageDataRequest.FilterCriteria);");
                sbGetOnePage.AppendLine($"\t\t\treturn await SerializationHelper.Instance.SerializeCallResultsGet<IList<{dtoNamespacePrefix}.{entityName}>>(Log, HttpClient, ");
                sbGetOnePage.AppendLine($"\t\t\t\t$\"{namespacePostfix}/{Inflector.Pluralize(entityName)}\", filter, page: pageDataRequest.Page, pageSize: pageDataRequest.PageSize);");
                sbGetOnePage.AppendLine($"\t\t}}");
                sbGetOnePage.AppendLine(string.Empty);

                // Get One Page - Part 2
                sbGetOnePage.AppendLine($"\t\tpublic async Task<IHttpCallResultCGHT<IPageDataT<IList<{dtoNamespacePrefix}.{entityName}>>>> Get{Inflector.Pluralize(entityName)}Async(");
                sbGetOnePage.Append($"\t\t\t");
                if (!string.IsNullOrEmpty(criteriaTypeAndFieldNamesConcatenated))
                {
                    sbGetOnePage.Append($"{criteriaTypeAndFieldNamesConcatenated}, ");
                }

                sbGetOnePage.AppendLine($"string sort = null, int page = 1, int pageSize = 100)");
                sbGetOnePage.AppendLine($"\t\t{{");
                BuildFilterCriteria(ref sbGetOnePage, criteriaFieldsThatExistInTable);

                sbGetOnePage.AppendLine(string.Empty);
                sbGetOnePage.AppendLine($"\t\t\tIPageDataRequest pageDataRequest = new PageDataRequest(filterCriteria: filterCriteria, sort: sort, page: page, pageSize: pageSize);");
                sbGetOnePage.AppendLine($"\t\t\treturn await Get{Inflector.Pluralize(entityName)}Async(pageDataRequest);");
                sbGetOnePage.AppendLine($"\t\t}}");
                sbGetOnePage.AppendLine(string.Empty);

                // Get By PK
                sbGetByPK.AppendLine($"\t\tpublic async Task<IHttpCallResultCGHT<{dtoNamespacePrefix}.{entityName}>> Get{entityName}Async({GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey())}, int numChildLevels)");
                sbGetByPK.AppendLine($"\t\t{{");

                sbGetByPK.AppendLine($"\t\t\tvar retVal = await SerializationHelper.Instance.SerializeCallResultsGet<{dtoNamespacePrefix}.{entityName}>(Log, HttpClient, $\"{namespacePostfix}/{Inflector.Pluralize(entityName)}/{{{keylist}}}?numChildLevels={{numChildLevels}}\");");
                sbGetByPK.AppendLine($"\t\t\treturn retVal;");
                sbGetByPK.AppendLine($"\t\t}}");
                sbGetByPK.AppendLine(string.Empty);

                // Create
                sbCreate.Append(GenerateSerializeCallFor(namespacePostfix: namespacePostfix, dtoNamespacePrefix: dtoNamespacePrefix,
                    httpVerb: "Post", prependSchemaNameIndicator: prependSchemaNameIndicator, entity: entity));

                // Update
                sbUpdate.Append(GenerateSerializeCallFor(namespacePostfix: namespacePostfix, dtoNamespacePrefix: dtoNamespacePrefix,
                    httpVerb: "Put", prependSchemaNameIndicator: prependSchemaNameIndicator, entity: entity));

                // Delete
                sbDelete.AppendLine($"\t\tpublic async Task<IHttpCallResultCGHT<{dtoNamespacePrefix}.{entityName}>> Delete{Inflector.Humanize(entity.ClrType.Name)}Async({GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey())})");
                sbDelete.AppendLine($"\t\t{{");

                sbDelete.AppendLine($"\t\t\tvar retVal = await SerializationHelper.Instance.SerializeCallResultsDelete<{dtoNamespacePrefix}.{entityName}>(Log, HttpClient, $\"{namespacePostfix}/{Inflector.Pluralize(entityName)}/{{{keylist}}}\");");
                sbDelete.AppendLine($"\t\t\treturn retVal;");
                sbDelete.AppendLine($"\t\t}}");
                sbDelete.AppendLine(string.Empty);
            }

            sbGetAllPages.AppendLine($"\t\t#endregion GetAllPages{Environment.NewLine}");
            sb.AppendLine(sbGetAllPages.ToString());

            sbGetOnePage.AppendLine($"\t\t#endregion GetOnePage{Environment.NewLine}");
            sb.AppendLine(sbGetOnePage.ToString());

            sbGetByPK.AppendLine($"\t\t#endregion Get By PK{Environment.NewLine}");
            sb.AppendLine(sbGetByPK.ToString());

            sbCreate.AppendLine($"\t\t#endregion Create{Environment.NewLine}");
            sb.AppendLine(sbCreate.ToString());

            sbUpdate.AppendLine($"\t\t#endregion Update{Environment.NewLine}");
            sb.AppendLine(sbUpdate.ToString());

            sbDelete.AppendLine($"\t\t#endregion Delete{Environment.NewLine}");
            sb.AppendLine(sbDelete.ToString());

            sb.Append(GenerateFooter());

            return sb.ToString();
        }

        private void BuildFilterCriteria(ref StringBuilder sb, List<Tuple<string, string, string, string>> criteriaFieldsThatExistInTable)
        {   // Each filter criterion should be in this format: guid?:primaryKeyId:IsEqualTo, bool?:isDeleted:IsEqualTo, DateTime?:[tablename]UpdDT:IsGreaterThan
            sb.AppendLine("\t\t\tList<IFilterCriterion> filterCriteria = new List<IFilterCriterion>();");
            foreach (var criterionThatExistsInTable in criteriaFieldsThatExistInTable)
            {
                string criterionVariableName = Inflector.ToLowerFirstCharacter(criterionThatExistsInTable.Item2);

                if (criterionThatExistsInTable.Item1.EndsWith("?"))
                {
                    sb.AppendLine($"\t\t\tif ({criterionVariableName}.HasValue)");
                    sb.AppendLine("\t\t\t{");
                }

                sb.AppendLine("\t\t\t\tIFilterCriterion filterCriterion = new FilterCriterion();");
                sb.AppendLine($"\t\t\t\tfilterCriterion.FieldName = \"{criterionThatExistsInTable.Item4}\";");
                sb.AppendLine($"\t\t\t\tfilterCriterion.FieldType = \"{criterionThatExistsInTable.Item1}\";");
                sb.AppendLine($"\t\t\t\tfilterCriterion.FilterOperator = \"{criterionThatExistsInTable.Item3}\";");
                sb.AppendLine($"\t\t\t\tfilterCriterion.Value = {criterionVariableName};");
                sb.AppendLine($"\t\t\t\tfilterCriteria.Add(filterCriterion);");

                if (criterionThatExistsInTable.Item1.EndsWith("?"))
                {
                    sb.AppendLine("\t\t\t}");
                }

                sb.AppendLine(string.Empty);
            }
        }
    }
}