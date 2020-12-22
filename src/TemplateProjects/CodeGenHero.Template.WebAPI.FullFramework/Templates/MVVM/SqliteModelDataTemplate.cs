using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.MVVM;
using System;
using CodeGenHero.Core;
using CodeGenHero.Inflector;
using CodeGenHero.Core;

namespace CodeGenHero.Template.WebAPI.FullFramework.MVVM
{
    [Template(name: Consts.TEMPLATE_SqliteModelData, version: "2.0",
        uniqueTemplateIdGuid: "{9EE14087-7B71-436B-B78A-47E5FF61A2D3}",
        description: "Provides the classes necessary to read/write data from/into a SQLite database for persistence.")]
    public class SqliteModelDataTemplate : BaseAPIFFTemplate
    {
        public SqliteModelDataTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_sqliteModelDataBaseAuditEditNamespace_DEFAULTVALUE)]
        public string SqliteModelDataBaseAuditEditNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.STG_sqliteModelDataNamespace_DEFAULTVALUE, description: "Namespace of the Sqllite Model classes.  Appears in the Usings.")]
        public string SqliteModelDataNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.OUT_sqliteModelData_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string SqliteModelDataOutputFilePath { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var entityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);

                foreach (var entity in entityTypes)
                {
                    string entityName = Inflector.Pascalize(entity.ClrType.Name);
                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                        fileName: Consts.OUT_sqliteModelData);
                    outputfile = outputfile.Replace("[tablename]", entityName).Replace("[tablepluralname]", Inflector.Pluralize(entityName));
                    string filepath = outputfile;

                    var generator = new SqliteModelDataGenerator(inflector: Inflector);
                    string generatedCode = generator.GenerateSqliteModelData(
                        classNamespace: SqliteModelDataNamespace,
                        baseAuditEditNamespace: SqliteModelDataBaseAuditEditNamespace,
                        prependSchemaNameIndicator: PrependSchemaNameIndicator,
                        entity: entity);

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