namespace CodeGenHero.Template.WebAPI.FullFramework
{
    public static class Consts
    {
        public const string OUT_apiBaseController = "apiBaseControllerOutputFilePath";
        public const string OUT_apiBaseController_DEFAULTVALUE = "Controllers\\Base\\{namespacePostfix}BaseApiController.cs";

        public const string OUT_apiBaseControllerAuthorized = "apiBaseControllerAuthorizedOutputFilePath";
        public const string OUT_apiBaseControllerAuthorized_DEFAULTVALUE = "Controllers\\Base\\{namespacePostfix}BaseApiControllerAuthorized.cs";
        public const string OUT_apiStatusController = "apiStatusControllerOutputFilePath";
        public const string OUT_apiStatusController_DEFAULTVALUE = "Controllers\\Base\\{namespacePostfix}APIStatusController.cs";
        public const string OUT_automapperProfile = "automapperProfileOutputFilePath";

        public const string OUT_automapperProfile_DEFAULTVALUE = "Repository\\Infrastructure\\{namespacePostfix}AutoMapperProfile.cs";

        public const string OUT_dto = "dtoOutputFilePath";

        public const string OUT_dto_DEFAULTVALUE = "DTO\\{namespacePostfix}\\[tablename].cs";

        public const string OUT_mapperDtoToSqliteModelDataAndMvvmLightModelObject = "mapperDtoToSqliteModelDataAndMvvmLightModelObjectOutputFilePath";
        public const string OUT_mapperDtoToSqliteModelDataAndMvvmLightModelObject_DEFAULTVALUE = "Xamarin\\ModelMapper\\ModelMapper{namespacePostfix}Dto.cs";
        public const string OUT_mapperSqliteModelDataToMvvmLightModelObject = "mapperSqliteModelDataToMvvmLightModelObjectOutputFilePath";

        public const string OUT_mapperSqliteModelDataToMvvmLightModelObject_DEFAULTVALUE = "Xamarin\\ModelMapper\\ModelMapper{namespacePostfix}.cs";

        public const string OUT_ModelsBackedByDto = "ModelsBackedByDtoOutputFilePath";

        public const string OUT_ModelsBackedByDto_DEFAULTVALUE = "[tablename].cs";

        //public const string OUT_ModelsBackedByDtoInterface = "ModelsBackedByDtoInterfaceOutputFilePath";
        //public const string OUT_ModelsBackedByDtoInterface_DEFAULTVALUE = "I[tablename].cs";
        public const string OUT_mvvmLightModelObject = "mvvmLightModelObjectOutputFilePath";

        public const string OUT_mvvmLightModelObject_DEFAULTVALUE = "Xamarin\\ModelObj\\{namespacePostfix}\\[tablename].cs";

        public const string OUT_repository = "repositoryOutputFilePath";

        public const string OUT_repository_DEFAULTVALUE = "Repository\\Repository\\{namespacePostfix}Repository.cs";

        public const string OUT_repositoryBase = "repositoryBaseOutputFilePath";

        public const string OUT_repositoryBase_DEFAULTVALUE = "Repository\\Repository\\{namespacePostfix}RepositoryBase.cs";

        public const string OUT_repositoryBasePartialMethods = "repositoryBasePartialMethodsOutputFilePath";

        public const string OUT_repositoryBasePartialMethods_DEFAULTVALUE = "Repository\\Repository\\Custom\\{namespacePostfix}RepositoryBaseCustom.cs";

        public const string OUT_repositoryInterface = "repositoryInterfaceOutputFilePath";

        public const string OUT_repositoryInterface_DEFAULTVALUE = "Repository\\Interface\\I{namespacePostfix}Repository.cs";

        public const string OUT_repositoryInterfaceCrud = "repositoryInterfaceCrudOutputFilePath";

        public const string OUT_repositoryInterfaceCrud_DEFAULTVALUE = "Repository\\Interface\\I{namespacePostfix}RepositoryCrud.cs";

        public const string OUT_sampleData = "SampleDataOutputFilePath";
        public const string OUT_sampleData_DEFAULTVALUE = "SampleData\\{namespacePostfix}\\[tablename].cs";
        public const string OUT_sqliteModelData = "sqliteModelDataOutputFilePath";

        public const string OUT_sqliteModelData_DEFAULTVALUE = "Xamarin\\ModelData\\{namespacePostfix}\\[tablename].cs";

        //public const string OUT_sqliteModelData = "sqliteModelDataOutputFilePath";
        //public const string OUT_sqliteModelData_DEFAULTVALUE = "Xamarin\\ModelData\\{namespacePostfix}\\[tablename].cs";
        public const string OUT_webApiController = "webApiControllerOutputFilePath";

