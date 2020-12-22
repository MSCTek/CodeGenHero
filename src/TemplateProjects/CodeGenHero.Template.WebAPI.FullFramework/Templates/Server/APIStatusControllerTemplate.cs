using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.Server;
using System;
using System.Collections.Generic;
using CodeGenHero.Inflector;
using CodeGenHero.Core;

namespace CodeGenHero.Template.WebAPI.FullFramework.Server
{
    [Template(name: Consts.TEMPLATE_APIStatusController, version: "2.0",
         uniqueTemplateIdGuid: "{670EEF0C-24A5-40BF-9152-FAAA31E3988A}",
         description: "Provides an endpoint that can be called using via Get verb to determine if the service is available.  Does not require security authentication.")]
    public class APIStatusControllerTemplate : BaseAPIFFTemplate
    {
        public APIStatusControllerTemplate()
        {
        }

        //[TemplateVariable(Consts.STG_apiHelpersNamespace_DEFAULTVALUE)]
        //public string ApiHelpersNamespace { get; set; } // Doesn't seem to be used anywhere. Commenting it out.

        [TemplateVariable(Consts.OUT_apiStatusController_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string ApiStatusControllerOutputFilePath { get; set; }

        //[TemplateVariable(Consts.STG_enumNamespace_DEFAULTVALUE)]
        //public string EnumNamespace { get; set; } // Doesn't seem to be used anywhere. Commenting it out.

        //[TemplateVariable(Consts.STG_iLoggingServiceNamespace_DEFAULTVALUE)]
        //public string ILoggingServiceNamespace { get; set; }  // Doesn't seem to be used anywhere. Commenting it out.

        [TemplateVariable(Consts.STG_repositoryInterfaceNamespace_DEFAULTVALUE, description: "The format of the reference to the repository interface. Will appear in the Usings list.")]
        public string RepositoryInterfaceNamespace { get; set; }

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string classNamespace = $"{BaseNamespace}.API.Controllers.{NamespacePostfix}";
                string repositoryName = $"I{NamespacePostfix}Repository";

                List<NamespaceItem> usingNamespaceItems = new List<NamespaceItem>();
                usingNamespaceItems.Insert(0, new NamespaceItem() { Namespace = "Microsoft.Extensions.Logging" });
                usingNamespaceItems.Insert(1, new NamespaceItem() { Namespace = "System", NamespacePrefix = null, Name = null });
                usingNamespaceItems.Insert(2, new NamespaceItem() { Namespace = "System.Threading.Tasks", NamespacePrefix = null, Name = null });
                usingNamespaceItems.Insert(3, new NamespaceItem() { Namespace = "System.Web.Http", NamespacePrefix = null, Name = null });
                usingNamespaceItems.Insert(4, new NamespaceItem() { Namespace = "CodeGenHero.Core.Enums", NamespacePrefix = "coreEnums" });
                usingNamespaceItems.Insert(5, new NamespaceItem() { Namespace = "CodeGenHero.WebApi" });
                usingNamespaceItems.Insert(6, new NamespaceItem() { Namespace = RepositoryInterfaceNamespace, NamespacePrefix = null, Name = null });

                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_apiStatusController);
                string filepath = outputfile;

                var generator = new APIStatusControllerGenerator(inflector: Inflector);
                string generatedCode = generator.GenerateApiStatusController(
                    usingNamespaceItems: usingNamespaceItems,
                    classnamePrefix: NamespacePostfix,
                    classNamespace: classNamespace,
                    repositoryname: repositoryName);

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