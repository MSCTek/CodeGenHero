using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.MVVM;
using System;

namespace CodeGenHero.Template.WebAPI.FullFramework.MVVM
{
    [Template(name: Consts.TEMPLATE_MvvmLightModelObject, version: "2.0",
         uniqueTemplateIdGuid: "{13213EE9-7E50-4DBD-82EB-9030FAD28B26}",
         description: "MvvmLight Model Objects).")]
    public class MvvmLightModelObjectTemplate : BaseAPIFFTemplate
    {
        private string _sqliteModelObjectNamespace = null;

        public MvvmLightModelObjectTemplate()
        {
        }

        [TemplateVariable(Consts.OUT_mvvmLightModelObject_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string MvvmLightModelObjectOutputFilePath { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelObjectBaseAuditEditNamespace_DEFAULTVALUE)]
        public string SqliteModelObjectBaseAuditEditNamespace { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelObjectNamespace_DEFAULTVALUE)]
        public string SqliteModelObjectNamespace
        {
            get
            {
                return _sqliteModelObjectNamespace;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.EndsWith("."))
                {   // Strip off trailing period.  This can happen if the user did not set a namespace postfix in the blueprint.
                    _sqliteModelObjectNamespace = value.Substring(0, value.Length - 1);
                }
                else
                {
                    _sqliteModelObjectNamespace = value;
                }
            }
        }

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                if (retVal.Errors.Count > 0)
                {
                    return retVal;
                }

                // Determine the navigation properties that should be excluded due to the RegEx patterns.
                var excludedEntityNavigations = ProcessModel.GetAllExcludedEntityNavigations(
                    excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);
                foreach (var entityType in filteredEntityTypes)
                {
                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                        fileName: Consts.OUT_mvvmLightModelObject);
                    string entityName = Inflector.Humanize(entityType.ClrType.Name);
                    outputfile = outputfile.Replace("[tablename]", entityName);
                    string filepath = outputfile;

                    var generator = new MvvmLightModelObjectGenerator(inflector: Inflector);
                    string generatedCode = generator.GenerateMvvmLightModelObject(
                        classNamespace: SqliteModelObjectNamespace,
                        baseAuditEditNamespace: SqliteModelObjectBaseAuditEditNamespace,
                        prependSchemaNameIndicator: PrependSchemaNameIndicator,
                         entity: entityType,
                        excludedEntityNavigations: excludedEntityNavigations);
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