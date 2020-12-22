using CodeGenHero.Core;
using CodeGenHero.Template.CSLA.Generators;
using CodeGenHero.Template.Models;
using System;

namespace CodeGenHero.Template.CSLA.Templates
{
    [Template(name: "DataAccessInterface", version: "1.0",
          uniqueTemplateIdGuid: "{CAFAEB1F-E98A-4624-9DA7-DD7032E9B8A7}",
          description: "Creates a data access interface for a POCO object")]
    public class DataAccessInterfaceTemplate : BaseCSLATemplate
    {
        private string _classNameSpace = null;

        public DataAccessInterfaceTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.BlazorDataAccessNamespace_DEFAULTVALUE)]
        public string ClassNameSpace
        {
            get
            {
                return _classNameSpace;
            }
            set
            {
                _classNameSpace = value;
            }
        }

        [TemplateVariable(Consts.STG_dataAccessIncludeRelatedObjects_DEFAULTVALUE)]
        public bool DataAccessIncludeRelatedObjects { get; set; }

        [TemplateVariable(Consts.OUT_Blazor_DataAccess_Interface_DEFAULTVALUE, true)]
        public string DataAccessInterfaceOutputFilePath { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var generator = new DataAccessInterfaceGenerator(inflector: Inflector);
                foreach (var entity in ProcessModel.MetadataSourceModel.EntityTypes)
                {
                    string entityName = Inflector.Humanize(entity.ClrType.Name);

                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                        fileName: Consts.OUT_Blazor_DataAccessInterface);
                    outputfile = outputfile.Replace("[entityname]", $"{entityName}");
                    string filepath = outputfile;

                    string generatedCode = generator.GenerateInterface(
                        baseNamespace: BaseNamespace,
                        namespacePostfix: NamespacePostfix,
                        classNamespace: ClassNameSpace,
                         InludeRelatedObjects: DataAccessIncludeRelatedObjects,
                        prependSchemaNameIndicator: PrependSchemaNameIndicator,
                        excludeCircularReferenceNavigationProperties: ProcessModel.ExcludedNavigationProperties,
                        entity: entity
                        );

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