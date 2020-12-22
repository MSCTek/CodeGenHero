using CodeGenHero.Core;
using CodeGenHero.Template.CSLA.Generators;
using CodeGenHero.Template.Models;
using System;

namespace CodeGenHero.Template.CSLA.Templates
{
    [Template(name: "CSLAReadOnlyList", version: "1.0",
       uniqueTemplateIdGuid: "{A2EF9017-43AC-4B1F-8B2C-1480E15B47CC}",
       description: "A template based on CSLA List")]
    public class CSLAReadOnlyListTemplate : BaseCSLATemplate
    {
        private string _classNameSpace = null;

        public CSLAReadOnlyListTemplate()
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

        [TemplateVariable(Consts.OUT_Blazor_DataAccess_CSLA_ReadOnlyList_DEFAULTVALUE, true)]
        public string CSLAReadOnlyListOutputFilePath { get; set; }

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
                        fileName: Consts.OUT_Blazor_CSLA_ReadOnly_List);
                    outputfile = outputfile.Replace("[entityname]", $"{entityName}");
                    string filepath = outputfile;

                    var generator = new CSLAReadOnlyListGenerator(inflector: Inflector);
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