using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.Models
{
    public abstract class BaseGenerator
    {
        public BaseGenerator(ICodeGenHeroInflector inflector)
        {
            Inflector = inflector;
        }

        protected virtual ICodeGenHeroInflector Inflector { get; set; }

        public static string GenerateHeader(List<NamespaceItem> usings, string classnamespace)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var usingItem in usings)
            {
                sb.Append($"using ");
                if (usingItem.NamespacePrefix != null && !string.IsNullOrEmpty(usingItem.NamespacePrefix.Trim()))
                    sb.Append($"{usingItem.NamespacePrefix.Trim()} = ");

                sb.AppendLine($"{usingItem.Namespace};");
            }

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {classnamespace}");
            sb.AppendLine($"{{");

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string that defines a where clause for Linq, based on the primary keys. Assumes primary keys are all simple types and not strings
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public virtual string BuildWhereClause(string linqName, IEntityType entityType)
        {
            string retVal = string.Empty;

            foreach (KeyValuePair<IList<IProperty>, IKey> entry in entityType.Keys)
            {
                foreach (var item in entry.Key)
                {
                    retVal += $"&& {linqName}.{item.Name} == {Inflector.Camelize(item.Name)}";
                }
            }
            // Lop off the initial "&& "
            if (retVal.Length > 3)
            {
                retVal = retVal.Substring(3);
            }
            return retVal;
        }

        public virtual bool ContainsAuditEditFields(IList<IProperty> props)
        {
            bool retVal = false;

            List<string> colNames = props.Select(x => x.Name.ToLowerInvariant()).ToList();
            if (colNames.Any(x => x.Equals("createddate"))
                && colNames.Any(x => x.Equals("createduserid"))
                && colNames.Any(x => x.Equals("updateddate"))
                && colNames.Any(x => x.Equals("updateduserid"))
                && colNames.Any(x => x.Equals("isdeleted"))
                )
            {
                retVal = true;
            }

            return retVal;
        }

        public virtual string ConvertToSimpleType(string propertyType)
        {
            if (propertyType.Equals("Int32") || propertyType.Equals("System.Int32"))
                return "int";
            if (propertyType.Equals("Int32?") || propertyType.Equals("System.Int32?"))
                return "int?";
            if (propertyType.Equals("DateTime") || propertyType.Equals("System.DateTime"))
                return "DateTime";
            if (propertyType.Equals("DateTime?") || propertyType.Equals("System.DateTime?"))
                return "DateTime?";
            if (propertyType.Equals("String"))
                return "string";
            if (propertyType.Equals("Boolean") || propertyType.Equals("System.Boolean"))
                return "bool";
            if (propertyType.Equals("Boolean?") || propertyType.Equals("System.Boolean?"))
                return "bool?";

            // If we reach here, then it wasn't one of the above simple types.
            return propertyType;
        }

        public virtual bool EntityNavigationsContainsNavigationName(
            IList<IEntityNavigation> entityNavigations,
            IEntityType entityType,
            string navigationName)
        {
            bool retVal = false;
            if (entityNavigations == null)
            {
                return retVal;
            }

            var filteredEntityNavigations = entityNavigations.Where(x =>
                x.EntityType.ClrType.FullName.Equals(entityType.ClrType.FullName, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            foreach (var entityNavigation in filteredEntityNavigations)
            {
                if (entityNavigation.Navigation != null
                    && entityNavigation.Navigation.Name.Equals(navigationName, StringComparison.InvariantCultureIgnoreCase))
                {
                    retVal = true;
                    break;
                }
            }

            return retVal;
        }

        public virtual List<Tuple<string, string, string, string>> FilterFieldTypeAndNamesByExistingColumns(IEntityType entityType, List<Tuple<string, string, string, string>> defaultCriteriaAsTypeAndFieldNameList)
        {
            var retVal = new List<Tuple<string, string, string, string>>();

            if (defaultCriteriaAsTypeAndFieldNameList == null || defaultCriteriaAsTypeAndFieldNameList.Count == 0)
            {
                return retVal;
            }

            foreach (var criteriaField in defaultCriteriaAsTypeAndFieldNameList)
            {
                var entityName = criteriaField.Item4.ToLower();
                var prop = entityType.Properties.Where(x => x.Value.Name.ToLower() == entityName);

                if (prop.Any())
                {
                    retVal.Add(criteriaField);
                }
            }

            return retVal;
        }

        public virtual string GenerateFooter()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }

        public virtual string GenerateUsings(List<string> usings)
        {
            if (usings == null || usings.Count == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (string s in usings)
            {
                sb.AppendLine(s);
            }

            return sb.ToString();
        }

        public virtual string GenerateUsings(IList<NamespaceItem> namespaceItems)
        {
            if (namespaceItems == null || namespaceItems.Count == 0)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            foreach (var namespaceItem in namespaceItems)
            {
                stringBuilder.Append($"using ");
                if (namespaceItem.NamespacePrefix != null && !string.IsNullOrEmpty(namespaceItem.NamespacePrefix.Trim()))
                    stringBuilder.Append($"{namespaceItem.NamespacePrefix.Trim()} = ");

                stringBuilder.AppendLine($"{namespaceItem.Namespace};");
            }

            return stringBuilder.ToString();
        }

        // Given the EF property, figure out what the actual property type is
        public virtual string GetCType(IProperty property)
        {
            string propertyType = property.ClrType.Name;
            if (propertyType.Contains("`1") && property.ClrType.FullName.Contains("[["))
            {
                // The property is more complicated than just the type.  Probably ICollection or DateTime
                // DateTime comes out as "System.Nullable`1[[System.DateTime, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]",
                // The type we want is inside the first '[['
                string fullname = property.ClrType.FullName;
                string realType = fullname.Substring(fullname.IndexOf("[[") + 2); // Grab the string after the [[
                propertyType = realType.Substring(0, realType.IndexOf(",")); // Chop off the part as of the first comma
            }
            //propertyType = ConvertToSimpleType(propertyType);
            string nullable = property.IsNullable ? "?" : "";
            nullable = (propertyType.ToLower().Equals("string") || propertyType.ToLower().Equals("byte[]")) ? "" : nullable; // If the propertyType is string, it can't be marked nullable.
            string retVal = $"{propertyType}{nullable}";

            return retVal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="defaultCriteria">In the format of [field type]:[field name]: [operator]
        /// </param>
        /// <remarks>
        /// Sample: int?:surveyInstanceId:=, int?:statusTypeId:=, DateTime?:[tablename]UpdDT:>
        /// </remarks>
        /// <returns></returns>
        public virtual List<Tuple<string, string, string, string>> GetDefaultCriteriaListForWebApi(string defaultCriteria)
        {
            var retVal = new List<Tuple<string, string, string, string>>();

            if (!string.IsNullOrEmpty(defaultCriteria))
            {   // Expected format of: "int?:surveyInstanceId:IsEqualTo, int?:statusTypeId:IsEqualTo, DateTime?:[tablename]UpdDT:IsGreaterThan:minUpdatedDate"
                // Tranlation of each item: [Parameter Type]:[Parameter Variable Name]:[Operator]:[Database Field/EF Property Name]
                // Note, the last token, "[Database Field/EF Property Name]" is optional.  If it is not provided, then the "[Parameter Variable Name]" is used in its place.
                var defaultCriteriaSplit = defaultCriteria.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < defaultCriteriaSplit.Count(); i++)
                {   // Construct default criteria for GetAllPages and GetPageData method signatures below.
                    var typeAndFieldNameSplit = defaultCriteriaSplit[i].Trim().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (typeAndFieldNameSplit.Length == 3)
                    {
                        retVal.Add(new Tuple<string, string, string, string>(
                            typeAndFieldNameSplit[0].Trim(), // [Parameter Type]
                            typeAndFieldNameSplit[1].Trim(), // [Parameter Variable Name]
                            typeAndFieldNameSplit[2].Trim(), // [Operator]
                            typeAndFieldNameSplit[1].Trim()  // [Database Field/EF Property Name]
                            ));
                    }
                    else if (typeAndFieldNameSplit.Length == 4)
                    {
                        retVal.Add(new Tuple<string, string, string, string>(
                            typeAndFieldNameSplit[0].Trim(), // [Parameter Type]
                            typeAndFieldNameSplit[1].Trim(), // [Parameter Variable Name]
                            typeAndFieldNameSplit[2].Trim(), // [Operator]
                            typeAndFieldNameSplit[3].Trim()  // [Database Field/EF Property Name]
                            ));
                    }
                    else
                    {
                        throw new ArgumentException($"Expected a length of three or four when parsing the default criteria and encountered a length of {typeAndFieldNameSplit.Length}");
                    }
                }
            }

            return retVal;
        }

        public virtual string GetFieldNamesConcatenated(List<Tuple<string, string, string, string>> criteriaFieldType_Name_Operator,
            string typeAndNameConcatenationString = " ", string itemAggregationString = ", ")
        {
            if (criteriaFieldType_Name_Operator == null || criteriaFieldType_Name_Operator.Count == 0)
                return null;

            var retVal = criteriaFieldType_Name_Operator
                .Select(x => Inflector.ToLowerFirstCharacter(x.Item2))
                .Aggregate((i, j) => i + ", " + j);

            return retVal;
        }

        public virtual string GetFieldTypeAndNamesConcatenated(
                                    List<Tuple<string, string, string, string>> criteriaFieldType_Name_Operators,
            string typeAndNameConcatenationString = " ",
            string itemAggregationString = ", ")
        {
            if (criteriaFieldType_Name_Operators == null || criteriaFieldType_Name_Operators.Count == 0)
                return null;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < criteriaFieldType_Name_Operators.Count; i++)
            {
                var criteriaFieldType_Name_Operator = criteriaFieldType_Name_Operators[i];
                sb.Append(string.Concat(criteriaFieldType_Name_Operator.Item1, typeAndNameConcatenationString, Inflector.ToLowerFirstCharacter(criteriaFieldType_Name_Operator.Item2)));

                if (!string.IsNullOrEmpty(criteriaFieldType_Name_Operator.Item1) &&
                    (criteriaFieldType_Name_Operator.Item1.EndsWith("?") || (criteriaFieldType_Name_Operator.Item1.ToLowerInvariant() == "string")))
                {
                    sb.Append($" = null");
                }

                if (i < criteriaFieldType_Name_Operators.Count - 1)
                {   // Not the last item, add a comma separator.
                    sb.Append(itemAggregationString);
                }
            }

            return sb.ToString();
        }

        public virtual string GetForeignKeyName(INavigation entityNavigation)
        {
            string retVal = null;
            if (entityNavigation == null)
            {
                return retVal;
            }

            var fkMetadataBase = entityNavigation.ForeignKey as CodeGenHero.Core.Metadata.MetadataBase;
            if (fkMetadataBase != null)
            {
                var relationalAnnotation = fkMetadataBase.FindAnnotation("Relational:Name");
                if (relationalAnnotation != null)
                {
                    retVal = relationalAnnotation.Value?.ToString();
                }
            }

            return retVal;
        }

        public virtual IList<string> GetForeignKeyNames(IList<INavigation> entityNavigations)
        {
            IList<string> retVal = new List<string>();

            if (entityNavigations == null)
            {
                return retVal;
            }

            entityNavigations.ForEach(entityNavigation =>
            {
                var foreignKeyName = GetForeignKeyName(entityNavigation);
                if (!string.IsNullOrWhiteSpace(foreignKeyName))
                {
                    retVal.Add(foreignKeyName);
                }
            });

            return retVal;
        }

        public virtual string GetNameModuleLevelVariable(string Propname)
        {
            return "_" + Inflector.ToLowerFirstCharacter(Propname);
        }

        public virtual List<string> GetPrimaryKeys(IEntityType entityType)
        {
            List<string> retVal = new List<string>();

            foreach (KeyValuePair<IList<IProperty>, IKey> entry in entityType.Keys)
            {
                foreach (var item in entry.Key)
                {
                    retVal.Add(item.Name);
                }
                //entityType.Keys[0].Value.Properties[0].Name;
            }

            return retVal;
        }

        /// <summary>
        /// Returns a list of the individual properties that aren't keys
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public virtual List<string> GetPropertiesExcludingKeys(IEntityType entityType)
        {
            List<string> retVal = new List<string>();
            var keys = GetPrimaryKeys(entityType);
            var entityProperties = entityType.GetProperties();
            foreach (var property in entityProperties)
            {
                if (!keys.Any(x => x.Equals(property.Name)))
                {
                    retVal.Add(property.Name);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Returns a string containing the necessary signature for an entity (int key1, int key2) based on the primary keys
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public virtual string GetSignature(IEntityType entityType)
        {
            string retVal = string.Empty;

            foreach (KeyValuePair<IList<IProperty>, IKey> entry in entityType.Keys)
            {
                foreach (var item in entry.Key)
                {
                    retVal += $", {item.ClrType.Name} {Inflector.Camelize(item.Name)}";
                }
            }
            // Lop off the initial ", "
            if (retVal.Length > 2)
            {
                retVal = retVal.Substring(2);
            }
            return retVal;
        }

        public virtual string GetSignatureWithFieldTypes(string indent, IKey primaryKey)
        {
            StringBuilder sb = new StringBuilder();
            if (primaryKey != null && primaryKey.Properties.Count > 0)
            {
                string propertyType = primaryKey.Properties[0].ClrType.Name;
                var pkPropertyName = Inflector.ToLowerFirstCharacter(primaryKey.Properties[0].Name);

                sb.Append($"{propertyType} {pkPropertyName}");
            }

            return sb.ToString();
        }

        public virtual string GetSignatureWithoutFieldTypes(string indent, IKey primaryKey, bool lowercasePkNameFirstChar)
        {
            StringBuilder sb = new StringBuilder();
            if (primaryKey != null && primaryKey.Properties.Count > 0)
            {
                var pkName = (lowercasePkNameFirstChar ? Inflector.ToLowerFirstCharacter(primaryKey.Properties[0].Name) : primaryKey.Properties[0].Name);
                sb.Append($"{pkName}");
            }
            return sb.ToString();
        }

        public virtual string GetSignatureWithoutFieldTypes(string indent, IKey primaryKey)
        {
            StringBuilder sb = new StringBuilder();
            if (primaryKey != null && primaryKey.Properties.Count > 0)
            {
                string propertyType = primaryKey.Properties[0].ClrType.Name;

                sb.Append($"{primaryKey.Properties[0].Name}");
            }
            return sb.ToString();
        }

        public virtual bool IsColumnAnAuditEditField(IProperty prop)
        {
            bool retVal = false;

            string colName = prop.Name.ToLowerInvariant();
            if (colName.Equals("createddate")
                || colName.Equals("createduserid")
                || colName.Equals("updateddate")
                || colName.Equals("updateduserid")
                || colName.Equals("isdeleted")
                )
            {
                retVal = true;
            }

            return retVal;
        }

        public virtual bool IsUnknownType(IProperty prop)
        {
            bool retVal = false;

            string colName = prop.ClrType.GetType().Name.ToLowerInvariant();
            if (colName.StartsWith("unknown"))
            {
                retVal = true;
            }

            return retVal;
        }

        /// <summary>
        /// postonlast false will suppress the final posttext on the last column key
        /// </summary>
        /// <param name="indent"></param>
        /// <param name="pretext"></param>
        /// <param name="posttext"></param>
        /// <param name="postonlast"></param>
        /// <returns></returns>
        public virtual string MakeModel(IKey primaryKey, string indent, string pretext, string posttext,
            bool postonlast, bool suppressFirstIndent, bool lowercaseVariableName)
        {
            StringBuilder sb = new StringBuilder();
            string posttextshow = posttext;
            if (primaryKey != null && primaryKey.Properties.Count > 0)
            {
                for (int i = 0; i < primaryKey.Properties.Count; i++)
                {
                    IProperty property = primaryKey.Properties[i];
                    if (!postonlast)
                    {
                        // TODO: The old implementation accounted for multiple columns.
                        // We have to revisit this
                        posttextshow = ""; //suppress the posttext.
                    }
                    if (!suppressFirstIndent || i > 0)
                    {
                        sb.Append(indent); // blank if only 1, to keep it on the same line
                    }

                    //string Name = Inflector.ToLowerFirstCharacter(property.Name);

                    sb.Append($"{pretext}{property.Name} = {(lowercaseVariableName ? Inflector.ToLowerFirstCharacter(property.Name) : property.Name)}{posttextshow}{Environment.NewLine}");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objectInstancePrefix"></param>
        /// <param name="indent"></param>
        /// <param name="properties"></param>
        /// <param name="primaryKey"></param>
        /// <returns>x => x.AnnouncementId == item.AnnouncementId</returns>
        public virtual string WhereClause(string objectInstancePrefix, string indent,
            IList<KeyValuePair<string, IProperty>> properties,
            IKey primaryKey, bool useLowerForFirstCharOfPropertyName)
        {
            StringBuilder sb = new StringBuilder();
            if (primaryKey != null && primaryKey.Properties.Count > 0)
            {
                sb.Append($"x => ");
                foreach (var property in primaryKey.Properties)
                {
                    sb.Append($"x.{property.Name} == " +
                        $"{(!string.IsNullOrEmpty(objectInstancePrefix) ? (objectInstancePrefix + ".") : string.Empty) }{(useLowerForFirstCharOfPropertyName ? Inflector.ToLowerFirstCharacter(property.Name) : property.Name)}");
                }
            }
            return sb.ToString();
        }
    }
}