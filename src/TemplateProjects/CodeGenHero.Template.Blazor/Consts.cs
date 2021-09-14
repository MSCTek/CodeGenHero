using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.Template.Blazor
{
    public static class Consts
    {
        public const string Entities_Namespace_DEFAULTVALUE = "{baseNamespace}.DataAccess.Entities.{namespacePostfix}";
        public const string Model_Namespace_DEFAULTVALUE = "{baseNamespace}.Model";
        public const string OUT_ToDataMapperFilePath_DEFAULTVALUE = "Blazor\\DataMappers\\ToDataMappers.cs";
        public const string OUT_ToModelMapperFilePath_DEFAULTVALUE = "Blazor\\ModelMappers\\ToModelMappers.cs";

        public const string TEMPLATE_ToDataMapper = "ToDataMapper";
        public const string TEMPLATE_ToModelMapper = "ToModelMapper";

        public const string ToDataMapper_OutFileVariableName = "ToDataMapperOutputFilePath";
        public const string ToDataMapperClassNamespace_DEFAULTVALUE = "{baseNamespace}.Service";
        public const string ToModelMapper_OutFileVariableName = "ToModelMapperOutputFilePath";
        public const string ToModelMapperClassNamespace_DEFAULTVALUE = "{baseNamespace}.Service";

        public const string BlazorAPIControllerClassNamespace_DEFAULTVALUE = "{baseNamespace}.Api.Controllers";
        public const string BlazorAPIController_OutFileVariableName = "BlazorAPIControllerOutputFilePath";
        public const string OUT_BlazorAPIControllerFilePath_DEFAULTVALUE = "Controllers\\{NamespacePostfix}[tablename]Controller.cs";

        public const string BlazorRepositoryClassNamespace_DEFAULTVALUE = "{BaseNamespace}.Api.Database";
        public const string BlazorRepositoryClassNamespace_DEFAULTDESCRIPTION = "The namespace of the Repository classes. Should be promoted to Global.";
        public const string BlazorRepositoryEntitiesNamespace_DEFAULTVALUE = "{BaseNamespace}.Entities";
        public const string BlazorRepositoryEntitiesNamespace_DEFAULTDESCRIPTION = "The namespace of the DBContext and Entity Model classes.";
        public const string BlazorRepositoryDbContextClassName_DEFAULTVALUE = "{BaseNamespace}DbContext";
        public const string BlazorRepositoryDbContextClassName_DEFAULTDESCRIPTION = "The name of the DbContext class for your Metadata.";
        public const string BlazorRepositoryChildIncludesByTableName_DEFAULTDESCRIPTION = "A list of NameValue items (JSON) to be used as .Includes for all of a repository class's DB Get methods. Child Include names should be comma-delimited.";
        public const string BlazorRepository_Implementation_OutFileVariableName = "BlazorRepositoryImplementationOutputFilePath";
        public const string BlazorRepository_Interface_OutFileVariableName = "BlazorRepositoryInterfaceOutputFilePath";
        public const string BlazorRepository_Base_OutFileVariableName = "BlazorRepositoryBaseOutputFilePath";
        public const string OUT_BlazorRepositoryImplementationFilePath_DEFAULTVALUE = "Repository\\{NamespacePostfix}[tablename]Repository.cs";
        public const string OUT_BlazorRepositoryInterfaceFilePath_DEFAULTVALUE = "Repository\\I{NamespacePostfix}[tablename]Repository.cs";
        public const string OUT_BlazorRepositoryBaseFilePath_DEFAULTVALUE = "Repository\\{NamespacePostfix}BaseRepository.cs";
    }
}