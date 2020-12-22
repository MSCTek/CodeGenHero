using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.Inflector;
using CodeGenHero.Core;
using CodeGenHero.Template.WebAPI.FullFramework.Generators.DTO;

namespace CodeGenHero.Template.WebAPI.FullFramework.DTO
{
    [Template(name: Consts.TEMPLATE_ModelsBackedByDtoTemplate, version: "2.0",
        uniqueTemplateIdGuid: "{DF0BF2C5-1BD8-49E2-B43C-668407F6587E}",
        description: "Models Backed By Dto Objects).")]
    public class ModelsBackedByDtoTemplate : BaseAPIFFTemplate
    {
        private string _classNameSpace = null;

        private string _modelsBackedByDtoInterfaceNamespace;

        public ModelsBackedByDtoTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.STG_ModelsBackedByDtoNamespace_DEFAULTVALUE)]
        public string ClassNameSpace
        {
            get
            {
                return _classNameSpace;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.EndsWith("."))
                {   // Strip off trailing period.  This can happen if the user did not set a namespace postfix in the blueprint.
                    _classNameSpace = value.Substring(0, value.Length - 1);
                }
                else
                {
                    _classNameSpace = value;
                }
            }
        }

        [TemplateVariable(Consts.STG_ModelsBackedByDtoInterfaceNamespace_DEFAULTVALUE)]
        public string ModelsBackedByDtoInterfaceNamespace
        {
            get
            {
                return _modelsBackedByDtoInterfaceNamespace;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.EndsWith("."))
                {   // Strip off trailing period.  This can happen if the user did not set a namespace postfix in the blueprint.
                    _modelsBackedByDtoInterfaceNamespace = value.Substring(0, value.Length - 1);
                }
                else
                {
                    _modelsBackedByDtoInterfaceNamespace = value;
                }
            }
        }

        [TemplateVariable(Consts.OUT_ModelsBackedByDto_DEFAULTVALUE, hiddenIndicator: true, description: "The format of the filename for the generated file.")]
        public string ModelsBackedByDtoOutputFilePath { get; set; }

        [TemplateVariable(Consts.STG_webApiDataServiceInterfaceClassName_DEFAULTVALUE)]
        public string WebApiDataServiceInterfaceClassName { get; set; }

        [TemplateVariable(Consts.STG_webApiDataServiceInterfaceNamespace_DEFAULTVALUE)]
        public string WebApiDataServiceInterfaceNamespace { get; set; }

        //  [TemplateVariable(Consts.STG_sqliteModelObjectBaseAuditEditNamespace_DEFAULTVALUE)]
        //  public string SqliteModelObjectBaseAuditEditNamespace { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                foreach (var entity in ProcessModel.MetadataSourceModel.EntityTypes)
                {
                    string entityName = Inflector.Humanize(entity.ClrType.Name);

                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity, fileName: Consts.OUT_ModelsBackedByDto);
                    //outputfile = outputfile.Replace("[tablename]", table.NameHumanCaseSingular).Replace("[tablepluralname]", table.NamePlural);
                    outputfile = outputfile.Replace("[tablename]", entityName).Replace("[tablepluralname]", entityName);
                    string filepath = outputfile;

                    var generator = new ModelsBackedByDtoGenerator(inflector: Inflector);
                    string generatedCode = generator.GenerateModelsBackedByDto(
                        baseNamespace: BaseNamespace,
                        namespacePostfix: NamespacePostfix,
                        classNamespace: ClassNameSpace,
                        prependSchemaNameIndicator: PrependSchemaNameIndicator,
                        modelsBackedByDtoInterfaceNamespace: ModelsBackedByDtoInterfaceNamespace,
                        webApiDataServiceInterfaceNamespace: WebApiDataServiceInterfaceNamespace,
                        webApiDataServiceInterfaceClassName: WebApiDataServiceInterfaceClassName,
                        excludedNavigationProperties: ProcessModel.ExcludedNavigationProperties,
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

            //AddTemplateVariablesManagerErrorsToRetVal(ref retVal);
            AddTemplateVariablesManagerErrorsToRetVal(ref retVal, Enums.LogLevel.Error);
            return retVal;
        }
    }
}