        public const string OUT_webApiController_DEFAULTVALUE = "Controllers\\{namespacePostfix}\\[tablepluralname]Controller.cs";

        public const string OUT_webApiControllerPartialMethods = "webApiControllerPartialMethodsOutputFilePath";

        public const string OUT_webApiControllerPartialMethods_DEFAULTVALUE = "Controllers\\{namespacePostfix}\\Custom\\[tablepluralname]ControllerCustom.cs";

        public const string OUT_webApiDataService = "webApiDataServiceOutputFilePath";

        public const string OUT_webApiDataService_DEFAULTVALUE = "Client\\WebAPIDataService\\WebAPIDataService{namespacePostfix}.cs";

        //public const string OUT_webApiDataServiceBaseInterface = "webApiDataServiceBaseInterfaceOutputFilePath";
        //public const string OUT_webApiDataServiceBaseInterface_DEFAULTVALUE = "Client\\IWebApiDataservice\\IWebApiDataServiceBase.cs";
        public const string OUT_webApiDataServiceInterface = "webApiDataServiceInterfaceOutputFilePath";

        public const string OUT_webApiDataServiceInterface_DEFAULTVALUE = "Client\\IWebApiDataservice\\IWebApiDataService{namespacePostfix}.cs";
        public const string STG_allowUpsertDuringPut_DEFAULTVALUE = "true";

        //public const string STG_apiHelpersNamespace = "apiHelpersNamespace";
        public const string STG_apiHelpersNamespace_DEFAULTVALUE = "{baseNamespace}.API.Helpers";

        //public const string STG_automapperProfileNamespace = "automapperProfileNamespace";
        //public const string STG_automapperProfileNamespace_DEFAULTVALUE = "{baseNamespace}.Repository.Infrastructure";
        //public const string STG_baseApiControllerAuthorizedClassName = "baseApiControllerAuthorizedClassName";
        //public const string STG_baseApiControllerAuthorizedClassName_DEFAULTVALUE = "{namespacePostfix}BaseApiControllerAuthorized";
        //public const string STG_baseApiControllerAuthorizedNameSpace = "baseApiControllerAuthorizedNameSpace";
        //public const string STG_baseApiControllerAuthorizedNameSpace_DEFAULTVALUE = "{baseNamespace}.API.Controllers.{namespacePostfix}";
        //public const string STG_baseApiControllerClassName = "baseApiControllerClassName";
        public const string STG_baseApiControllerClassName_DEFAULTVALUE = "{namespacePostfix}BaseApiController";

        //public const string STG_baseApiControllerNameSpace = "baseApiControllerNameSpace";
        public const string STG_baseApiControllerNameSpace_DEFAULTVALUE = "{baseNamespace}.API.Controllers.{namespacePostfix}";

        //public const string STG_namespacePostfix = "namespacePostfix";
        //public const string STG_namespacePostfix_DEFAULTVALUE = "";
        //public const string STG_prependSchemaNameIndicator = "prependSchemaNameIndicator";
        //public const string STG_prependSchemaNameIndicator_DEFAULTVALUE = "true";
        //public const string STG_regexExclude = "regexExclude";
        //public const string STG_regexExclude_DEFAULTVALUE = "";
        //public const string STG_regexInclude = "regexInclude";
        //public const string STG_regexInclude_DEFAULTVALUE = "";
        //public const string STG_repositoryClassName = "repositoryClassName";
        //public const string STG_repositoryClassName_DEFAULTVALUE = "{namespacePostfix}Repository";
        //public const string STG_repositoryDbContextName = "DbContextName";
        public const string STG_DbContextName_DEFAULTVALUE = "{namespacePostFix}DataContext";

        //public const string STG_baseNamespace = "baseNamespace";
        //public const string STG_baseNamespace_DEFAULTVALUE = "CodeGenHero";
        //public const string STG_basePath = "basePath";
        //public const string STG_defaultCriteria = "defaultCriteria";
        public const string STG_defaultCriteria_DEFAULTVALUE = "DateTime?:minModifiedUtcDate:IsGreaterThan:ModifiedUtcDate,bool?:isDeleted:IsEqualTo:IsDeleted";

        //public const string STG_dtoIncludeRelatedObjects = "dtoIncludeRelatedObjects";
        public const string STG_dtoIncludeRelatedObjects_DEFAULTVALUE = "true";

