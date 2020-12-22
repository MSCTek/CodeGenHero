using CodeGenHero.Core;
using CodeGenHero.Template.CSLA.Generators;
using CodeGenHero.Template.Models;
using System;

namespace CodeGenHero.Template.CSLA.Templates
{
    [Template(name: "CSLAReadOnlyInfo", version: "1.0",
       uniqueTemplateIdGuid: "{CD5502CC-A7A2-4836-8244-67A64B9125B3}",
       description: "A template based on CSLA Info")]
    public class CSLAReadOnlyInfoTemplate : BaseCSLATemplate
    {
        private string _classNameSpace = null;

        public CSLAReadOnlyInfoTemplate()
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

        [TemplateVariable(Consts.OUT_Blazor_DataAccess_CSLA_ReadOnlyInfo_DEFAULTVALUE, true)]
        public string CSLAReadOnlyInfoOutputFilePath { get; set; }

        [TemplateVariable(Consts.STG_dataAccessIncludeRelatedObjects_DEFAULTVALUE)]
        public bool DataAccessIncludeRelatedObjects { get; set; }

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
                        fileName: Consts.OUT_Blazor_CSLA_ReadOnly_Info);
                    outputfile = outputfile.Replace("[entityname]", $"{entityName}");
                    string filepath = outputfile;

                    var generator = new CSLAReadOnlyInfoGenerator(inflector: Inflector);
                    string generatedCode = generator.GenerateActions(
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