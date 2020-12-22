using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using System.Linq;
using CodeGenHero.Template.Models;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Server
{
    public class AutomapperProfileControllerGenerator : BaseAPIFFGenerator
    {
        private const string EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION = " -- Excluded navigation property per configuration.";

        public AutomapperProfileControllerGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        [TemplateVariable(Consts.STG_dtoNamespace_DEFAULTVALUE)]
        public string DtoNamespace { get; set; }

        public string GenerateAutomapperProfile(string baseNamespace,
                                                string namespacePostfix,
                                                string dtoNamespace,
                                                string repositoryEntitiesNamespace,
                                                IList<IEntityType> entityTypes,
                                                IList<IEntityNavigation> excludedNavigationProperties
                                                )
        {
            var sb = new StringBuilder();

            sb.AppendLine($"using AutoMapper;");
            sb.AppendLine($"using xDTO = {dtoNamespace};");
            sb.AppendLine($"using xENT = {repositoryEntitiesNamespace};");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {baseNamespace}.Repository.Infrastructure");
            sb.AppendLine($"{{");

            sb.AppendLine($"\tpublic partial class {namespacePostfix}AutoMapperProfile : Profile");
            sb.AppendLine($"\t{{");

            sb.AppendLine($"\t\tpublic {namespacePostfix}AutoMapperProfile()");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine($"\t\t\tInitializeProfile();");
            sb.AppendLine($"\t\t\tInitializePartial();");
            sb.AppendLine($"\t\t}}");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tpartial void InitializePartial();");

            sb.AppendLine(string.Empty);
            sb.AppendLine($"\t\tprivate void InitializeProfile()");
            sb.AppendLine($"\t\t{{");

            foreach (var entity in entityTypes)
            {
                string tableName = Inflector.Pascalize(entity.ClrType.Name); // entity.GetNameHumanCaseSingular(prependSchemaNameIndicator);
                sb.AppendLine($"\t\t\tCreateMap<xDTO.{tableName}, xENT.{tableName}>()");

                //bool addtable = table.FKs.Count + table.

                if (entity.ForeignKeys.Any() || entity.Navigations.Any())
                {
                    List<string> listed = new List<string>(); //keep a list of these so we don't duplicate because the user table, for example, is referenced twice by many tables.

                    foreach (var navigation in entity.Navigations)
                    {
                        sb.Append($"\t\t\t\t");

                        string name = navigation.Name;
                        bool excludeCircularReferenceNavigationIndicator = IsEntityInExcludedReferenceNavigionationProperties(excludedNavigationProperties, name);
                        if (excludeCircularReferenceNavigationIndicator) // We want to negate the value check because we are saying AutoMapper should ignore the property below.
                        {   // Include the line, but comment it out.
                            sb.Append("// ");
                        }

                        sb.Append($".ForMember(d => d.{name}, opt => opt.Ignore()) // Reverse nav");

                        if (excludeCircularReferenceNavigationIndicator)
                        {
                            sb.Append(EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION);
                        }

                        sb.AppendLine(string.Empty);
                    }

                    foreach (var foreignKey in entity.ForeignKeys)
                    {
                        sb.Append($"\t\t\t\t");

                        string name = Inflector.Pascalize(foreignKey.DependentToPrincipal.ClrType.Name);//foreignKey.RefTableHumanCase;
                        bool excludeCircularReferenceNavigationIndicator = IsEntityInExcludedReferenceNavigionationProperties(excludedNavigationProperties, name);
                        if (excludeCircularReferenceNavigationIndicator) // We want to negate the value check because we are saying AutoMapper should ignore the property below.
                        {   // Include the line, but comment it out.
                            sb.Append("// ");
                        }

                        if (!string.IsNullOrEmpty(name))
                        {
                            sb.Append($".ForMember(d => d.{name}, opt => opt.Ignore())");
                        }

                        if (excludeCircularReferenceNavigationIndicator)
                        {
                            sb.Append($" // {EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION}");
                        }

                        sb.AppendLine(string.Empty);
                    }
                }

                sb.AppendLine($"\t\t\t.ReverseMap();");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine($"\t\t}}");
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }
    }
}