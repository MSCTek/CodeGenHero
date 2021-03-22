//------------------------------------------------------------------------------
// <copyright file="WebApiDataServiceTemplate.cs" company="Micro Support Center, Inc.">
//     Copyright (c) Micro Support Center, Inc..  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.Client;
using System;

namespace CodeGenHero.Template.WebAPI.FullFramework.Client
{
    [Template(name: Consts.TEMPLATE_WebApiDataService, version: "2.0",
        uniqueTemplateIdGuid: "{003C0699-A7FB-4F60-AC64-530A2DBC7A25}",
        description: "Provides functionality to abstract calls to Web API services.")]
    public class WebApiDataServiceTemplate : BaseAPIFFTemplate
    {
        public WebApiDataServiceTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_defaultCriteria_DEFAULTVALUE)]
        public string DefaultCriteria { get; set; }

        [TemplateVariable(Consts.STG_dtoNamespace_DEFAULTVALUE, description: "Namespace of the DTO classes.  Appears in the Usings.")]
        public string DtoNamespace { get; set; }

        [TemplateVariable(Consts.STG_iLoggingServiceNamespace_DEFAULTVALUE)]
        public string ILoggingServiceNamespace { get; set; }

        [TemplateVariable(Consts.STG_serializationHelperNamespace_DEFAULTVALUE)]
        public string SerializationHelperNamespace { get; set; }

        [TemplateVariable(Consts.STG_webApiDataServiceInterfaceNamespace_DEFAULTVALUE)]
        public string WebApiDataServiceInterfaceNamespace { get; set; }

        [TemplateVariable(Consts.STG_webApiDataServiceNamespace_DEFAULTVALUE)]
        public string WebApiDataServiceNamespace { get; set; }

        [TemplateVariable(Consts.OUT_webApiDataService_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string WebApiDataServiceOutputFilePath { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_webApiDataService);
                string filepath = outputfile;
                string dtoNamespacePrefix = "xDTO";
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new WebApiDataServiceGenerator(inflector: Inflector);
                var generatedCode = generator.GenerateWebApiDataService(
                    iLoggingServiceNamespace: ILoggingServiceNamespace,
                    serializationHelperNamespace: SerializationHelperNamespace,
                    webApiDataServiceInterfaceNamespace: WebApiDataServiceInterfaceNamespace,
                    webApiDataServiceNamespace: WebApiDataServiceNamespace,
                    dtoNamespacePrefix: dtoNamespacePrefix,
                    dtoNamespace: DtoNamespace,
                    classNamespace: WebApiDataServiceNamespace,
                    namespacePostfix: NamespacePostfix,
                    defaultCriteria: DefaultCriteria,
                    prependSchemaNameIndicator: PrependSchemaNameIndicator,
                    entityTypes: filteredEntityTypes,
                    baseNamespace: BaseNamespace
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