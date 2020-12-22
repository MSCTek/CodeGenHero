using CodeGenHero.Template.CSLA.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.CSLA.Templates
{
    [Template(name: "CSLAEditBusiness", version: "1.0",
       uniqueTemplateIdGuid: "{A3332222-EEA5-4AC2-8454-F89714220BE4}",
       description: "A template based on BusinessBase")]
    public class CSLAEditBusinessTemplate : BaseCSLATemplate
    {
        private string _classNameSpace = null;

        public CSLAEditBusinessTemplate()
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

        [TemplateVariable(Consts.OUT_Blazor_DataAccess_CSLA_ReadOnlyEditBusiness_DEFAULTVALUE, true)]
        public string CSLAEditBusinessOutputFilePath { get; set; }

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
                        fileName: Consts.OUT_Blazor_CSLA_EditBusiness);
                    outputfile = outputfile.Replace("[entityname]", $"{entityName}");
                    string filepath = outputfile;

                    var generator = new CSLAEditBusinessGenerator(inflector: Inflector);
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