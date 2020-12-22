using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using System.Linq;
using System;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.MVVM
{
    public class SampleDataGenerator : BaseAPIFFGenerator
    {
        private const string EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION = " -- Excluded navigation property per configuration.";

        private Random _random = new Random();

        public SampleDataGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        /*
        public static BingoContent SampleBingoContent01
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId01,
                    Content = "'Let's take this offline'",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }
        */

        public string GenerateSampleData(
            IEntityType entity,
            string sqliteModelDataNamespace,
            bool prependSchemaNameIndicator,
            int sampleDataCount,
            int sampleDataDigits)
        {
            var sb = new StringBuilder();
            var k = entity.FindPrimaryKey();
            string entityName = Inflector.Pascalize(entity.ClrType.Name);
            var entityProperties = entity.GetProperties();
            bool hasMultiplePrimaryKeys = entity.FindPrimaryKey().Properties.Count > 1;
            string numberFormat = new String('0', sampleDataDigits);
            Dictionary<string, string> uniquePKModuleLevelVariableNames = new Dictionary<string, string>();

            sb.AppendLine("using System;");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {sqliteModelDataNamespace}");
            sb.AppendLine($"{{");

            sb.AppendLine($"\tpublic static class Demo{entityName}");
            sb.AppendLine("\t{");
            //sb.AppendLine($"\t\t//number format is \"{numberFormat}\" and sampledigits is {SampleDataDigits}");
            for (int sampleNumber = 0; sampleNumber < sampleDataCount; sampleNumber++)
            {
                string uniquePKModuleLevelVariableName;
                // Generate unique keys for use in the sample objects
                for (int i = 0; i < entityProperties.Count(); i++)
                {
                    var property = entityProperties[i];
                    string propertyName = Inflector.Pascalize(property.Name);
                    string ctype = GetCType(property);
                    var simpleType = ConvertToSimpleType(ctype);

                    if (property.Name == k.Properties[0].Name)
                    {
                        var sampleNumberFormatted = sampleNumber.ToString(numberFormat);

                        uniquePKModuleLevelVariableName = GetUniquePKModuleLevelVariableName(entityName: entityName, sampleNumber: sampleNumber, columnNumber: i, numberFormat: numberFormat);
                        uniquePKModuleLevelVariableNames[$"{propertyName}{sampleNumberFormatted}"] = uniquePKModuleLevelVariableName; // Gather up the unique PK variable names for use below.

                        if (hasMultiplePrimaryKeys)
                        {   // By and large, this really only happens with link/associative tables that are pointing at other tables.
                            //   So, here we are depending on values in another sample data class.
                            var nameOfOtherSampleDataClass = propertyName.Replace("Id", string.Empty); //.GetNameHumanCaseSingular(prependSchemaNameIndicator);
                            sb.AppendLine($"\t\tpublic static {simpleType} {uniquePKModuleLevelVariableName} = Demo{nameOfOtherSampleDataClass}.Sample{propertyName}{sampleNumberFormatted}00;"); // Note assumption with "00" in place of the column number there only being one PK in the associated table
                        }
                        else
                        {
                            sb.AppendLine($"\t\tpublic static {simpleType} {uniquePKModuleLevelVariableName} = {SampleValue(property)};");
                        }
                    }
                }
            }
            sb.AppendLine(string.Empty);

            // Constructor .ctor code

            for (int sampleNumber = 0; sampleNumber < sampleDataCount; sampleNumber++)
            {
                string sampleNumberFormatted = sampleNumber.ToString(numberFormat);
                sb.AppendLine($"\t\tpublic static {entityName} Sample{entityName}{sampleNumberFormatted}");
                sb.AppendLine("\t\t{");

                sb.AppendLine("\t\t\tget");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine($"\t\t\t\treturn new {entityName}()");
                sb.AppendLine("\t\t\t\t{");

                for (int i = 0; i < entityProperties.Count(); i++)
                {
                    var property = entityProperties[i];
                    string propertyName = Inflector.Pascalize(property.Name);
                    string ctype = GetCType(property);
                    var simpleType = ConvertToSimpleType(ctype);
                    // We need to pass the sampleNumber along in case this is a primary key column
                    var sampleType = GetSampleDataValueOrUniquePKModuleLevelVariableName(property: property, sampleNumber: sampleNumber,
                        columnNumber: i, numberFormat: numberFormat, uniquePKModuleLevelVariableNames: uniquePKModuleLevelVariableNames, entityName: entityName, k.Properties[0].Name);
                    sb.AppendLine($"\t\t\t\t\t{propertyName} = {sampleType},");
                }

                sb.AppendLine("\t\t\t\t};");

                sb.AppendLine("\t\t\t}");
                sb.AppendLine("\t\t}");
            }

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }

        //TODO: Make an enum with the system types instead
        private string GetSampleDataValueOrUniquePKModuleLevelVariableName(IProperty property, int sampleNumber, int columnNumber, string numberFormat,
            Dictionary<string, string> uniquePKModuleLevelVariableNames, string entityName, string primaryKeyname)
        {
            string retVal = string.Empty;
            string propertyName = Inflector.Pascalize(property.Name);
            string ctype = GetCType(property);
            var simpleType = ConvertToSimpleType(ctype);
            // We need to use one of the existing premade identity keys if it's a PK
            // for now assume there are no 2 part keys
            if (property.Name == primaryKeyname)
            {
                string uniquePKModuleLevelVariableName = GetUniquePKModuleLevelVariableName(entityName: entityName, sampleNumber: sampleNumber, columnNumber: columnNumber, numberFormat: numberFormat);
                string uniquePKModuleLevelVariableNameKey = $"{property.Name}{sampleNumber.ToString(numberFormat)}";

                if (!uniquePKModuleLevelVariableNames.ContainsKey(uniquePKModuleLevelVariableNameKey))
                {
                    throw new ApplicationException($"Error in GenerateSampleData.{nameof(GetSampleDataValueOrUniquePKModuleLevelVariableName)} - matching column name {propertyName} was not found in uniquePKModuleLevelVariableNames.");
                }

                retVal = uniquePKModuleLevelVariableNames[uniquePKModuleLevelVariableNameKey];
            }
            else
            {
                // These columns are not primary keys.
                switch (simpleType.ToLower())
                {
                    case "guid":
                    case "guid?":
                    case "system.guid":
                    case "system.guid?":
                        retVal = $"Guid.Parse(\"{Guid.NewGuid().ToString()}\")";
                        break;

                    case "datetime":
                    case "datetime?":
                    case "datetimeoffset":
                    case "system.datetime":
                    case "system.datetime?":
                        retVal = "DateTime.Now";
                        break;

                    case "short":
                    case "short?":
                    case "int":
                    case "int?":
                    case "system.int32":
                    case "system.int32?":
                    case "system.int64":
                    case "system.int64?":
                    case "double":
                    case "double?":
                    case "long":
                    case "long?":
                    case "decimal":
                    case "decimal?":
                    case "float":
                    case "float?":
                        retVal = "0";
                        break;

                    case "bool":
                    case "bool?":
                        retVal = "false";
                        break;

                    case "byte[]":
                    case "byte[]?":
                        retVal = "new byte[0]";
                        break;

                    case "string":
                        retVal = $"\"Sample{propertyName}\"";
                        break;

                    case "system.data.entity.spatial.dbgeometry":
                        retVal = "\"geometry\"";
                        break;

                    default:
                        retVal = "Unknown";
                        break;
                }
            }
            return retVal;
        }

        private string GetUniquePKModuleLevelVariableName(string entityName, int sampleNumber, int columnNumber, string numberFormat)
        {
            return $"Sample{entityName}Id{sampleNumber.ToString(numberFormat)}{columnNumber.ToString(numberFormat)}"; // We need to add the "i.ToString(numberFormat)" for tables with multiple primary keys.
        }

        private DateTime RandomDate()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(_random.Next(range));
        }

        private int RandomInt()
        {
            return _random.Next(1, int.MaxValue);
        }

        private string SampleValue(IProperty property)
        {
            string retVal = string.Empty;

            string propertyName = Inflector.Pascalize(property.Name);
            string ctype = GetCType(property);
            DateTime dateRandom = RandomDate();
            var simpleType = ConvertToSimpleType(ctype);
            // return appropriate data for the given column type.
            switch (simpleType.ToLower())
            {
                case "guid":
                case "guid?":
                case "system.guid":
                case "system.guid?":
                    retVal = $"Guid.Parse(\"{Guid.NewGuid().ToString()}\")";
                    break;

                case "datetime":
                case "datetime?":
                case "datetimeoffset":
                case "system.datetime":
                case "system.datetime?":
                    // While I can't imagine a datetime field as a primary key, it could happen
                    retVal = $"DateTime({dateRandom.Year},{dateRandom.Month},{dateRandom.Day},{dateRandom.Hour},{dateRandom.Minute},{dateRandom.Second})";
                    break;

                case "short":
                case "short?":
                case "int":
                case "int?":
                case "system.int32":
                case "system.int32?":
                case "system.int64":
                case "system.int64?":
                case "double":
                case "double?":
                case "long":
                case "long?":
                case "decimal":
                case "decimal?":
                case "float":
                case "float?":
                    // Quite possibly an int would be used as a primary key but we definitely want a non-zero
                    retVal = $"{RandomInt()}";
                    break;

                case "bool":
                case "bool?":
                    // As a primary key, highly unlikely
                    retVal = "false";
                    break;

                case "byte[]":
                case "byte[]?":
                    // As a primary key, highly unlikely
                    retVal = "new byte[0]";
                    break;

                case "string":
                    // I've seen people using strings as primary keys.  seriously.
                    retVal = $"Sample{property.Name}{RandomInt()}";
                    break;

                case "system.data.entity.spatial.dbgeometry":
                    retVal = "geometry";
                    break;

                default:
                    retVal = "Unknown";
                    break;
            }

            return retVal;
        }
    }
}