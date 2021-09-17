using CodeGenHero.Template.Blazor.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor.Templates
{
    [Template(name: "AutoMapperProfile", version: "2021.9.14", uniqueTemplateIdGuid: "7B0AA8DE-D2FB-4EFA-98E1-75FEB116A153",
        description: "Generates an Automapper Profile based off provided Metadata. Requires AutoMapper.Extensions.Microsoft.DependencyInjection NuGet package.")]
    class AutoMapperTemplate : BaseBlazorTemplate
    {
        public AutoMapperTemplate()
        {

        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.AutoMapperOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string AutoMapperOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_AutoMapperName_DEFAULT, description: Consts.PTG_AutoMapperName_DESC)]
        public string AutoMapperProfileClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_EntitiesNamespace_DEFAULT, description: Consts.PTG_EntitiesNamespace_DESC)]
        public string EntitiesNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_MappersNamespace_DEFAULT, description: Consts.PTG_MappersNamespace_DESC)]
        public string MappersNamespace { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_AutoMapperOutputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("AutoMapper"),
                    new NamespaceItem($"xDTO = {DtoNamespace}"),
                    new NamespaceItem($"xENT = {EntitiesNamespace}")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);
                var excludedEntityNavigations = ProcessModel.GetAllExcludedEntityNavigations(RegexExclude, RegexInclude);

                var generator = new AutoMapperGenerator(inflector: Inflector);
                var generatedCode = generator.Generate(usings, MappersNamespace, NamespacePostfix, entities, excludedEntityNavigations);

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
