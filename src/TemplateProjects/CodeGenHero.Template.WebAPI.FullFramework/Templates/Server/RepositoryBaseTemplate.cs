using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.Server;
using System;

namespace CodeGenHero.Template.WebAPI.FullFramework.Server
{
    [Template(name: Consts.TEMPLATE_RepositoryBase, version: "2.0",
        uniqueTemplateIdGuid: "{4ECCA302-133E-48BC-BDA9-5F495B9FDD26}",
        description: "Creates the base repository class where most of the actual implementation of CRUD operations resides.")]
    public class RepositoryBaseTemplate : BaseAPIFFTemplate
    {
        public RepositoryBaseTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_DbContextName_DEFAULTVALUE)]
        public string DbContextName { get; set; }

        [TemplateVariable(Consts.OUT_repositoryBase_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string RepositoryBaseOutputFilePath { get; set; }

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
                    fileName: Consts.OUT_repositoryBase);
                string filepath = outputfile;
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new RepositoryBaseGenerator(inflector: Inflector);
                var generatedCode = generator.GenerateRepositoryBase(
                    repositoryInterfaceNamespace: RepositoryInterfaceNamespace,
                    repositoryEntitiesNamespace: RepositoryEntitiesNamespace,
                    dbContextName: DbContextName,
                    namespacePostfix: NamespacePostfix,
                    baseNamespace: BaseNamespace,
                    entityTypes: filteredEntityTypes
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