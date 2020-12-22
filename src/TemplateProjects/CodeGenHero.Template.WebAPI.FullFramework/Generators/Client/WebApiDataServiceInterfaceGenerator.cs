using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Client
{
    public class WebApiDataServiceInterfaceGenerator : BaseAPIFFGenerator
    {
        public WebApiDataServiceInterfaceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateIWebApiDataServiceFooterString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }

        public string GenerateWebApiDataServiceInterface(string dtoNamespace,
            string defaultCriteria, string baseNamespace,
            string webApiDataServiceInterfaceNamespace, string webApiDataServiceInterfaceClassName,
            bool prependSchemaNameIndicator,
            IList<IEntityType> EntityTypes)
        {
            var sb = new StringBuilder();
            string dtoNamespacePrefix = "xDTO";

            string generatedCode = GenerateWebApiDataServiceInterfaceHeader(
                dtoNamespacePrefix: dtoNamespacePrefix,
                dtoNamespace: dtoNamespace,
                classNamespace: webApiDataServiceInterfaceNamespace,
                className: webApiDataServiceInterfaceClassName,
                basenamespace: baseNamespace
            );
            sb.Append(generatedCode);

            generatedCode = GenerateWebApiDataServiceInterfaceBody(
                dtoNamespacePrefix: dtoNamespacePrefix,
                defaultCriteria: defaultCriteria,
                prependSchemaNameIndicator: prependSchemaNameIndicator,
                EntityTypes: EntityTypes
            );
            sb.Append(generatedCode);

            generatedCode = GenerateIWebApiDataServiceFooterString();
            sb.Append(generatedCode);

            return sb.ToString();
        }

        public string GenerateWebApiDataServiceInterfaceBody(
            string dtoNamespacePrefix, string defaultCriteria, bool prependSchemaNameIndicator,
            IList<IEntityType> EntityTypes)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbGetAllPages = new StringBuilder();
            StringBuilder sbGetPageData = new StringBuilder();
            StringBuilder sbGetByPK = new StringBuilder();
            StringBuilder sbCreate = new StringBuilder();
            StringBuilder sbUpdate = new StringBuilder();
            StringBuilder sbDelete = new StringBuilder();

            sbGetAllPages.AppendLine($"\t\t#region GetAllPages{Environment.NewLine}");
            sbGetPageData.AppendLine($"\t\t#region GetPageData{Environment.NewLine}");
            sbGetByPK.AppendLine($"\t\t#region Get By PK{Environment.NewLine}");
            sbCreate.AppendLine($"\t\t#region Create{Environment.NewLine}");
            sbUpdate.AppendLine($"\t\t#region Update{Environment.NewLine}");
            sbDelete.AppendLine($"\t\t#region Delete{Environment.NewLine}");

            foreach (var entity in EntityTypes)
            {
                string defaultCriteriaTableNameSubstituted = defaultCriteria.Replace("[tablename]", Inflector.Humanize(entity.ClrType.Name));
                var defaultCriteriaAsTypeAndFieldNameList = GetDefaultCriteriaListForWebApi(defaultCriteriaTableNameSubstituted);
                string tableName = Inflector.Pascalize(entity.ClrType.Name);
                var criteriaFieldsThatExistInTable = FilterFieldTypeAndNamesByExistingColumns(entity, defaultCriteriaAsTypeAndFieldNameList);
                var criteriaTypeAndFieldNamesConcatenated = GetFieldTypeAndNamesConcatenated(criteriaFieldsThatExistInTable);
                string keylist = string.Join("}/{", entity.GetKeys().Select(k => k.Properties[0].Name));

                // Get All Pages
                sbGetAllPages.Append($"\t\tTask<IList<{dtoNamespacePrefix}.{tableName}>> GetAllPages{Inflector.Pluralize(entity.ClrType.Name)}Async(");
                if (!string.IsNullOrEmpty(criteriaTypeAndFieldNamesConcatenated))
                {
                    sbGetAllPages.Append($"{criteriaTypeAndFieldNamesConcatenated}, ");
                }
                sbGetAllPages.AppendLine($"string sort = null);{Environment.NewLine}");

                // Get One Page - Part 1
                sbGetPageData.AppendLine($"\t\tTask<IHttpCallResultCGHT<IPageDataT<IList<{dtoNamespacePrefix}.{tableName}>>>> Get{Inflector.Pluralize(entity.ClrType.Name)}Async(IPageDataRequest pageDataRequest);{Environment.NewLine}");

                // Get One Page - Part 2
                sbGetPageData.Append($"\t\tTask<IHttpCallResultCGHT<IPageDataT<IList<{dtoNamespacePrefix}.{tableName}>>>> Get{Inflector.Pluralize(entity.ClrType.Name)}Async(");
                if (!string.IsNullOrEmpty(criteriaTypeAndFieldNamesConcatenated))
                {
                    sbGetPageData.Append($"{criteriaTypeAndFieldNamesConcatenated}, ");
                }

                sbGetPageData.AppendLine($"string sort = null, int page = 1, int pageSize = 100);{Environment.NewLine}");

                sbGetByPK.AppendLine($"\t\tTask<IHttpCallResultCGHT<{dtoNamespacePrefix}.{tableName}>> Get{Inflector.Humanize(entity.ClrType.Name)}Async({GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey())}, int numChildLevels);{Environment.NewLine}");
                sbCreate.AppendLine($"\t\tTask<IHttpCallResultCGHT<{dtoNamespacePrefix}.{tableName}>> Create{Inflector.Humanize(entity.ClrType.Name)}Async({dtoNamespacePrefix}.{tableName} item);{Environment.NewLine}");
                sbUpdate.AppendLine($"\t\tTask<IHttpCallResultCGHT<{dtoNamespacePrefix}.{tableName}>> Update{Inflector.Humanize(entity.ClrType.Name)}Async({dtoNamespacePrefix}.{tableName} item);{Environment.NewLine}");
                sbDelete.AppendLine($"\t\tTask<IHttpCallResultCGHT<{dtoNamespacePrefix}.{tableName}>> Delete{Inflector.Humanize(entity.ClrType.Name)}Async({GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey())});{Environment.NewLine}");
            }

            sbGetAllPages.AppendLine($"{Environment.NewLine}\t\t#endregion GetAllPages{Environment.NewLine}");
            sb.AppendLine(sbGetAllPages.ToString());

            sbGetPageData.AppendLine($"{Environment.NewLine}\t\t#endregion GetPageData{Environment.NewLine}");
            sb.AppendLine(sbGetPageData.ToString());

            sbGetByPK.AppendLine($"{Environment.NewLine}\t\t#endregion Get By PK{Environment.NewLine}");
            sb.AppendLine(sbGetByPK.ToString());

            sbCreate.AppendLine($"{Environment.NewLine}\t\t#endregion Create{Environment.NewLine}");
            sb.AppendLine(sbCreate.ToString());

            sbUpdate.AppendLine($"{Environment.NewLine}\t\t#endregion Update{Environment.NewLine}");
            sb.AppendLine(sbUpdate.ToString());

            sbDelete.AppendLine($"{Environment.NewLine}\t\t#endregion Delete{Environment.NewLine}");
            sb.AppendLine(sbDelete.ToString());

            return sb.ToString();
        }

        public string GenerateWebApiDataServiceInterfaceHeader(string dtoNamespacePrefix, string dtoNamespace,
            string classNamespace, string className, string basenamespace)
        {
            StringBuilder sb = new StringBuilder();

            List<NamespaceItem> usingNamespaceItems = new List<NamespaceItem>();

            usingNamespaceItems.Insert(0, new NamespaceItem() { Namespace = "System", NamespacePrefix = null, Name = null });
            usingNamespaceItems.Insert(1, new NamespaceItem() { Namespace = "System.Collections.Generic", NamespacePrefix = null, Name = null });
            usingNamespaceItems.Insert(2, new NamespaceItem() { Namespace = "System.Threading.Tasks", NamespacePrefix = null, Name = null });
            usingNamespaceItems.Insert(3, new NamespaceItem() { Namespace = "CodeGenHero.DataService", NamespacePrefix = null, Name = null });
            usingNamespaceItems.Insert(4, new NamespaceItem() { Namespace = dtoNamespace, NamespacePrefix = dtoNamespacePrefix });

            sb.Append(GenerateHeader(usingNamespaceItems, classNamespace));
            sb.AppendLine($"\tpublic partial interface {className} : IWebApiDataServiceBase");
            sb.AppendLine($"\t{{");

            return sb.ToString();
        }
    }
}