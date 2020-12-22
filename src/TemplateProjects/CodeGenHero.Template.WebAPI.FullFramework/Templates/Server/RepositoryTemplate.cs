using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.Server;
using System;
using CodeGenHero.Core;
using CodeGenHero.Inflector;

namespace CodeGenHero.Template.WebAPI.FullFramework.Server
{
    [Template(name: Consts.TEMPLATE_Repository, version: "2.0",
        uniqueTemplateIdGuid: "{1CF6C29C-C8CF-4620-85D8-9122C2C05091}",
        description: "Creates the repository class where options can be set on the Entity Framework data context (i.e. disable lazy loading or log EF queries when a debugger is attached).")]
    public class RepositoryTemplate : BaseAPIFFTemplate
    {
        public RepositoryTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_DbContextName_DEFAULTVALUE)]
        public string DbContextName { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_repositoryEntitiesNamespace_DEFAULTVALUE, description: "")]
        public string RepositoryEntitiesNamespace { get; set; }

        [TemplateVariable(Consts.STG_repositoryInterfaceNamespace_DEFAULTVALUE)]
        public string RepositoryInterfaceNamespace { get; set; }

        [TemplateVariable(Consts.OUT_repository_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string RepositoryOutputFilePath { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_repository);
                string filepath = outputfile;

                var generator = new RepositoryGenerator(inflector: Inflector);
                string generatedCode = generator.GenerateRepository(
                    baseNamespace: BaseNamespace,
                    namespacePostfix: NamespacePostfix,
                    repositoryEntitiesNamespace: RepositoryEntitiesNamespace,
                    dbContextName: DbContextName,
                    repositoryInterfaceNamespace: RepositoryInterfaceNamespace);

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