using CodeGenHero.Core;
using CodeGenHero.Template.CSLA.Generators;
using CodeGenHero.Template.Models;
using System;

namespace CodeGenHero.Template.CSLA.Templates
{
    [Template(name: "CSLAEditRazor", version: "1.0",
       uniqueTemplateIdGuid: "{10E586C0-77D4-4FC4-9A51-A13595B798BD}",
       description: "A razor template for editing a CSLA business object.")]
    public class CSLAEditRazorTemplate : BaseCSLATemplate
    {
        private string _classNameSpace = null;

        public CSLAEditRazorTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.OUT_Blazor_Razor_CSLA_Edit_DEFAULT_Roles, false,
            "Comma separated list of roles to use in generated Razor markup Authorize attribute.")]
        public string AuthorizeRoles { get; set; }

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

        [TemplateVariable(Consts.OUT_Blazor_Razor_CSLA_Edit_DEFAULTVALUE, true)]
        public string CSLARazorEditOutputFilePath { get; set; }

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
                        fileName: Consts.OUT_Blazor_CSLA_Razor_Edit);
                    outputfile = outputfile.Replace("[entityname]", $"{entityName}");
                    string filepath = outputfile;

                    if (UseBootstrap)
                    {
                        var generator = new CSLAEditRazorGeneratorBootstrap(inflector: Inflector);
                        generatedCode = generator.GenerateRazor(baseNamespace: BaseNamespace,
                           namespacePostfix: NamespacePostfix,
                           entity: entity, AuthorizeRoles: AuthorizeRoles);
                    }
                    else
                    {
                        var generator = new CSLAEditRazorGenerator(inflector: Inflector);
                        generatedCode = generator.GenerateRazor(baseNamespace: BaseNamespace,
                           namespacePostfix: NamespacePostfix,
                           entity: entity, AuthorizeRoles: AuthorizeRoles);
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