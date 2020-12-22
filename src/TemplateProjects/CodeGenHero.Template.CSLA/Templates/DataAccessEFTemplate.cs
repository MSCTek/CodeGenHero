using CodeGenHero.Template.CSLA.Generators;
using System;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.CSLA.Templates
{
    [Template(name: Consts.TEMPLATE_DATAACCESS_EF, version: "1.0",
       uniqueTemplateIdGuid: "{3D7E6300-B238-4349-9FDC-49EC0F871D34}",
       description: "DataAccess models for EntityFramework.")]
    public class DataAccessEFTemplate : BaseCSLATemplate
    {
        public DataAccessEFTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_dataAccessEF_IncludeRelatedObjects_DEFAULTVALUE)]
        public bool DataAccessEF_IncludeRelatedObjects { get; set; }

        [TemplateVariable(Consts.STG_dataAccessEF_Namespace_DEFAULTVALUE)]
        public string DataAccessEFNamespace { get; set; }

        [TemplateVariable(Consts.OUT_dataAccessEF_DEFAULTVALUE, true)]
        public string DataAccessEFOutputFilePath { get; set; }

        [TemplateVariable(Consts.OUT_dataAccessEF_DBContextName_DEFAULTVALUE)]
        public string DBContextName { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                foreach (var entity in ProcessModel.MetadataSourceModel.EntityTypes)
                {
                    string entityName = Inflector.Humanize(entity.ClrType.Name);

                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity, fileName: Consts.OUT_DataAccessEF);
                    outputfile = outputfile.Replace("[entityname]", entityName).Replace("[tablename]", entityName).Replace("[entitynamepluralname]", entityName).Replace("[tablenamepluralname]", entityName);
                    string filepath = outputfile;

                    string useNamespace = TemplateVariablesManager.GetValue(Consts.STG_dataAccessEFNamespace);

                    var generator = new DataAccessEFGenerator(inflector: Inflector);
                    string generatedCode = generator.GenerateDataAccessEF(
                        entity: entity,
                        useNamespace: useNamespace,
                        dbContextName: DBContextName
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