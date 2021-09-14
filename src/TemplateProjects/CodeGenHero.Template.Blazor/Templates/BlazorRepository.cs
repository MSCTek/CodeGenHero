using CodeGenHero.Template.Blazor.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor.Templates
{
    [Template(name: "BlazorRepository", version: "1.0", uniqueTemplateIdGuid: "9E1967A4-B36C-4318-8282-F6066E43283C",
        description: "Creates standard repositories to be accessed via API.")]
    public class BlazorRepository : BaseBlazorTemplate
    {
        public BlazorRepository()
        {

        }

        #region TemplateVariables

        [TemplateVariable(Consts.BlazorRepositoryClassNamespace_DEFAULTVALUE, description: Consts.BlazorRepositoryClassNamespace_DEFAULTDESCRIPTION)]
        public string BlazorRepositoryClassNamespace { get; set; }

        [TemplateVariable(Consts.BlazorRepositoryEntitiesNamespace_DEFAULTVALUE, description: Consts.BlazorRepositoryEntitiesNamespace_DEFAULTDESCRIPTION)]
        public string EntitiesNamespace { get; set; }

        [TemplateVariable(Consts.BlazorRepositoryDbContextClassName_DEFAULTVALUE, description: Consts.BlazorRepositoryDbContextClassName_DEFAULTDESCRIPTION)]
        public string DbContextClassName { get; set; }

        [TemplateVariable(Consts.OUT_BlazorRepositoryImplementationFilePath_DEFAULTVALUE, hiddenIndicator: true)]
        public string BlazorRepositoryImplementationOutputFilePath { get; set; }

        [TemplateVariable(Consts.OUT_BlazorRepositoryInterfaceFilePath_DEFAULTVALUE, hiddenIndicator: true)]
        public string BlazorRepositoryInterfaceOutputFilePath { get; set; }

        [TemplateVariable(Consts.OUT_BlazorRepositoryBaseFilePath_DEFAULTVALUE, hiddenIndicator: true)]
        public string BlazorRepositoryBaseOutputFilePath { get; set; }

        [TemplateVariable(defaultValue: null,
            description: "A list of MSC.CodeGenHero.DTO.NameValue items serialized as JSON that correspond to table names and integer values for the maximum number of rows to return for a single request for a page of data.")]
        public string MaxRequestPerPageOverrideByTableName { get; set; }

        [TemplateVariable(defaultValue: null,
            description:Consts.BlazorRepositoryChildIncludesByTableName_DEFAULTDESCRIPTION)]
        public string ChildIncludesByTableName { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var generator = new BlazorRepositoryGenerator(inflector: Inflector);
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                #region NameValues
                var maxRequestPerPageOverrides = DeserializeJsonObject<List<NameValue>>(MaxRequestPerPageOverrideByTableName);
                if (maxRequestPerPageOverrides == null)
                {
                    maxRequestPerPageOverrides = new List<NameValue>();
                }

                var childIncludes = DeserializeJsonObject<List<NameValue>>(ChildIncludesByTableName);
                if (childIncludes == null)
                {
                    childIncludes = new List<NameValue>();
                }

                #endregion

                #region Base Repository

                string baseOutputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.BlazorRepository_Base_OutFileVariableName);
                string baseFilepath = baseOutputFile;

                var baseUsings = new List<NamespaceItem>
                {
                    new NamespaceItem(EntitiesNamespace)
                };

                string generatedBaseClassCode = generator.GenerateBaseClass(baseUsings, BlazorRepositoryClassNamespace, NamespacePostfix, EntitiesNamespace, DbContextClassName, PrependSchemaNameIndicator);

                retVal.Files.Add(new OutputFile()
                {
                    Content = generatedBaseClassCode,
                    Name = baseFilepath
                });

                #endregion

                foreach (var entity in filteredEntityTypes)
                {
                    
                    // Generate the Interface
                    #region Interface
                    string interfaceOutputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                        fileName: Consts.BlazorRepository_Interface_OutFileVariableName);
                    interfaceOutputfile = FilepathTokenReplacements(interfaceOutputfile, entity);
                    string interfaceFilepath = interfaceOutputfile;

                    var interfaceUsings = new List<NamespaceItem>
                    {
                        new NamespaceItem($"System.Collections.Generic"),
                        new NamespaceItem("System"),
                        new NamespaceItem(EntitiesNamespace)
                    };

                    string generatedInterfaceCode = generator.GenerateInterface(interfaceUsings, BlazorRepositoryClassNamespace, NamespacePostfix, EntitiesNamespace, DbContextClassName, PrependSchemaNameIndicator, entity);

                    retVal.Files.Add(new OutputFile()
                    {
                        Content = generatedInterfaceCode,
                        Name = interfaceFilepath
                    });

                    #endregion

                    // Generate the Implementation
                    #region Implementation
                    string implementationOutputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.BlazorRepository_Implementation_OutFileVariableName);
                    implementationOutputfile = FilepathTokenReplacements(implementationOutputfile, entity);
                    string implementationFilepath = implementationOutputfile;

                    var implementationUsings = new List<NamespaceItem>
                    {
                        new NamespaceItem("Microsoft.EntityFrameworkCore"),
                        new NamespaceItem("System.Collections.Generic"),
                        new NamespaceItem("System.Linq"),
                        new NamespaceItem("System"),
                        new NamespaceItem(EntitiesNamespace)
                    };

                    string generatedImplementationCode = generator.GenerateImplementation(implementationUsings, BlazorRepositoryClassNamespace, NamespacePostfix, maxRequestPerPageOverrides, childIncludes, EntitiesNamespace, DbContextClassName, PrependSchemaNameIndicator, entity);

                    retVal.Files.Add(new OutputFile()
                    {
                        Content = generatedImplementationCode,
                        Name = implementationFilepath
                    });

                    #endregion
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
