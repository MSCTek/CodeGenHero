using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Server
{
    public class WebApiControllerPartialMethodsGenerator : BaseAPIFFGenerator
    {
        private const string EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION = " -- Excluded navigation property per configuration.";

        public WebApiControllerPartialMethodsGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateWebApiControllerPartialMethods(
            string namespacePostfix,
            string classNamespace,
            string baseControllerName,
            string efEntityNamespacePrefix,
            string efEntityNamespace,
            string dbContextName,
            IEntityType entity,
            IList<IEntityNavigation> excludedEntityNavigations,
            bool allowUpsertDuringPut)
        {
            var className = $"{Inflector.Pluralize(entity.ClrType.Name)}{namespacePostfix}Controller";

            var sb = new IndentingStringBuilder();

            var usings = new List<NamespaceItem>
            {
                new NamespaceItem("CodeGenHero.Repository"),
                //new NamespaceItem("Microsoft.EntityFrameworkCore"),
                //new NamespaceItem("System.Linq"),
                new NamespaceItem(efEntityNamespacePrefix, efEntityNamespace)
            };

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic partial class {className} : {baseControllerName}");
            sb.AppendLine("\t{");

            sb.Append(GeneratePartialMethods(dbContextName: dbContextName, entity: entity,
                efEntityNamespacePrefix: efEntityNamespacePrefix, excludedNavigationProperties: excludedEntityNavigations,
                allowUpsertDuringPut: allowUpsertDuringPut));

            return sb.ToString();
        }

        private string GeneratePartialMethods(string dbContextName, IEntityType entity, string efEntityNamespacePrefix,
                                    IList<IEntityNavigation> excludedNavigationProperties, bool allowUpsertDuringPut)
        {
            StringBuilder sb = new StringBuilder();

            string tablenameSchema = Inflector.Humanize(entity.ClrType.Name);
            string tablenameNoSchema = tablenameSchema;
            string tablenamePlural = Inflector.Pluralize(entity.ClrType.Name);
            string entityTypeName = $"{efEntityNamespacePrefix}.{tablenameSchema}";

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t//partial void RunCustomLogicAfterInsert(ref {entityTypeName} newDBItem, ref IRepositoryActionResult<{entityTypeName}> result) {{}}");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\t//partial void RunCustomLogicAfterUpdatePatch(ref {entityTypeName} updatedDBItem, ref IRepositoryActionResult<{entityTypeName}> result) {{}}");
            sb.AppendLine(string.Empty);

            if (allowUpsertDuringPut)
            {
                sb.AppendLine($"\t\tpartial void RunCustomLogicAfterUpdatePut(ref {entityTypeName} updatedDBItem, ref IRepositoryActionResult<{entityTypeName}> result)");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\tif (result.Status == Enums.RepositoryActionStatus.NotFound)");
                sb.AppendLine($"\t\t\t{{\t// An update/PUT was attempted when it should have been a create/POST.");
                sb.AppendLine($"\t\t\t\tvar localDBItem = updatedDBItem;");
                sb.AppendLine($"\t\t\t\tvar insertResult = RunSync<IRepositoryActionResult<{entityTypeName}>>(() => Repo.InsertAsync(localDBItem));");
                sb.AppendLine($"\t\t\t\tif (insertResult.Status == Enums.RepositoryActionStatus.Created)");
                sb.AppendLine($"\t\t\t\t{{   // Insert worked");
                sb.AppendLine($"\t\t\t\t\tresult = new RepositoryActionResult<{entityTypeName}>(insertResult.Entity, Enums.RepositoryActionStatus.Updated);");
                sb.AppendLine($"\t\t\t\t}}");
                sb.AppendLine($"\t\t\t}}");
                sb.AppendLine($"\t\t}}");
            }
            else
            {
                sb.AppendLine($"\t\t//partial void RunCustomLogicAfterUpdatePut(ref {entityTypeName} updatedDBItem, ref IRepositoryActionResult<{entityTypeName}> result) {{}}");
            }

            string signatureWithFieldTypes = GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey());
            string signatureWithoutFieldTypes = GetSignatureWithoutFieldTypes(string.Empty, entity.FindPrimaryKey(), lowercasePkNameFirstChar: true);

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t// partial void RunCustomLogicBeforeUpdatePut(ref {entityTypeName} updatedDBItem, {signatureWithFieldTypes})");
            sb.AppendLine($"\t\t// {{");
            sb.AppendLine($"\t\t// \tvar existingDBItem = Utils.AsyncHelper.RunSync<{entityTypeName}>(() => Repo.Get_{tablenameSchema}Async({signatureWithoutFieldTypes}, 1));");
            sb.AppendLine($"\t\t// \tif (existingDBItem != null)");
            sb.AppendLine($"\t\t// \t{{\t// Do not allow the user to change the \"MySpecialField\" value.");
            sb.AppendLine($"\t\t// \t\tupdatedDBItem.MySpecialField = existingDBItem.MySpecialField;");
            sb.AppendLine($"\t\t// \t}}");
            sb.AppendLine($"\t\t// }}");

            sb.AppendLine(string.Empty);
            sb.AppendLine("\t\t///// <summary>");
            sb.AppendLine("\t\t///// A sample implementation of custom logic used to either manipulate a DTO item or include related entities.");
            sb.AppendLine("\t\t///// </summary>");
            sb.AppendLine("\t\t///// <param name=\"dbItem\"></param>");
            sb.AppendLine("\t\t///// <param name=\"id\"></param>");
            sb.AppendLine("\t\t///// <param name=\"numChildLevels\"></param>");
            sb.AppendLine($"\t\t// partial void RunCustomLogicOnGetEntityByPK(ref {entityTypeName} dbItem, {GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey())}, int numChildLevels)");
            sb.AppendLine("\t\t// {");
            sb.AppendLine($"\t\t\t// if (numChildLevels > 1)");
            sb.AppendLine($"\t\t\t// {{");
            sb.AppendLine($"\t\t\t\t// int[] orderLineItemIds = dbItem.OrderLineItems.Select(x => x.OrderLineItemId).ToArray();");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t\t// var lineItemDiscounts = Repo.{dbContextName}.OrderLineItemDiscounts.Where(x => orderLineItemIds.Contains(x.OrderLineItemId)).ToList();");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t\t// foreach (var lineItemDiscount in lineItemDiscounts)");
            sb.AppendLine($"\t\t\t\t// {{ // Find the match and add the item to it.");
            sb.AppendLine($"\t\t\t\t\t// var orderLineItem = dbItem.OrderLineItems.Where(x => x.OrderLineItemId == lineItemDiscount.OrderLineItemId).FirstOrDefault();");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t\t\t// if (orderLineItem == null)");
            sb.AppendLine($"\t\t\t\t\t// {{");
            sb.AppendLine($"\t\t\t\t\t\t// throw new Microsoft.EntityFrameworkCore.ObjectNotFoundException($\"Unable to locate matching OrderLineItem record for {{lineItemDiscount.OrderLineItemId}}.\"");
            sb.AppendLine($"\t\t\t\t\t// }}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t\t\t\t// orderLineItem.LineItemDiscounts.Add(lineItemDiscount);");
            sb.AppendLine($"\t\t\t\t// }}");
            sb.AppendLine($"\t\t\t// }}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("\t\t// }");

            sb.AppendLine(string.Empty);
            sb.AppendLine("\t\t///// <summary>");
            sb.AppendLine("\t\t///// A sample implementation of custom logic used to filter on a field that exists in a related, parent, table.");
            sb.AppendLine("\t\t///// </summary>");
            sb.AppendLine("\t\t///// <param name=\"dbItems\"></param>");
            sb.AppendLine("\t\t///// <param name=\"filterList\"></param>");
            sb.AppendLine($"\t\t//partial void RunCustomLogicAfterGetQueryableList(ref IQueryable<{efEntityNamespacePrefix}.{tablenameNoSchema}> dbItems, ref List<string> filterList)");
            sb.AppendLine("\t\t//{");
            sb.AppendLine($"\t\t//\tvar queryableFilters = filterList.ToQueryableFilter();");
            sb.AppendLine($"\t\t//\tvar myFilterCriterion = queryableFilters.Where(y => y.Member.ToLowerInvariant() == \"<myFieldName>\").FirstOrDefault(); // Examine the incoming filter for the presence of a field name which does not exist on the target entity.");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\t//\tif (myFilterCriterion != null)");
            sb.AppendLine($"\t\t//\t{{   // myFieldName is a criterion that has to be evaluated at a level other than our target entity.");
            sb.AppendLine("\t\t//\t\tdbItems = dbItems.Include(x => x.myFKRelatedEntity).Where(x => x.myFKRelatedEntity.myFieldName == new Guid(myFilterCriterion.Value));");
            sb.AppendLine($"\t\t//\t\tqueryableFilters.Remove(myFilterCriterion);  // The evaluated criterion needs to be removed from the list of filters before we invoke the ApplyFilter() extension method.");
            sb.AppendLine($"\t\t//\t\tfilterList = queryableFilters.ToQueryableStringList();");
            sb.AppendLine("\t\t//\t}");
            sb.AppendLine("\t\t//}");

            sb.AppendLine($"\t}}\r\n}}");

            return sb.ToString();
        }
    }
}