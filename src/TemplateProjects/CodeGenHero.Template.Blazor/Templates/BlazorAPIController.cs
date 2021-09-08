using CodeGenHero.Template.Blazor.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor.Templates
{
    [Template(name: "BlazorAPIController", version: "1.0", uniqueTemplateIdGuid: "70A21A48-7EE1-42F5-B1EB-4891E290A17D", )]
    public class BlazorAPIController : BaseBlazorTemplate
    {
        public BlazorAPIController()
        {

        }

        #region TemplateVariables

        [TemplateVariable(Consts.BlazorAPIControllerClassNamespace_DEFAULTVALUE, description: "")]
        public string BlazorAPIControllerClassNamespace { get; set; }

        [TemplateVariable(Consts.OUT_BlazorAPIControllerFilePath_DEFAULTVALUE, hiddenIndicator: true, description:"Path of outputted file.")]
        public string BlazorAPIControllerOutputFilePath { get; set; }

        [TemplateVariable(defaultValue: null,
            description: "A list of MSC.CodeGenHero.DTO.NameValue items serialized as JSON that correspond to table names and integer values for the maximum number of rows to return for a single request for a page of data.")]
        public string MaxRequestPerPageOverrideByTableName { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            var maxRequestPerPageOverrides =
                    DeserializeJsonObject<List<NameValue>>(MaxRequestPerPageOverrideByTableName);

            if (maxRequestPerPageOverrides == null)
            {
                maxRequestPerPageOverrides = new List<NameValue>();
            }

            try
            {
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                foreach(var entity in filteredEntityTypes)
                {
                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.ToDataMapper_OutFileVariableName);
                    outputfile = outputfile.Replace("[tablename]", Inflector.Humanize(entity.ClrType.Name)).Replace("[tablepluralname]", Inflector.Pluralize(entity.ClrType.Name));
                    string filepath = outputfile;

                    var usings = new List<NamespaceItem>
                    {
                        new NamespaceItem("Microsoft.AspNetCore.Authorization"),
                        new NamespaceItem($"Microsoft.AspNetCore.Mvc"),
                        new NamespaceItem($"{BaseNamespace}.Api.Database")
                    };

                    var generator = new BlazorAPIControllerGenerator(inflector: Inflector);
                    string generatedCode = generator.Generate(usings, BlazorAPIControllerClassNamespace, NamespacePostfix, maxRequestPerPageOverrides: maxRequestPerPageOverrides, PrependSchemaNameIndicator, entityType: entity);

                    retVal.Files.Add(new OutputFile()
                    {
                        Content = generatedCode,
                        Name = filepath
                    });
                }
            }
            catch(Exception ex)
            {
                base.AddError(ref retVal, ex, Enums.LogLevel.Error);
            }

            AddTemplateVariablesManagerErrorsToRetVal(ref retVal, Enums.LogLevel.Error);
            return retVal;
        }
    }
}
