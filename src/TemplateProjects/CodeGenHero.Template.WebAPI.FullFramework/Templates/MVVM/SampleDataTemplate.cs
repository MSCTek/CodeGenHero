using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.MVVM;
using System;
using CodeGenHero.Core;
using CodeGenHero.Inflector;
using CodeGenHero.Core;
using System.Collections.Generic;
using CodeGenHero.Template;
using System.Diagnostics;

namespace CodeGenHero.Template.WebAPI.FullFramework.MVVM
{
    [Template(name: Consts.TEMPLATE_SampleData, version: "2.0",
         uniqueTemplateIdGuid: "{28F7ACD7-FF48-4CD5-8068-2B234E52A11E}",
         description: "Sample Data Objects for use in generating demo versions.")]
    public class SampleDataTemplate : BaseAPIFFTemplate
    {
        public SampleDataTemplate()
        {
        }

        //[TemplateVariable(Consts.STG_dtoIncludeRelatedObjects_DEFAULTVALUE)]
        //public bool DTOIncludeRelatedObjects { get; set; }

        [TemplateVariable(Consts.STG_SampleDataCount_DEFAULTVALUE)]
        public string SampleDataCount { get; set; }

        [TemplateVariable(Consts.STG_SampleDataDigits_DEFAULTVALUE)]
        public string SampleDataDigits { get; set; }

        [TemplateVariable(Consts.OUT_sampleData_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string SampleDataOutputFilePath { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelDataNamespace_DEFAULTVALUE, description: "Namespace of the Sqllite Model classes.  Appears in the Usings.")]
        public string SqliteModelDataNamespace { get; set; }

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var entityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);

                foreach (var entity in entityTypes)
                {
                    string entityName = Inflector.Pascalize(entity.ClrType.Name);

                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                        fileName: Consts.OUT_sampleData);
                    outputfile = outputfile.Replace("[tablename]", entityName).Replace("[tablepluralname]", Inflector.Pluralize(entityName));
                    string filepath = outputfile;

                    var generator = new SampleDataGenerator(inflector: Inflector);

                    int sampleDataCount = 0;
                    if (int.TryParse(SampleDataCount, out int sampleCount))
                    {
                        sampleDataCount = sampleCount;
                    }
                    else
                    {
                        ProcessModel.Errors.Add("Unable to parse the SampleDataCount from the blueprint");
                        sampleCount = 3; // Use 3 if we can parse the value
                    }

                    int sampleDataDigits = 2;
                    if (int.TryParse(SampleDataDigits, out int sampleDigits))
                    {
                        Debug.WriteLine($"Successfully parsed \"{SampleDataDigits}\" to {sampleDigits}");
                        sampleDataCount = sampleDigits;
                    }
                    else
                    {
                        Debug.WriteLine($"Failed trying to parse \"{SampleDataDigits}\" to a number");
                        ProcessModel.Errors.Add("Unable to parse the SampleDataDigits from the blueprint");
                        sampleCount = 2; // Use 2 if we can't parse the value
                    }

                    string generatedCode = generator.GenerateSampleData(
                        entity: entity,
                        sqliteModelDataNamespace: SqliteModelDataNamespace,
                        prependSchemaNameIndicator: PrependSchemaNameIndicator,
                        sampleDataCount: sampleDataCount,
                        sampleDataDigits: sampleDataDigits
                        );

                    retVal.Files.Add(new OutputFile()
                    {
                        Content = generatedCode,
                        Name = filepath
                    });
                }
            }
            catch (Exception ex)
            {
                base.AddError(ref retVal, ex, Enums.LogLevel.Error);
            }

            AddTemplateVariablesManagerErrorsToRetVal(ref retVal, Enums.LogLevel.Error);
            return retVal;
        }
    }
}