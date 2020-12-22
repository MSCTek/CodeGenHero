using CodeGenHero.Template.WebAPI.FullFramework.Generators.Server;
using System;
using CodeGenHero.Core;
using System.Collections.Generic;
using MSC.CodeGenHero.Library;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.WebAPI.FullFramework.Server
{
    [Template(name: Consts.TEMPLATE_APIBaseController, version: "2.0",
           uniqueTemplateIdGuid: "{CAB9BF26-C662-4D64-B50D-3D907694545A}",
           description: "The base controller from which non-authorized controllers inherit.")]
    public class APIBaseControllerTemplate : BaseAPIFFTemplate
    {
        public APIBaseControllerTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.OUT_apiBaseController_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string ApiBaseControllerOutputFilePath { get; set; }

        [TemplateVariable(defaultValue: "false", description: "If true, will include the flag [AutoInvalidateCacheOutput] in the generated code.")]
        public bool AutoInvalidateCacheOutput { get; set; }

        [TemplateVariable(Consts.STG_baseApiControllerClassName_DEFAULTVALUE, description: "The format of the class name.")]
        public string BaseApiControllerClassName { get; set; }

        [TemplateVariable(Consts.STG_baseApiControllerNameSpace_DEFAULTVALUE, description: "The full namespace to use. By default, incorporates the baseNamespace, and namespacePostfix")]
        public string BaseApiControllerNameSpace { get; set; }

        [TemplateVariable(Consts.STG_DbContextName_DEFAULTVALUE)]
        public virtual string DbContextName { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_repositoryEntitiesNamespace_DEFAULTVALUE, description: "")] // Not finding this being used anywhere. Commenting it out
        public string RepositoryEntitiesNamespace { get; set; }

        [TemplateVariable(Consts.STG_repositoryInterfaceNamespace_DEFAULTVALUE, description: "The format of the reference to the Interface class. Will appear in the Usings list.")]
        public string RepositoryInterfaceNamespace { get; set; }

        //[TemplateVariable(Consts.STG_loggingServiceFullyQualifiedClassName_DEFAULTVALUE)]
        //public string LoggingServiceFullyQualifiedClassName { get; set; }
        [TemplateVariable(Consts.STG_repositoryNamespace_DEFAULTVALUE, description: "The format of the reference to the repository class. Will appear in the Usings list.")]
        public string RepositoryNamespace { get; set; }

        [TemplateVariable(Consts.STG_useAuthorizedBaseController_DEFAULTVALUE, description: "If true, will add an [Authorize] to the generated code and change the class name to include Authorized.")]
        public bool UseAuthorizedBaseController { get; set; }

        [TemplateVariable(defaultValue: "false", description: "If UseAuthorizedBaseController is set, will set [Authorize] flag to [IdentityBasicAuthentication] instead.")]
        public bool UseIdentityBasicAuthenticationAttribute { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string interfaceRepository = $"I{NamespacePostfix}Repository";
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity, fileName: Consts.OUT_apiBaseController);

                string className = BaseApiControllerClassName;
                if (UseAuthorizedBaseController)
                {
                    outputfile = outputfile.Replace(".cs", "Authorized.cs");
                    if (!className.Contains("Authorized"))
                    {
                        className = $"{className}Authorized";
                    }
                }

                var generator = new APIBaseControllerGenerator(inflector: Inflector);
                List<NamespaceItem> usings = generator.BuildBaseApiControllerUsings(
                    repositoryNamespace: RepositoryNamespace,
                    repositoryInterfaceNamespace: RepositoryInterfaceNamespace,
                    autoInvalidateCacheOutput: AutoInvalidateCacheOutput);

                var values = ProcessModel.MetadataSourceProperties;

                //string dbContextFullyQualifiedTypeName = ProcessModel.MetadataSourceModel.ModelFullyQualifiedTypeName;
                //if (values.KeyValues.ContainsKey(cghConsts.METADATASOURCE_PROPERTYNAME_FQTypeName))
                //{
                //    dbContextName = values.KeyValues[cghConsts.METADATASOURCE_PROPERTYNAME_FQTypeName];
                //}

                string generatedCode = generator.GenerateBaseApiController(
                    usings: usings,
                    namespacePostfix: NamespacePostfix,
                    className: className, // "Authorized" gets appended if useAuthorizedBaseController = true.
                    classNamespace: BaseApiControllerNameSpace,
                    interfaceRepository: interfaceRepository,
                    repositoryEntitiesNamespace: RepositoryEntitiesNamespace,
                    dbContextName: DbContextName,
                    autoInvalidateCacheOutput: AutoInvalidateCacheOutput,
                    useAuthorizedBaseController: UseAuthorizedBaseController,
                    useIdentityBasicAuthenticationAttribute: UseIdentityBasicAuthenticationAttribute
                 );

                retVal.Files.Add(new OutputFile()
                {
                    Content = generatedCode,
                    Name = outputfile
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