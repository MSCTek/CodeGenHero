using CodeGenHero.Template.Blazor.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor.Templates
{
    [Template(name: "Repository", version: "2021.9.14", uniqueTemplateIdGuid: "F51D4B1E-D1F8-45C4-BC9C-9E50B9090FBE",
        description: "Generates the Repository class. Can be used as a Base for custom Repository classes.")]
    class RepositoryTemplate : BaseBlazorTemplate
    {
        public RepositoryTemplate()
        {

        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.RepositoryOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string RepositoryOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryName_DEFAULT, description: Consts.PTG_RepositoryName_DESC)]
        public string RepositoryClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryInterfaceClassName_DEFAULT, description: Consts.PTG_RepositoryInterfaceClassName_DESC)]
        public string RepositoryInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryNamespace_DEFAULT, description: Consts.PTG_RepositoryNamespace_DESC)]
        public string RepositoryNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_EntitiesNamespace_DEFAULT, description: Consts.PTG_EntitiesNamespace_DESC)]
        public string EntitiesNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DbContextName_DEFAULT, description: Consts.PTG_DbContextName_DESC)]
        public string DbContextName { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_RepositoryOutputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Linq"),
                    new NamespaceItem("System.Threading.Tasks"),
                    new NamespaceItem("Microsoft.EntityFrameworkCore"),
                    new NamespaceItem("CodeGenHero.Repository"),
                    new NamespaceItem("cghEnums = CodeGenHero.Repository.Enums"),
                    new NamespaceItem(EntitiesNamespace),
                    new NamespaceItem($"waEnums = {BaseNamespace}.Shared.Constants.Enums")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new RepositoryGenerator(inflector: Inflector);
                string generatedCode = generator.Generate(usings, RepositoryNamespace, NamespacePostfix, entities, RepositoryClassName, RepositoryInterfaceClassName, DbContextName);

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
