using CodeGenHero.Template.Blazor.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor.Templates
{
    [Template(name: "APIController", version: "2021.9.14", uniqueTemplateIdGuid: "70A21A48-7EE1-42F5-B1EB-4891E290A17D", 
        description: "Creates standard API Controllers to perform CRUD operations on Metadata-provided Entities.")]
    public class APIControllerTemplate : BaseBlazorTemplate
    {
        public APIControllerTemplate()
        {

        }

        #region TemplateVariables

        [TemplateVariable(Consts.APIControllerFilePath_DEFAULTVALUE, hiddenIndicator: true)]
        public string APIControllerOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: null,
            description: "A list of MSC.CodeGenHero.DTO.NameValue items serialized as JSON that correspond to table names and integer values for the maximum number of rows to return for a single request for a page of data.")]
        public string MaxRequestPerPageOverrideByTableName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_APIControllerNamespace_DEFAULT, description: Consts.PTG_APIControllerNamespace_DESC)]
        public string APIControllerNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryNamespace_DEFAULT, description: Consts.PTG_RepositoryNamespace_DESC)]
        public string RepositoryNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_EntitiesNamespace_DEFAULT, description: Consts.PTG_EntitiesNamespace_DESC)]
        public string EntitiesNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            var maxRequestPerPageOverrides =
                    DeserializeJsonObject<List<NameValue>>(MaxRequestPerPageOverrideByTableName);

            if (maxRequestPerPageOverrides == null)
            {
                maxRequestPerPageOverrides = new List<NameValue>();
            }

            try
            {
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                foreach(var entity in filteredEntityTypes)
                {
                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_APIControllerFilePath_DEFAULTVALUE);
                    outputfile = outputfile.Replace("[tablename]", Inflector.Humanize(entity.ClrType.Name)).Replace("[tablepluralname]", Inflector.Pluralize(entity.ClrType.Name));
                    string filepath = outputfile;

                    var usings = new List<NamespaceItem>
                    {
                        new NamespaceItem("CodeGenHero.Repository"),
                        new NamespaceItem("Marvin.JsonPatch"),
                        new NamespaceItem("Microsoft.AspNetCore.Http"),
                        new NamespaceItem("Microsoft.AspNetCore.Mvc"),
                        new NamespaceItem("Microsoft.AspNetCore.Routing"),
                        new NamespaceItem("Microsoft.EntityFrameworkCore"),
                        new NamespaceItem("Microsoft.Extensions.Logging"),
                        new NamespaceItem("MSC.WhittierArtists.Api.Infrastructure"),
                        new NamespaceItem("MSC.WhittierArtists.Repository.Mappers"),
                        new NamespaceItem("MSC.WhittierArtists.Repository.Repositories"),
                        new NamespaceItem("MSC.WhittierArtists.Shared.DTO"),
                        new NamespaceItem("System"),
                        new NamespaceItem("System.Collections.Generic"),
                        new NamespaceItem("System.Linq"),
                        new NamespaceItem("System.Threading.Tasks"),
                        new NamespaceItem("cghrEnums = CodeGenHero.Repository.Enums"),
                        new NamespaceItem($"dto{NamespacePostfix} = {DtoNamespace}"),
                        new NamespaceItem($"ent{NamespacePostfix} = {EntitiesNamespace}"),
                        new NamespaceItem("waEnums = MSC.WhittierArtists.Shared.Constants.Enums")
                    };

                    var generator = new APIControllerGenerator(inflector: Inflector);
                    string generatedCode = generator.Generate(usings, APIControllerNamespace, NamespacePostfix, entity, maxRequestPerPageOverrides);

                    retVal.Files.Add(new OutputFile()
                    {
                        Content = generatedCode,
                        Name = filepath
                    });
                }
            }
            catch(Exception ex)
            {
                base.AddError(ref retVal, ex, Enums.LogLevel.Error);
            }

            AddTemplateVariablesManagerErrorsToRetVal(ref retVal, Enums.LogLevel.Error);
            return retVal;
        }
    }
}
