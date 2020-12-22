using CodeGenHero.Template.Blazor.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor.Templates
{
    [Template(name: Consts.TEMPLATE_ToDataMapper, version: "1.0", uniqueTemplateIdGuid: "{509CD2C0-64EB-4AA5-BDE8-3F066A073F9E}",
        description: "Creates code to map from models to entities.")]
    public class ToDataMapper : BaseBlazorTemplate
    {
        public ToDataMapper()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.Entities_Namespace_DEFAULTVALUE, description: "The format of the reference to the Entities namespace. Will appear in the Usings list.")]
        public string EntitiesNamespace { get; set; }

        [TemplateVariable(Consts.Model_Namespace_DEFAULTVALUE, description: "The format of the reference to the Model namespace. Will appear in the Usings list.")]
        public string ModelNamespace { get; set; }

        [TemplateVariable(Consts.ToDataMapperClassNamespace_DEFAULTVALUE, description: "")]
        public string ToDataMapperClassNamespace { get; set; }

        [TemplateVariable(Consts.OUT_ToDataMapperFilePath_DEFAULTVALUE, hiddenIndicator: true,
                    description: "The format of the filename for the generated file.")]
        public string ToDataMapperOutputFilePath { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.ToDataMapper_OutFileVariableName);
                string filepath = outputfile;

                var usings = new List<string>
                {
                    $"using System;",
                    $"using System.Linq;",
                    $"using System.Collections.Generic;",
                    $"using System.Reflection;",
                    $"using cEnums = CGH.Common.Enums;",
                    $"using xData = {EntitiesNamespace};",
                    $"using xModel = {ModelNamespace};",
                };

                var generator = new ToDataMapperGenerator(inflector: Inflector);
                string generatedCode = generator.Generate(
                    usings: usings,
                    ToDataMapperClassNamespace,
                    prependSchemaNameIndicator: PrependSchemaNameIndicator,
                     entityTypes: ProcessModel.MetadataSourceModel.EntityTypes
                 );

                retVal.Files.Add(new OutputFile()
                {
                    Content = generatedCode,
                    Name = filepath
                });
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