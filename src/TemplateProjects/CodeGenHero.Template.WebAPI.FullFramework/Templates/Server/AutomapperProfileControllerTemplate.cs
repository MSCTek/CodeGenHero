using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.Server;
using System;

namespace CodeGenHero.Template.WebAPI.FullFramework.Server
{
    [Template(name: Consts.TEMPLATE_AutoMapperProfile, version: "2.0",
        uniqueTemplateIdGuid: "{A72DC8D8-F6C5-422A-9554-93FA6DC17282}",
        description: "Maps Entity Framework objects to DTOs and vice versa.")]
    public class AutomapperProfileControllerTemplate : BaseAPIFFTemplate
    {
        public AutomapperProfileControllerTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.OUT_automapperProfile_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string AutomapperProfileOutputFilePath { get; set; }

        [TemplateVariable(Consts.STG_dtoNamespace_DEFAULTVALUE, description: "Namespace of the DTO classes.  Appears in the Usings.")]
        public string DtoNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_repositoryEntitiesNamespace_DEFAULTVALUE, description: "")]
        public string RepositoryEntitiesNamespace { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_automapperProfile);
                string filepath = outputfile;

                var excludedEntityNavigations = ProcessModel.GetAllExcludedEntityNavigations(
                    excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new AutomapperProfileControllerGenerator(inflector: Inflector);
                string generatedCode = generator.GenerateAutomapperProfile(
                                            baseNamespace: BaseNamespace,
                                            namespacePostfix: NamespacePostfix,
                                            dtoNamespace: DtoNamespace,
                                            repositoryEntitiesNamespace: RepositoryEntitiesNamespace,
                                            entityTypes: filteredEntityTypes,
                                            excludedEntityNavigations: excludedEntityNavigations);

                retVal.Files.Add(new OutputFile()
                {
                    Content = generatedCode,
                    Name = filepath
                });

                AddTemplateVariablesManagerErrorsToRetVal(ref retVal, Enums.LogLevel.Error);
            }
            catch (Exception ex)
            {
                base.AddError(ref retVal, ex, Enums.LogLevel.Error);
            }

            return retVal;
        }
    }
}