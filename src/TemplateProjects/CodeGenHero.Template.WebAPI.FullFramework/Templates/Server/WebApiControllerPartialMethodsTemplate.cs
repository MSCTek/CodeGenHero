using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.Server;
using System;

namespace CodeGenHero.Template.WebAPI.FullFramework.Server
{
    [Template(name: Consts.TEMPLATE_WebApiControllerPartialMethods, version: "2.1",
        uniqueTemplateIdGuid: "{0AEA3751-020A-4F50-93E5-0C58A2C8E953}",
        description: "A complimentary template for Web API 2.0 controllers that generates sample implementations for partial methods that get invoked as injection points in the controller's REST operations.")]
    public class WebApiControllerPartialMethodsTemplate : BaseAPIFFTemplate
    {
        public WebApiControllerPartialMethodsTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_allowUpsertDuringPut_DEFAULTVALUE)]
        public bool AllowUpsertDuringPut { get; set; }

        [TemplateVariable(Consts.STG_DbContextName_DEFAULTVALUE)]
        public string DbContextName { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_repositoryEntitiesNamespace_DEFAULTVALUE)]
        public string RepositoryEntitiesNamespace { get; set; }

        [TemplateVariable(Consts.STG_repositoryNamespace_DEFAULTVALUE, description: "The format of the reference to the repository class. Will appear in the Usings list.")]
        public string RepositoryNamespace { get; set; }

        [TemplateVariable(Consts.STG_useAuthorizedBaseController_DEFAULTVALUE)]
        public bool UseAuthorizedBaseController { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_webApiControllerNamespace_DEFAULTVALUE)]
        public string WebApiControllerNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.OUT_webApiControllerPartialMethods_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string WebApiControllerPartialMethodsOutputFilePath { get; set; }

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
                        fileName: Consts.OUT_webApiControllerPartialMethods);
                    outputfile = outputfile.Replace("[tablename]", Inflector.Humanize(entity.ClrType.Name)).Replace("[tablepluralname]", Inflector.Pluralize(entity.ClrType.Name));
                    string filepath = outputfile;

                    string baseControllerName = $"{NamespacePostfix}BaseApiController";

                    if (UseAuthorizedBaseController) //!string.IsNullOrEmpty(UseAuthorizedBaseController) && UseAuthorizedBaseController.ToLowerInvariant() == "true")
                    {
                        baseControllerName = $"{NamespacePostfix}BaseApiControllerAuthorized";
                    }

                    var generator = new WebApiControllerPartialMethodsGenerator(inflector: Inflector);
                    string generatedCode = generator.GenerateWebApiControllerPartialMethods(
                        namespacePostfix: NamespacePostfix,
                        classNamespace: WebApiControllerNamespace,
                        baseControllerName: baseControllerName,
                        efEntityNamespacePrefix: $"ent{NamespacePostfix}",
                        efEntityNamespace: RepositoryEntitiesNamespace,
                        dbContextName: DbContextName,
                        entity: entity,
                        excludedNavigationProperties: ProcessModel.ExcludedNavigationProperties,
                        allowUpsertDuringPut: AllowUpsertDuringPut
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