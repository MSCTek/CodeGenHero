using CodeGenHero.Template.WebAPI.FullFramework.Generators.Server;
using System;
using CodeGenHero.Core;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.WebAPI.FullFramework.Server
{
    [Template(name: Consts.TEMPLATE_RepositoryInterfaceCrud, version: "2.0",
        uniqueTemplateIdGuid: "{387DFDC5-B9F2-4AB4-ACCB-00A22CDD916C}",
        description: "Creates the interface that supports create, read, update, and delete functions in the repository class.")]
    public class RepositoryInterfaceCrudTemplate : BaseAPIFFTemplate
    {
        public RepositoryInterfaceCrudTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_DbContextName_DEFAULTVALUE)]
        public string DbContextName { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_repositoryEntitiesNamespace_DEFAULTVALUE, description: "")]
        public string RepositoryEntitiesNamespace { get; set; }

        [TemplateVariable(Consts.OUT_repositoryInterfaceCrud_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string RepositoryInterfaceCrudOutputFilePath { get; set; }

        [TemplateVariable(Consts.STG_repositoryInterfaceNamespace_DEFAULTVALUE)]
        public string RepositoryInterfaceNamespace { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_repositoryInterfaceCrud);
                string filepath = outputfile;

                var generator = new RepositoryInterfaceCrudGenerator(inflector: Inflector);
                string generatedCode = generator.GenerateRepositoryInterfaceCrud(
                    repositoryInterfaceNamespace: RepositoryInterfaceNamespace,
                    namespacePostfix: NamespacePostfix,
                    repositoryEntitiesNamespace: RepositoryEntitiesNamespace,
                    EntityTypes: ProcessModel.MetadataSourceModel.EntityTypes
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