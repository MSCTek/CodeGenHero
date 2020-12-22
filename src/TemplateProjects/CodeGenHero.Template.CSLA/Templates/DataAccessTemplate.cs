using CodeGenHero.Core;
using CodeGenHero.Template.CSLA.Generators;
using CodeGenHero.Template.Models;
using System;

namespace CodeGenHero.Template.CSLA.Templates
{
    [Template(name: "DataAccessTemplate", version: "1.0",
       uniqueTemplateIdGuid: "{E6AA865F-F8B6-410B-8955-FDDD36F70E64}",
       description: "A template for generating a POCO class from metadata objects")]
    public class DataAccessTemplate : BaseCSLATemplate
    {
        private string _classNameSpace = null;

        public DataAccessTemplate()
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

        [TemplateVariable(Consts.OUT_Blazor_DataAccess_DEFAULTVALUE, true)]
        public string DataAccessOutputFilePath { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();
            try
            {
                foreach (var entity in ProcessModel.MetadataSourceModel.EntityTypes)
                {
                    string entityName = Inflector.Humanize(entity.ClrType.Name);

                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                        fileName: Consts.OUT_Blazor_DataAccess);
                    outputfile = outputfile.Replace("[entityname]", $"{entityName}Entity");
                    string filepath = outputfile;

                    var generator = new DataAccessGenerator(inflector: Inflector);
                    string generatedCode = generator.GenerateEntity(
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