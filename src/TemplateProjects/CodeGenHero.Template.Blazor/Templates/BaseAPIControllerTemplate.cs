using CodeGenHero.Template.Blazor.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor.Templates
{
    [Template(name: "BaseAPIController", version: "2021.9.14", uniqueTemplateIdGuid: "AF56140D-4926-4E6A-ADDB-49F3CFCD4A53",
        description: "Generates a Base API Controller class for anonymous API Controllers to inherit from.")]
    class BaseAPIControllerTemplate : BaseBlazorTemplate
    {
        public BaseAPIControllerTemplate()
        {

        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.BaseAPIControllerOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string BaseAPIControllerOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: "false", description: "If true, will include the flag [AutoInvalidateCacheOutput] in the generated code.")]
        public bool AutoInvalidateCacheOutput { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_BaseAPIControllerName_DEFAULT, description: Consts.PTG_BaseAPIControllerName_DESC)]
        public string BaseAPIControllerClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_APIControllerNamespace_DEFAULT, description: Consts.PTG_APIControllerNamespace_DESC)]
        public string APIControllerNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryNamespace_DEFAULT, description: Consts.PTG_RepositoryNamespace_DESC)]
        public string RepositoryNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_BaseAPIControllerOutputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic"),
                    new NamespaceItem("System.Linq"),
                    new NamespaceItem("System.Net"),
                    new NamespaceItem("System.Runtime.CompilerServices"),
                    new NamespaceItem("Microsoft.AspNetCore.Authorization"),
                    new NamespaceItem("Microsoft.AspNetCore.Http"),
                    new NamespaceItem("Microsoft.AspNetCore.Http.Extensions;"),
                    new NamespaceItem("Microsoft.AspNetCore.Mvc"),
                    new NamespaceItem("Microsoft.AspNetCore.Routing"),
                    new NamespaceItem("Microsoft.Extensions.Logging"),
                    new NamespaceItem("Microsoft.Extensions.Logging.Abstractions"),
                    new NamespaceItem(RepositoryNamespace),
                    new NamespaceItem(DtoNamespace),
                    new NamespaceItem("cghcEnums = CodeGenHero.Core.Enums")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new BaseAPIControllerGenerator(inflector: Inflector);
                var generatedCode = generator.Generate(usings, APIControllerNamespace, NamespacePostfix, AutoInvalidateCacheOutput, BaseAPIControllerClassName);

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
