using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.Server;
using System;

namespace CodeGenHero.Template.WebAPI.FullFramework.Server
{
    [Template(name: Consts.TEMPLATE_RepositoryInterface, version: "2.0",
         uniqueTemplateIdGuid: "{C31289E3-93DA-4FDF-BA45-05BC7BCF9204}",
         description: "Creates the interface that the repository class must implement")]
    public class RepositoryInterfaceTemplate : BaseAPIFFTemplate
    {
        public RepositoryInterfaceTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_DbContextName_DEFAULTVALUE)]
        public string DbContextName { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_repositoryEntitiesNamespace_DEFAULTVALUE, description: "")]
        public string RepositoryEntitiesNamespace { get; set; }

        [TemplateVariable(Consts.OUT_repositoryInterface_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string RepositoryInterfaceOutputFilePath { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_repositoryInterface);
                string filepath = outputfile;
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new RepositoryInterfaceGenerator(inflector: Inflector);
                var generatedCode = generator.GenerateRepositoryInterface(
                    namespacePostfix: NamespacePostfix,
                    baseNamespace: BaseNamespace,
                    repositoryEntitiesNamespace: RepositoryEntitiesNamespace,
                    dbContextName: DbContextName,
                    EntityTypes: filteredEntityTypes
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