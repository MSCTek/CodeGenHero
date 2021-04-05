using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.Server;
using System;

namespace CodeGenHero.Template.WebAPI.FullFramework.Server
{
    [Template(name: Consts.TEMPLATE_RepositoryBasePartialMethods, version: "2.0",
        uniqueTemplateIdGuid: "{6420F92E-975B-4C40-B251-D0404C951A5F}",
        description: "Creates sample partial method injections to compliment the base repository class.")]
    public class RepositoryBaseTemplatePartialMethods : BaseAPIFFTemplate
    {
        public RepositoryBaseTemplatePartialMethods()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_DbContextName_DEFAULTVALUE)]
        public string DbContextName { get; set; }

        [TemplateVariable(Consts.OUT_repositoryBasePartialMethods_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string RepositoryBasePartialMethodsOutputFilePath { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_repositoryEntitiesNamespace_DEFAULTVALUE, description: "")]
        public string RepositoryEntitiesNamespace { get; set; }

        [TemplateVariable(Consts.STG_repositoryInterfaceNamespace_DEFAULTVALUE)]
        public string RepositoryInterfaceNamespace { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_repositoryBasePartialMethods);
                string filepath = outputfile;

                var excludedEntityNavigations = ProcessModel.GetAllExcludedEntityNavigations(
                    excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new RepositoryBasePartialMethodsGenerator(inflector: Inflector);
                var generatedCode = generator.GenerateRepositoryBasePartialMethods(
                    namespacePostfix: NamespacePostfix,
                    baseNamespace: BaseNamespace,
                    repositoryInterfaceNamespace: RepositoryInterfaceNamespace,
                    repositoryEntitiesNamespace: RepositoryEntitiesNamespace,
                    efEntityNamespacePrefix: $"ent{NamespacePostfix}",
                    dbContextName: DbContextName,
                    EntityTypes: filteredEntityTypes,
                    excludedEntityNavigations: excludedEntityNavigations
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