        //public const string STG_dtoNamespace = "dtoNamespace";
        public const string STG_dtoNamespace_DEFAULTVALUE = "{baseNamespace}.DTO.{namespacePostfix}";

        //public const string STG_entNamespace = "entNamespace";
        //public const string STG_entNamespace_DEFAULTVALUE = "{baseNamespace}.Repository.Entities.{namespacePostfix}";
        //public const string STG_enumNamespace = "enumNamespace";
        public const string STG_enumNamespace_DEFAULTVALUE = "{baseNamespace}.Constants.Enums";

        //public const string STG_enumNamespacePrefix = "enumNamespacePrefix";
        //public const string STG_enumNamespacePrefix_DEFAULTVALUE = "appEnums";
        //public const string STG_factoryNamespace = "factoryNamespace";
        public const string STG_factoryNamespace_DEFAULTVALUE = "{baseNamespace}.Repository.Factory";

        //public const string STG_iLoggingServiceNamespace = "iLoggingServiceNamespace";
        public const string STG_iLoggingServiceNamespace_DEFAULTVALUE = "CodeGenHero.Logging";

        //public const string STG_iWebApiExecutionContextNamespace = "iWebApiExecutionContextNamespace";
        //public const string STG_iWebApiExecutionContextNamespace_DEFAULTVALUE = "CodeGenHero.DataService";
        //public const string STG_maxRequestPerPageOverrideByTableName = "maxRequestPerPageOverrideByTableName";
        //public const string STG_ModelsBackedByDtoInterfaceNamespace = "ModelsBackedByDtoNamespaceInterface";
        public const string STG_ModelsBackedByDtoInterfaceNamespace_DEFAULTVALUE = "{baseNamespace}.Model.{namespacePostfix}.Interface";

        //public const string STG_ModelsBackedByDtoNamespace = "ModelsBackedByDtoNamespace";
        public const string STG_ModelsBackedByDtoNamespace_DEFAULTVALUE = "{baseNamespace}.Model";

        //public const string STG_repositoryEntitiesNamespace = "repositoryEntitiesNamespace";
        public const string STG_repositoryEntitiesNamespace_DEFAULTVALUE = "{repositoryNamespace}.Entities.{namespacePostfix}";

        //public const string STG_repositoryInterfaceNamespace = "repositoryInterfaceNamespace";
        public const string STG_repositoryInterfaceNamespace_DEFAULTVALUE = "{baseNamespace}.Repository.Interface";

        //public const string STG_repositoryNamespace = "repositoryNamespace";
        public const string STG_repositoryNamespace_DEFAULTVALUE = "{baseNamespace}.Repository";

        public const string STG_SampleDataCount_DEFAULTVALUE = "3";

        public const string STG_SampleDataDigits_DEFAULTVALUE = "2";

        public const string STG_SampleDataNamespace = "SampleDataNamespace";

        public const string STG_SampleDataNamespace_DEFAULTVALUE = "{baseNamespace}.SampleData.{namespacePostfix}";

        //public const string STG_serializationHelperNamespace = "serializationHelperNamespace";
        public const string STG_serializationHelperNamespace_DEFAULTVALUE = "{baseNamespace}.Common.Helpers";

        //public const string STG_sqliteModelDataBaseAuditEditNamespace = "sqliteModelDataBaseAuditEditNamespace";
        public const string STG_sqliteModelDataBaseAuditEditNamespace_DEFAULTVALUE = "CodeGenHero.Xam.Sqlite";

        //public const string STG_sqliteModelDataNamespace = "sqliteModelDataNamespace";
        public const string STG_sqliteModelDataNamespace_DEFAULTVALUE = "{baseNamespace}.Xam.ModelData.{namespacePostfix}";

        //public const string STG_sqliteModelDataBaseAuditEditNamespace = "sqliteModelDataBaseAuditEditNamespace";
        //public const string STG_sqliteModelDataBaseAuditEditNamespace_DEFAULTVALUE = "CodeGenHero.Xam.Sqlite";
        //public const string STG_sqliteModelDataNamespace = "sqliteModelDataNamespace";
        //public const string STG_sqliteModelDataNamespace_DEFAULTVALUE = "{baseNamespace}.Xam.ModelData.{namespacePostfix}";
        //public const string STG_sqliteModelMapperClassName = "sqliteModelMapperClassName";
        public const string STG_sqliteModelMapperClassName_DEFAULTVALUE = "ModelMapper";

        //public const string STG_sqliteModelMapperClassNamespace = "sqliteModelMapperClassNamespace";
        public const string STG_sqliteModelMapperClassNamespace_DEFAULTVALUE = "{baseNamespace}.Xam";

