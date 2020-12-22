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
    [Template(name: Consts.TEMPLATE_MapperDtoToSqliteModelDataAndMvvmLightModelObject, version: "2.0",
     uniqueTemplateIdGuid: "{5078579F-7AB4-430D-9DCC-A3190F678EFC}",
     description: "Maps DTO objects to SQLite model data objects as well as MvvmLight objects and vice versa.")]
    public class MapperDtoToSqliteModelDataAndMvvmLightModelObjectTemplate : BaseAPIFFTemplate
    {
        public MapperDtoToSqliteModelDataAndMvvmLightModelObjectTemplate()
        {
        }

        [TemplateVariable(Consts.STG_dtoNamespace_DEFAULTVALUE, description: "Namespace of the DTO classes.  Appears in the Usings.")]
        public string DtoNamespace { get; set; }

        [TemplateVariable(Consts.OUT_mapperDtoToSqliteModelDataAndMvvmLightModelObject_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string MapperDtoToSqliteModelDataAndMvvmLightModelObjectOutputFilePath { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelDataNamespace_DEFAULTVALUE, description: "Namespace of the Sqllite Model classes.  Appears in the Usings.")]
        public string SqliteModelDataNamespace { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelMapperClassName_DEFAULTVALUE, description: "Classname for the ModelMapper class. Required in order to convert data between classes.")]
        public string SqliteModelMapperClassName { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelMapperClassNamespace_DEFAULTVALUE, description: "")]
        public string SqliteModelMapperClassNamespace { get; set; }

        [TemplateVariable(Consts.STG_sqliteModelObjectNamespace_DEFAULTVALUE)]
        public string SqliteModelObjectNamespace { get; set; }

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_mapperDtoToSqliteModelDataAndMvvmLightModelObject);
                string filepath = outputfile;

                string modelObjNamespacePrefix = $"obj{NamespacePostfix}";
                string modelDataNamespacePrefix = $"data{NamespacePostfix}";
                string modelDtoNamespacePrefix = $"dto{NamespacePostfix}";
                string regexExclude = RegexExclude;

                var usings = new List<string>
            {
                $"using {modelObjNamespacePrefix} = {SqliteModelObjectNamespace};",
                $"using {modelDataNamespacePrefix} = {SqliteModelDataNamespace};",
                $"using {modelDtoNamespacePrefix} = {DtoNamespace};"
            };

                var entityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);
                var generator = new MapperDtoToSqliteModelDataAndMvvmLightModelObjectGenerator(inflector: Inflector);
                string generatedCode = generator.GenerateMapperDtoTosqliteModelDataAndMvvmLightModelObject(
                    usings: usings,
                    classNamespace: SqliteModelMapperClassNamespace,
                    className: SqliteModelMapperClassName,
                    modelObjNamespacePrefix: modelObjNamespacePrefix,
                    modelDataNamespacePrefix: modelDataNamespacePrefix,
                    modelDtoNamespacePrefix: modelDtoNamespacePrefix,
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