using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.Template.Blazor
{
    public static class Consts
    {
        // Old
        #region Old Templates
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

        // Template Variable Defaults
        public const string OutFilepath_DEFAULTDESCRIPTION = "Path and name of the outputted file.";

        public const string BlazorAPIControllerClassNamespace_DEFAULTVALUE = "{baseNamespace}.Api.Controllers";
        public const string BlazorAPIBaseControllerClassName_DEFAULTVALUE = "{NamespacePostfix}BaseApiController";

        public const string BlazorRepositoryClassNamespace_DEFAULTVALUE = "{BaseNamespace}.Api.Database";
        public const string BlazorRepositoryClassNamespace_DEFAULTDESCRIPTION = "The namespace of the Repository classes. Should be promoted to Global.";
        public const string BlazorRepositoryEntitiesNamespace_DEFAULTVALUE = "{BaseNamespace}.Entities";
        public const string BlazorRepositoryEntitiesNamespace_DEFAULTDESCRIPTION = "The namespace of the DBContext and Entity Model classes.";
        public const string BlazorRepositoryDbContextClassName_DEFAULTVALUE = "{BaseNamespace}DbContext";
        public const string BlazorRepositoryDbContextClassName_DEFAULTDESCRIPTION = "The name of the DbContext class for your Metadata.";
        public const string BlazorRepositoryChildIncludesByTableName_DEFAULTDESCRIPTION = "A list of NameValue items (JSON) to be used as .Includes for all of a repository class's DB Get methods. Child Include names should be comma-delimited.";

        // Output Variable Names
        
        public const string BlazorAPIController_Base_OutFileVariableName = "BlazorAPIControllerBaseOutputFilePath";

        public const string BlazorRepository_Implementation_OutFileVariableName = "BlazorRepositoryImplementationOutputFilePath";
        public const string BlazorRepository_Interface_OutFileVariableName = "BlazorRepositoryInterfaceOutputFilePath";
        public const string BlazorRepository_Base_OutFileVariableName = "BlazorRepositoryBaseOutputFilePath";

        // Output Paths/Filenames
        public const string OUT_BlazorAPIControllerFilePath_DEFAULTVALUE = "Controllers\\{NamespacePostfix}[tablename]Controller.cs";

        public const string OUT_BlazorRepositoryImplementationFilePath_DEFAULTVALUE = "Repository\\{NamespacePostfix}[tablename]Repository.cs";
        public const string OUT_BlazorRepositoryInterfaceFilePath_DEFAULTVALUE = "Repository\\I{NamespacePostfix}[tablename]Repository.cs";
        public const string OUT_BlazorRepositoryBaseFilePath_DEFAULTVALUE = "Repository\\{NamespacePostfix}BaseRepository.cs";
        #endregion

        // Global Recommended
        #region Namespaces

        public const string PTG_EntitiesNamespace_DEFAULT = "{BaseNamespace}.Entities.{NamespacePostFix}";
        public const string PTG_EntitiesNamespace_DESC = "The namespace of the Db Entities that this Repository will use. Should be promoted to Global.";

        public const string PTG_RepositoryNamespace_DEFAULT = "{BaseNamespace}.Repository.Repositories.{NamespacePostFix}";
        public const string PTG_RepositoryNamespace_DESC = "The namespace of the Repository classes this template will use. Should be promoted to Global.";

        public const string PTG_APIControllerNamespace_DEFAULT = "{BaseNamespace}.Api.Controllers.{NamespacePostFix}";
        public const string PTG_APIControllerNamespace_DESC = "The namespace of the API Controller classes this template will use. Should be promoted to Global.";

        public const string PTG_MappersNamespace_DEFAULT = "{BaseNamespace}.Repository.Mappers.{NamespacePostFix}";
        public const string PTG_MappersNamespace_DESC = "The namespace of Entity to DTO Mapper classes. Should be promoted to Global.";

        public const string PTG_DtoNamespace_DEFAULT = "{BaseNamespace}.DTO.{NamespacePostFix}";
        public const string PTG_DtoNamespace_DESC = "The namespace of the DTO classes this template will use. Should be promoted to Global.";

        #endregion

        #region Class Names
        public const string PTG_DbContextName_DEFAULT = "{NamespacePostFix}DataContext";
        public const string PTG_DbContextName_DESC = "The name of the DbContext class this Repository is expected to access. Should be promoted to Global.";

        public const string PTG_RepositoryCrudInterfaceName_DEFAULT = "I{namespacePostfix}RepositoryCrud";
        public const string PTG_RepositoryCrudInterfaceName_DESC = "The name of the Repository CRUD Interface class. Should be promoted to Global.";

        public const string PTG_RepositoryInterfaceClassName_DEFAULT = "I{namespacePostfix}Repository";
        public const string PTG_RepositoryInterfaceClassName_DESC = "The name of the Repository Interface class. Should be promoted to Global.";

        public const string PTG_RepositoryName_DEFAULT = "{namespacePostfix}Repository";
        public const string PTG_RepositoryName_DESC = "The name of the Repository class. Should be promoted to Global.";

        public const string PTG_AutoMapperName_DEFAULT = "{namespacePostfix}AutoMapperProfile";
        public const string PTG_AutoMapperName_DESC = "The name of the AutoMapper Profile class. Should be promoted to Global.";

        public const string PTG_GenericFactoryInterfaceName_DEFAULT = "";
        public const string PTG_GenericFactoryInterfaceName_DESC = "The name of the Repository CRUD Interface class. Should be promoted to Global.";

        public const string PTG_GenericFactoryName_DEFAULT = "";
        public const string PTG_GenericFactoryName_DESC = "The name of the Repository CRUD Interface class. Should be promoted to Global.";

        public const string PTG_BaseAPIControllerName_DEFAULT = "";
        public const string PTG_BaseAPIControllerName_DESC = "The name of the Repository CRUD Interface class. Should be promoted to Global.";

        public const string PTG_APIControllerName_DEFAULT = "";
        public const string PTG_APIControllerName_DESC = "The name of the Repository CRUD Interface class. Should be promoted to Global.";

        #endregion

        // CRUD Interface
        public const string RepositoryCrudInterfaceOutputFilepath_DEFAULT = "Blazor\\Repository\\I{NamespacePostfix}RepositoryCrud.cs";
        public const string OUT_RepositoryCrudInterfaceOutputFilepath_DEFAULT = "RepositoryCrudInterfaceOutputFilepath";

        // Repository Interface
        public const string RepositoryInterfaceOutputFilepath_DEFAULT = "Blazor\\Repository\\I{NamespacePostfix}Repository.cs";
        public const string OUT_RepositoryInterfaceOutputFilepath_DEFAULT = "RepositoryInterfaceOutputFilepath";

        // Repository
        public const string RepositoryOutputFilepath_DEFAULT = "Blazor\\Repository\\Base{NamespacePostfix}Repository.cs";
        public const string OUT_RepositoryOutputFilepath_DEFAULT = "RepositoryOutputFilepath";

        // AutoMapper
        public const string AutoMapperOutputFilepath_DEFAULT = "Blazor\\Repository\\{NamespacePostfix}AutoMapperProfile.cs";
        public const string OUT_AutoMapperOutputFilepath_DEFAULT = "AutoMapperOutputFilepath";

        // Generic Factory Interface
        public const string GenericFactoryInterfaceOutputFilepath_DEFAULT = "Blazor\\Repository\\I{NamespacePostfix}GenericFactory.cs";
        public const string OUT_GenericFactoryInterfaceOutputFilepath_DEFAULT = "GenericFactoryInterfaceOutputFilepath";

        // Generic Factory
        public const string GenericFactoryOutputFilepath_DEFAULT = "Blazor\\Repository\\{NamespacePostfix}GenericFactory.cs";
        public const string OUT_GenericFactoryOutputFilepath_DEFAULT = "GenericFactoryOutputFilepath";

        // Base API Controller
        public const string BaseAPIControllerOutputFilepath_DEFAULT = "Blazor\\API\\{NamespacePostfix}BaseApiController.cs";
        public const string OUT_BaseAPIControllerOutputFilepath_DEFAULT = "BaseAPIControllerOutputFilepath";

        // API Controller
        public const string APIControllerFilePath_DEFAULTVALUE = "Blazor\\Controllers\\[tablename]Controller.cs";
        public const string OUT_APIControllerFilePath_DEFAULTVALUE = "APIControllerOutputFilepath";
    }
}