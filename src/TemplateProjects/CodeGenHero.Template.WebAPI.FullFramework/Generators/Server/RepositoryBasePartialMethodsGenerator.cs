using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Server
{
    public class RepositoryBasePartialMethodsGenerator : BaseAPIFFGenerator
    {
        private const string EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION = " -- Excluded navigation property per configuration.";

        public RepositoryBasePartialMethodsGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateRepositoryBasePartialMethods(
            string namespacePostfix, string baseNamespace,
            string repositoryInterfaceNamespace,
            string efEntityNamespacePrefix,
            string repositoryEntitiesNamespace,
            string dbContextName,
            IList<IEntityType> EntityTypes,
            IList<IEntityNavigation> excludedNavigationProperties)
        {
            StringBuilder sb = new StringBuilder();

            var usings = new List<NamespaceItem>
            {
                new NamespaceItem("System"),
                new NamespaceItem("Microsoft.EntityFrameworkCore"),
                new NamespaceItem("System.Linq"),
                new NamespaceItem(efEntityNamespacePrefix, repositoryEntitiesNamespace),
                new NamespaceItem($"{repositoryInterfaceNamespace}"),
                new NamespaceItem("CodeGenHero.Repository")
            };

            sb.Append(GenerateHeader(usings, $"{baseNamespace}.Repository"));

            sb.AppendLine($"\tpublic abstract partial class {namespacePostfix}RepositoryBase : I{namespacePostfix}RepositoryCrud");
            sb.AppendLine($"\t{{");

            foreach (var entity in EntityTypes)
            {
                string entityName = entity.ClrType.Name;
                string tableNamePlural = Inflector.Pluralize(entity.ClrType.Name);
                string whereClause = WhereClause(objectInstancePrefix: null, indent: "\r\n\t\t\t\t\t\t", entity.Properties, entity.FindPrimaryKey(), useLowerForFirstCharOfPropertyName: true);

                //generates: x => x.AnnouncementId == item.AnnouncementId
                string whereClauseWithObjectInstancePrefix =
                    WhereClause(objectInstancePrefix: "item", indent: "\r\n\t\t\t\t\t\t", entity.Properties, entity.FindPrimaryKey(), useLowerForFirstCharOfPropertyName: false);

                //generates: int announcementId
                string methodParameterSignature = GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey());

                sb.Append(GenerateRunCustomLogicAfterInsert(dbContextName: dbContextName,
                    efEntityNamespacePrefix: efEntityNamespacePrefix,
                    excludedNavigationProperties: excludedNavigationProperties,
                    entity: entity,
                    entityName: entityName,
                    tableNamePlural: tableNamePlural,
                    whereClause: whereClause,
                    whereClauseWithObjectInstancePrefix: whereClauseWithObjectInstancePrefix,
                    methodParameterSignature: methodParameterSignature));

                sb.Append(GenerateRunCustomLogicAfterUpdate(dbContextName: dbContextName,
                    efEntityNamespacePrefix: efEntityNamespacePrefix,
                    excludedNavigationProperties: excludedNavigationProperties,
                    entity: entity,
                    entityName: entityName,
                    tableNamePlural: tableNamePlural,
                    whereClause: whereClause,
                    whereClauseWithObjectInstancePrefix: whereClauseWithObjectInstancePrefix,
                    methodParameterSignature: methodParameterSignature));

                sb.Append(GenerateRunCustomLogicOnGetQueryableByPK(dbContextName: dbContextName,
                    efEntityNamespacePrefix: efEntityNamespacePrefix,
                    excludedNavigationProperties: excludedNavigationProperties,
                    entity: entity,
                    entityName: entityName,
                    tableNamePlural: tableNamePlural,
                    whereClause: whereClause,
                    whereClauseWithObjectInstancePrefix: whereClauseWithObjectInstancePrefix,
                    methodParameterSignature: methodParameterSignature));

                sb.Append(GeneratRunCustomLogicOnGetEntityByPK(dbContextName: dbContextName,
                    efEntityNamespacePrefix: efEntityNamespacePrefix,
                    excludedNavigationProperties: excludedNavigationProperties,
                    entity: entity,
                    entityName: entityName,
                    tableNamePlural: tableNamePlural,
                    whereClause: whereClause,
                    whereClauseWithObjectInstancePrefix: whereClauseWithObjectInstancePrefix,
                    methodParameterSignature: methodParameterSignature));
            }

            sb.AppendLine($"\t}}{Environment.NewLine}}}{Environment.NewLine}");

            return sb.ToString();
        }

        /// <summary>
        /// sb.AppendLine($"\t\tpartial void RunCustomLogicAfterInsert_{tableName}({tableName} item, IRepositoryActionResult<{tableName}> result);{Environment.NewLine}");
        /// </summary>
        /// <param name="dbContextName"></param>
        /// <param name="efEntityNamespacePrefix"></param>
        /// <param name="excludedNavigationProperties"></param>
        /// <param name="table"></param>
        /// <param name="entityName"></param>
        /// <param name="tableNamePlural"></param>
        /// <param name="whereClause"></param>
        /// <param name="whereClauseWithObjectInstancePrefix"></param>
        /// <param name="methodParameterSignature"></param>
        /// <returns></returns>
        private string GenerateRunCustomLogicAfterInsert(string dbContextName, string efEntityNamespacePrefix, IList<IEntityNavigation> excludedNavigationProperties,
            IEntityType entity, string entityName, string tableNamePlural, string whereClause, string whereClauseWithObjectInstancePrefix, string methodParameterSignature)
        {
            return null;
        }

        /// <summary>
        /// sb.AppendLine($"\t\tpartial void RunCustomLogicAfterUpdate_{tableName}({tableName} newItem, {tableName} oldItem, IRepositoryActionResult<{tableName}> result);{Environment.NewLine}");
        /// </summary>
        /// <param name="dbContextName"></param>
        /// <param name="efEntityNamespacePrefix"></param>
        /// <param name="excludedNavigationProperties"></param>
        /// <param name="table"></param>
        /// <param name="entityName"></param>
        /// <param name="tableNamePlural"></param>
        /// <param name="whereClause"></param>
        /// <param name="whereClauseWithObjectInstancePrefix"></param>
        /// <param name="methodParameterSignature"></param>
        /// <returns></returns>
        private string GenerateRunCustomLogicAfterUpdate(string dbContextName, string efEntityNamespacePrefix, IList<IEntityNavigation> excludedNavigationProperties,
            IEntityType entity, string entityName, string tableNamePlural, string whereClause, string whereClauseWithObjectInstancePrefix, string methodParameterSignature)
        {
            return null;
        }

        /// <summary>
        /// sb.AppendLine($"\t\tpartial void RunCustomLogicOnGetQueryableByPK_{tableName}(ref IQueryable<{tableName}> qryItem, int numChildLevels);{Environment.NewLine}");
        /// </summary>
        /// <param name="dbContextName"></param>
        /// <param name="efEntityNamespacePrefix"></param>
        /// <param name="excludedNavigationProperties"></param>
        /// <param name="entity"></param>
        /// <param name="entityName"></param>
        /// <param name="tableNamePlural"></param>
        /// <param name="whereClause"></param>
        /// <param name="whereClauseWithObjectInstancePrefix"></param>
        /// <param name="methodParameterSignature"></param>
        /// <returns></returns>
        private string GenerateRunCustomLogicOnGetQueryableByPK(string dbContextName,
            string efEntityNamespacePrefix, IList<IEntityNavigation> excludedNavigationProperties,
             IEntityType entity, string entityName, string tableNamePlural,
            string whereClause, string whereClauseWithObjectInstancePrefix, string methodParameterSignature)
        {
            StringBuilder sb = new StringBuilder();

            bool isAllForeignKeysExcluded = entity.Navigations.All(x => IsEntityInExcludedReferenceNavigionationProperties(excludedNavigationProperties, entityName));

            sb.AppendLine($"{Environment.NewLine}\t\t/// <summary>");
            sb.AppendLine("\t\t/// Custom logic that is generally used to include related entities to return with the parent entity that was requested.");
            sb.AppendLine("\t\t/// </summary>");
            sb.AppendLine("\t\t/// <param name=\"qryItem\"></param>");
            sb.AppendLine("\t\t/// <param name=\"id\"></param>");
            sb.AppendLine("\t\t/// <param name=\"numChildLevels\"></param>");
            sb.AppendLine($"\t\t partial void RunCustomLogicOnGetQueryableByPK_{entityName}(ref IQueryable<{efEntityNamespacePrefix}.{entityName}> qryItem, {methodParameterSignature}, int numChildLevels)");
            sb.AppendLine("\t\t {");
            sb.AppendLine($"\t\t\t if (numChildLevels > 0)");
            sb.AppendLine($"\t\t\t {{");

            bool firstItemInList = true;
            for (int i = 0; i < entity.Navigations.Count; i++)
            {
                var item = entity.Navigations[i];
                bool isLastItemInList = i == entity.Navigations.Count - 1;
                bool excludeCircularReferenceNavigationIndicator = IsEntityInExcludedReferenceNavigionationProperties(excludedNavigationProperties, entityName);
                var remainingFkItemsInList = new List<INavigation>();
                for (int j = i + 1; j < entity.Navigations.Count; j++)
                {
                    remainingFkItemsInList.Add(entity.Navigations[j]);
                }
                bool isAllRemainingForeignKeysExcluded = remainingFkItemsInList.All(x => IsEntityInExcludedReferenceNavigionationProperties(excludedNavigationProperties, entityName));

                sb.Append($"\t\t\t\t ");

                if (firstItemInList)
                {
                    sb.Append($"qryItem = qryItem");

                    // If all foreign keys are excluded or - it is the first AND ONLY item in the list and that first item is excluded, then we need to append a semi-colon.
                    if (isAllForeignKeysExcluded
                        || (entity.Navigations.Count == 1 && excludeCircularReferenceNavigationIndicator))
                    {
                        sb.AppendLine($";");
                    }
                    else
                    {
                        sb.AppendLine(string.Empty);
                    }

                    sb.Append($"\t\t\t\t ");    // Tabs for next line
                    firstItemInList = false;
                }

                if (excludeCircularReferenceNavigationIndicator)
                {   // Include the line, but add extra comment marker
                    sb.Append($"// ");
                }

                sb.Append($".Include(x => x.{item.Name}).AsNoTracking()");

                //// If it is either the last item in the list or the penultimate item in the list, but the next one (the last item) is excluded (commented out), then we need to add a semi-colon.
                //if (isPenultimateItemInList)
                //{
                //	ForeignKey lastItemInList = fklist[i + 1]; // Get the last item in the list.
                //											   // If the current item is not excluded, but the last item will be excluded, then we need to append a semi-colon to this line.
                //	if (!excludeCircularReferenceNavigationIndicator
                //		&& lastItemInList.ExcludeCircularReferenceNavigationIndicator(excludedNavigationProperties))
                //	{
                //		isForceAppendEndOfLineSemiColon = true;
                //	}
                //}

                if (isLastItemInList || isAllRemainingForeignKeysExcluded)
                {
                    sb.Append(";");
                    if (excludeCircularReferenceNavigationIndicator)
                    {
                        sb.Append(EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION);
                    }

                    sb.AppendLine(string.Empty);
                }
                else
                {   // More items to come.
                    if (excludeCircularReferenceNavigationIndicator)
                    {
                        sb.Append(EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION);
                    }

                    sb.AppendLine(string.Empty);
                }
            }

            sb.AppendLine($"\t\t\t }}");
            sb.AppendLine($"\t\t }}{Environment.NewLine}");

            return sb.ToString();
        }

        /// <summary>
        /// sb.AppendLine($"\t\tpartial void RunCustomLogicOnGetEntityByPK_{tableName}(ref {tableName} dbItem, int numChildLevels);{Environment.NewLine}");
        /// </summary>
        /// <param name="dbContextName"></param>
        /// <param name="efEntityNamespacePrefix"></param>
        /// <param name="excludedNavigationProperties"></param>
        /// <param name="table"></param>
        /// <param name="entityName"></param>
        /// <param name="tableNamePlural"></param>
        /// <param name="whereClause"></param>
        /// <param name="whereClauseWithObjectInstancePrefix"></param>
        /// <param name="methodParameterSignature"></param>
        /// <returns></returns>
        private string GeneratRunCustomLogicOnGetEntityByPK(string dbContextName, string efEntityNamespacePrefix, IList<IEntityNavigation> excludedNavigationProperties,
             IEntityType entity, string entityName, string tableNamePlural, string whereClause, string whereClauseWithObjectInstancePrefix, string methodParameterSignature)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{Environment.NewLine}\t\t///// <summary>");
            sb.AppendLine("\t\t///// A sample implementation of custom logic used to either manipulate a DTO item or include related entities.");
            sb.AppendLine("\t\t///// </summary>");
            sb.AppendLine("\t\t///// <param name=\"dbItem\"></param>");
            sb.AppendLine("\t\t///// <param name=\"id\"></param>");
            sb.AppendLine("\t\t///// <param name=\"numChildLevels\"></param>");
            sb.AppendLine($"\t\t// partial void RunCustomLogicOnGetEntityByPK_{entityName}(ref {efEntityNamespacePrefix}.{entityName} dbItem, {methodParameterSignature}, int numChildLevels)");
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

            return sb.ToString();
        }
    }
}