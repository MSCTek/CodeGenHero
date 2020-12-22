using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.Inflector;
using CodeGenHero.Core;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.MVVM;

namespace CodeGenHero.Template.WebAPI.FullFramework.MVVM
{
    [Template(name: Consts.TEMPLATE_MapperSqliteModelDataToMvvmLightModelObject, version: "2.0",
        uniqueTemplateIdGuid: "{77D31B89-1034-451E-AF7C-9ED1FA3613EF}",
        description: "Maps SQLite model data objects to MvvmLight objects and vice versa.")]
    public class MapperSqliteModelDataToMvvmLightModelObject : BaseAPIFFTemplate
    {
        public MapperSqliteModelDataToMvvmLightModelObject()
        {
        }

        [TemplateVariable(Consts.OUT_mapperSqliteModelDataToMvvmLightModelObject_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string MapperSqliteModelDataToMvvmLightModelObjectOutputFilePath { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelDataNamespace_DEFAULTVALUE, description: "Namespace of the Sqllite Model classes.  Appears in the Usings.")]
        public string SqliteModelDataNamespace { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelMapperClassName_DEFAULTVALUE)]
        public string SqliteModelMapperClassName { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelMapperClassNamespace_DEFAULTVALUE)]
        public string SqliteModelMapperClassNamespace { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelObjectNamespace_DEFAULTVALUE)]
        public string SqliteModelObjectNamespace { get; set; }

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_mapperSqliteModelDataToMvvmLightModelObject);
                string filepath = outputfile;

                string modelObjNamespacePrefix = $"obj{NamespacePostfix}";
                string modelDataNamespacePrefix = $"data{NamespacePostfix}";

                var entityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);
                var generator = new MapperSqliteModelDataToMvvmLightModelObjectGenerator(inflector: Inflector);
                string generatedCode = generator.GenerateMapperSqliteModelDataToMvvmLightModelObject(
                    classNamespace: SqliteModelMapperClassNamespace,
                    className: SqliteModelMapperClassName,
                    modelObjNamespacePrefix: modelObjNamespacePrefix,
                    modelObjNamespace: SqliteModelObjectNamespace,
                    modelDataNamespacePrefix: modelDataNamespacePrefix,
                    modelDataNamespace: SqliteModelDataNamespace,
                    prependSchemaNameIndicator: PrependSchemaNameIndicator,
                   entityTypes: entityTypes
                 );

                retVal.Files.Add(new OutputFile()
                {
                    Content = generatedCode,
                    Name = filepath
                });
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