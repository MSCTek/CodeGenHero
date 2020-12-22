using CodeGenHero.Template.CSLA.Generators;
using System;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.CSLA.Templates
{
    [Template(name: Consts.TEMPLATE_DATAACCESS_MOCK, version: "1.0",
       uniqueTemplateIdGuid: "{E1C2ABD7-8077-41D3-A5EF-D6D9EF54A2E2}",
       description: "Mock DataAccess models for EntityFramework, for testing.")]
    public class DataAccessMockTemplate : BaseCSLATemplate
    {
        public DataAccessMockTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_dataAccessMock_IncludeRelatedObjects_DEFAULTVALUE)]
        public bool DataAccessMock_IncludeRelatedObjects { get; set; }

        [TemplateVariable(Consts.STG_dataAccessMock_Namespace_DEFAULTVALUE)]
        public string DataAccessMockNamespace { get; set; }

        [TemplateVariable(Consts.OUT_dataAccessMock_DEFAULTVALUE, true)]
        public string DataAccessMockOutputFilePath { get; set; }

        [TemplateVariable(Consts.OUT_dataAccessMock_DBContextName_DEFAULTVALUE)]
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

                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity, fileName: Consts.OUT_DataAccessMock);
                    outputfile = outputfile.Replace("[tablename]", entityName).Replace("[tablepluralname]", entityName);
                    string filepath = outputfile;

                    string useNamespace = TemplateVariablesManager.GetValue(Consts.STG_dataAccessMockNamespace);

                    var generator = new DataAccessEFMockGenerator(inflector: Inflector);
                    string generatedCode = generator.GenerateDataAccessMock(
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