using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.DTO;
using System;

namespace CodeGenHero.Template.WebAPI.FullFramework.DTO
{
    [Template(name: Consts.TEMPLATE_DTO, version: "2.0",
       uniqueTemplateIdGuid: "{0D58C309-4B48-4464-819D-83C9C87CF463}",
       description: "Simple Data Transfer Objects for use in communicating across process boundaries.  Ideal for sending results from a server-side Web API to a consuming client application (i.e. Xamarin mobile or ASP.Net website).")]
    public class DTOSimpleTemplate : BaseAPIFFTemplate
    {
        public DTOSimpleTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_dtoIncludeRelatedObjects_DEFAULTVALUE, description: "Determines whether to include related objects.")]
        public bool DTOIncludeRelatedObjects { get; set; }

        [TemplateVariable(Consts.STG_dtoNamespace_DEFAULTVALUE, description: "The namespace to use for the DTO classes.")]
        public string DTONamespace { get; set; }

        [TemplateVariable(Consts.OUT_dto_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string DTOOutputFilePath { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var excludedEntityNavigations = ProcessModel.GetAllExcludedEntityNavigations(
                    excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);
                foreach (var entityType in filteredEntityTypes)
                {
                    string entityName = Inflector.Humanize(entityType.ClrType.Name);

                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity, fileName: Consts.OUT_dto);
                    outputfile = outputfile.Replace("[tablename]", entityName).Replace("[tablepluralname]", Inflector.Pluralize(entityName));
                    string filepath = outputfile;

                    var generator = new DTOSimpleGenerator(inflector: Inflector);
                    string generatedCode = generator.GenerateDTO(
                        excludedEntityNavigations: excludedEntityNavigations,
                        entity: entityType,
                        namespacePostfix: NamespacePostfix,
                        baseNamespace: BaseNamespace,
                        dtoIncludeRelatedObjects: DTOIncludeRelatedObjects,
                        prependSchemaNameIndicator: PrependSchemaNameIndicator,
                        dtoNamespace: DTONamespace
                    );

                    retVal.Files.Add(new OutputFile()
                    {
                        Content = generatedCode,
                        Name = filepath
                    });
                }

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