using CodeGenHero.Template.Blazor.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor.Templates
{
    [Template(name: "GenericFactoryInterface", version: "2021.9.14", uniqueTemplateIdGuid: "2BED5905-CED6-4CE8-93EE-EB63365B87EC",
        description: "Generates an interface for a Factory class that maps Entities to DTOs. Implies use of AutoMapper template and associated prerequisites.")]
    class GenericFactoryInterfaceTemplate : BaseBlazorTemplate
    {
        public GenericFactoryInterfaceTemplate()
        {

        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.GenericFactoryInterfaceOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string GenericFactoryInterfaceOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_GenericFactoryInterfaceName_DEFAULT, description: Consts.PTG_GenericFactoryInterfaceName_DESC)]
        public string GenericFactoryInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_MappersNamespace_DEFAULT, description: Consts.PTG_MappersNamespace_DESC)]
        public string MappersNamespace { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_GenericFactoryInterfaceOutputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("System.Collections.Generic")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new GenericFactoryInterfaceGenerator(inflector: Inflector);
                var generatedCode = generator.Generate(usings, MappersNamespace, NamespacePostfix, GenericFactoryInterfaceClassName);

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