        //public const string STG_sqliteModelObjectBaseAuditEditNamespace = "sqliteModelObjectBaseAuditEditNamespace";
        public const string STG_sqliteModelObjectBaseAuditEditNamespace_DEFAULTVALUE = "CodeGenHero.Xam.MvvmLight";

        //public const string STG_sqliteModelObjectNamespace = "sqliteModelObjectNamespace";
        public const string STG_sqliteModelObjectNamespace_DEFAULTVALUE = "{baseNamespace}.Xam.ModelObj.{namespacePostfix}";

        //public const string STG_sqliteModelObjectNamespace = "sqliteModelObjectNamespace";
        //public const string STG_sqliteModelObjectNamespace_DEFAULTVALUE = "{baseNamespace}.Xam.ModelObj.{namespacePostfix}";
        //public const string STG_useAuthorizedBaseController = "useAuthorizedBaseController";
        public const string STG_useAuthorizedBaseController_DEFAULTVALUE = "true";

        //public const string STG_usingList = "usinglist";
        //public const string STG_version = "version";
        //public const string STG_version_DEFAULTVALUE = "1.0";
        //public const string STG_vsSolutionRelativeMapPath = "vsSolutionRelativeMapPath";
        //public const string STG_webApiControllerNamespace = "webApiControllerNamespace";
        public const string STG_webApiControllerNamespace_DEFAULTVALUE = "{baseNamespace}.API.Controllers.{namespacePostfix}";

        //public const string STG_webApiDataServiceClassName = "webApiDataServiceClassName";
        //public const string STG_webApiDataServiceClassName_DEFAULTVALUE = "WebApiDataService{namespacePostfix}";
        //public const string STG_webApiDataServiceInterfaceClassName = "webApiDataServiceInterfaceClassName";
        public const string STG_webApiDataServiceInterfaceClassName_DEFAULTVALUE = "IWebApiDataService{namespacePostfix}";

        //public const string STG_webApiDataServiceInterfaceNamespace = "webApiDataServiceInterfaceNamespace";
        public const string STG_webApiDataServiceInterfaceNamespace_DEFAULTVALUE = "{baseNamespace}.API.Client.Interface";

        //public const string STG_webApiDataServiceNamespace = "webApiDataServiceNamespace";
        public const string STG_webApiDataServiceNamespace_DEFAULTVALUE = "{baseNamespace}.API.Client";

        public const string TEMPLATE_APIBaseController = "APIBaseController";

        public const string TEMPLATE_APIStatusController = "APIStatusController";
        public const string TEMPLATE_AutoMapperProfile = "AutoMapperProfile";

        //public const string TEMPLATE_BaseAPIControllerAuthorized = "BaseAPIControllerAuthorized";
        public const string TEMPLATE_DTO = "DTO";

        public const string TEMPLATE_MapperDtoToSqliteModelDataAndMvvmLightModelObject = "MapperDtoToSqliteModelDataAndMvvmLightModelObject";
        public const string TEMPLATE_MapperSqliteModelDataToMvvmLightModelObject = "MapperSqliteModelDataToMvvmLightModelObject";

        //public const string TEMPLATE_ModelsBackedByDtoInterfaceTemplate = "ModelsBackedByDtoInterface";
        public const string TEMPLATE_ModelsBackedByDtoTemplate = "ModelsBackedByDto";

        public const string TEMPLATE_MvvmLightModelObject = "MvvmLightModelObject";
        public const string TEMPLATE_Repository = "Repository";

        public const string TEMPLATE_RepositoryBase = "RepositoryBase";

        public const string TEMPLATE_RepositoryBasePartialMethods = "RepositoryBasePartialMethods";

        public const string TEMPLATE_RepositoryInterface = "RepositoryInterface";

        public const string TEMPLATE_RepositoryInterfaceCrud = "RepositoryInterfaceCrud";

        public const string TEMPLATE_SampleData = "SampleData";
        public const string TEMPLATE_SqliteModelData = "SqliteModelData";

        //public const string TEMPLATE_SqliteModelData = "SqliteModelData";
        public const string TEMPLATE_WebApiController = "WebApiController";

        public const string TEMPLATE_WebApiControllerPartialMethods = "WebApiControllerPartialMethods";
        public const string TEMPLATE_WebApiDataService = "WebApiDataService";

        //public const string TEMPLATE_WebApiDataServiceBaseInterface = "WebApiDataServiceBaseInterface";
        public const string TEMPLATE_WebApiDataServiceInterface = "WebApiDataServiceInterface";
    }
}