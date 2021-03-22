using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.Server;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.WebAPI.FullFramework.Server
{
    [Template(name: Consts.TEMPLATE_WebApiController, version: "2.1",
        uniqueTemplateIdGuid: "{4F4C0CB1-134C-477A-B796-0EA82A911D33}",
        description: "Web API 2.0 controllers that support web and mobile clients and can, optionally, require basic or OAuth 2.0 security authentication.")]
    public class WebApiControllerTemplate : BaseAPIFFTemplate
    {
        public WebApiControllerTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.STG_apiHelpersNamespace_DEFAULTVALUE)]
        public string ApiHelpersNamespace { get; set; }

        [TemplateVariable(Consts.STG_DbContextName_DEFAULTVALUE)]
        public string DbContextName { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_dtoNamespace_DEFAULTVALUE, description: "Namespace of the DTO classes.  Appears in the Usings.")]
        public string DtoNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_enumNamespace_DEFAULTVALUE)]
        public string EnumNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_factoryNamespace_DEFAULTVALUE)]
        public string FactoryNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_iLoggingServiceNamespace_DEFAULTVALUE)]
        public string ILoggingServiceNamespace { get; set; }

        [TemplateVariable(defaultValue: null,
            description: "A list of MSC.CodeGenHero.DTO.NameValue items serialized as JSON that correspond to table names and integer values for the maximum number of rows to return for a single request for a page of data.")]
        public string MaxRequestPerPageOverrideByTableName { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_repositoryEntitiesNamespace_DEFAULTVALUE)]
        public string RepositoryEntitiesNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_repositoryNamespace_DEFAULTVALUE, description: "The format of the reference to the repository class. Will appear in the Usings list.")]
        public string RepositoryNamespace { get; set; }

        [TemplateVariable(Consts.STG_useAuthorizedBaseController_DEFAULTVALUE)]
        public bool UseAuthorizedBaseController { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_webApiControllerNamespace_DEFAULTVALUE)]
        public string WebApiControllerNamespace { get; set; }

        [TemplateVariable(Consts.OUT_webApiController_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string WebApiControllerOutputFilePath { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);
                foreach (var entity in filteredEntityTypes)
                {
                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                        fileName: Consts.OUT_webApiController);
                    outputfile = outputfile.Replace("[tablename]", Inflector.Humanize(entity.ClrType.Name)).Replace("[tablepluralname]", Inflector.Pluralize(entity.ClrType.Name));
                    string filepath = outputfile;

                    var maxRequestPerPageOverrides =
                    DeserializeJsonObject<List<NameValue>>(MaxRequestPerPageOverrideByTableName);

                    if (maxRequestPerPageOverrides == null)
                    {
                        maxRequestPerPageOverrides = new List<NameValue>();
                    }

                    string baseControllerName = $"{NamespacePostfix}BaseApiController";

                    if (UseAuthorizedBaseController) //!string.IsNullOrEmpty(UseAuthorizedBaseController) && UseAuthorizedBaseController.ToLowerInvariant() == "true")
                    {
                        baseControllerName = $"{NamespacePostfix}BaseApiControllerAuthorized";
                    }

                    WebApiControllerGenerator webApiControllerGenerator = new WebApiControllerGenerator(inflector: Inflector);
                    string generatedCode = webApiControllerGenerator.GenerateWebApiController(
                        baseNamespace: BaseNamespace,
                        namespacePostfix: NamespacePostfix,
                        classNamespace: WebApiControllerNamespace,
                        baseControllerName: baseControllerName,
                        apiHelpersNamespace: ApiHelpersNamespace,
                        iLoggingServiceNamespace: ILoggingServiceNamespace,
                        repositoryNamespace: RepositoryNamespace,
                        factoryNamespace: FactoryNamespace,
                        efEntityNamespacePrefix: $"ent{NamespacePostfix}",
                        efEntityNamespace: RepositoryEntitiesNamespace,
                        dtoNamespacePrefix: $"dto{NamespacePostfix}",
                        dtoNamespace: DtoNamespace,
                        enumNamespace: EnumNamespace,
                        repositoryInterfaceName: $"I{NamespacePostfix}Repository",
                        dbContextName: DbContextName,
                        maxRequestPerPageOverrides: maxRequestPerPageOverrides,
                        prependSchemaNameIndicator: PrependSchemaNameIndicator,
                        entity: entity,
                        excludedNavigationProperties: ProcessModel.ExcludedNavigationProperties
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