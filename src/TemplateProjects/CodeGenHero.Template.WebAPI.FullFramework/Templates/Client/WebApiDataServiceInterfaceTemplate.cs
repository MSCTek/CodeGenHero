using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.Client;
using System;
using CodeGenHero.Core;
using CodeGenHero.Inflector;
using CodeGenHero.Core;
using System.Collections.Generic;
using CodeGenHero.Template;

namespace CodeGenHero.Template.WebAPI.FullFramework.Client
{
    [Template(name: Consts.TEMPLATE_WebApiDataServiceInterface, version: "2.0",
         uniqueTemplateIdGuid: "{F3203AD7-73AB-46E0-9ECE-43B75C855164}",
         description: "Provides an interface for the implementation class that provides functionality to abstract calls to Web API services.")]
    public class WebApiDataServiceInterfaceTemplate : BaseAPIFFTemplate
    {
        public WebApiDataServiceInterfaceTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_defaultCriteria_DEFAULTVALUE)]
        public string DefaultCriteria { get; set; }

        [TemplateVariable(Consts.STG_dtoNamespace_DEFAULTVALUE, description: "Namespace of the DTO classes.  Appears in the Usings.")]
        public string DtoNamespace { get; set; }

        [TemplateVariable(Consts.STG_webApiDataServiceInterfaceClassName_DEFAULTVALUE)]
        public string WebApiDataServiceInterfaceClassName { get; set; }

        [TemplateVariable(Consts.STG_webApiDataServiceInterfaceNamespace_DEFAULTVALUE)]
        public string WebApiDataServiceInterfaceNamespace { get; set; }

        [TemplateVariable(Consts.OUT_webApiDataServiceInterface_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string WebApiDataServiceInterfaceOutputFilePath { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                fileName: Consts.OUT_webApiDataServiceInterface);
                string filepath = outputfile;
                var generator = new WebApiDataServiceInterfaceGenerator(inflector: Inflector);
                var generatedCode = generator.GenerateWebApiDataServiceInterface(dtoNamespace: DtoNamespace,
                    defaultCriteria: DefaultCriteria,
                    baseNamespace: BaseNamespace,
                    webApiDataServiceInterfaceNamespace: WebApiDataServiceInterfaceNamespace,
                    webApiDataServiceInterfaceClassName: WebApiDataServiceInterfaceClassName,
                    prependSchemaNameIndicator: PrependSchemaNameIndicator,
                     EntityTypes: ProcessModel.MetadataSourceModel.EntityTypes
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