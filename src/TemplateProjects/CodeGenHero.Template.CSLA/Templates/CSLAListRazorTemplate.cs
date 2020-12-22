using CodeGenHero.Core;
using CodeGenHero.Template.CSLA.Generators;
using CodeGenHero.Template.Models;
using System;

namespace CodeGenHero.Template.CSLA.Templates
{
    [Template(name: "CSLAListRazor", version: "1.0",
       uniqueTemplateIdGuid: "{618D4797-7BB3-44B0-9585-AF9874C54635}",
       description: "A razor template for listing CSLA business objects.")]
    public class CSLAListRazorTemplate : BaseCSLATemplate
    {
        private string _classNameSpace = null;

        public CSLAListRazorTemplate()
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

        [TemplateVariable(Consts.OUT_Blazor_Razor_CSLA_List_DEFAULTVALUE, true)]
        public string CSLARazorListOutputFilePath { get; set; }

        [TemplateVariable("false", false, "Generate markup with bootstrap styling")]
        public bool UseBootstrap { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();
            try
            {
                foreach (var entity in ProcessModel.MetadataSourceModel.EntityTypes)
                {
                    string entityName = Inflector.Humanize(entity.ClrType.Name);
                    string generatedCode;
                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                        fileName: Consts.OUT_Blazor_CSLA_Razor_List);
                    outputfile = outputfile.Replace("[entityname]", $"{Inflector.Pluralize(entityName)}");
                    string filepath = outputfile;

                    var generator = new CSLAListRazorGenerator(inflector: Inflector);  //TODO:  Make a bootstrap version generator
                    if (UseBootstrap)
                    {
                        generatedCode = generator.GenerateRazor(baseNamespace: BaseNamespace,
                           namespacePostfix: NamespacePostfix,
                           entity: entity);
                    }
                    else
                    {
                        generatedCode = generator.GenerateRazor(baseNamespace: BaseNamespace,
                           namespacePostfix: NamespacePostfix,
                           entity: entity);
                    }

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