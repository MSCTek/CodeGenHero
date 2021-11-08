using CodeGenHero.Template.Blazor.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor.Templates
{
    class DTOTemplate : BaseBlazorTemplate
    {
        public DTOTemplate()
        {

        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.DTOFilePath_DEFAULTVALUE, hiddenIndicator: true)]
        public string DTOOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: "true", description: "Determines whether to include related objects.")]
        public bool IncludeRelatedObjects { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var excludedEntityNavigations = ProcessModel.GetAllExcludedEntityNavigations(
                    excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var usings = new List<NamespaceItem>(); // Empty, but we need to provide one.

                foreach (var entityType in filteredEntityTypes)
                {
                    var className = Inflector.Pascalize(entityType.ClrType.Name);
                    string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_DTOFilePath_DEFAULTVALUE);
                    string filepath = TokenReplacements(outputFile, entityType);

                    var generator = new DTOGenerator(inflector: Inflector);
                    var generatedCode = generator.Generate(usings, DtoNamespace, NamespacePostfix, entityType, excludedEntityNavigations, IncludeRelatedObjects, className);

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
