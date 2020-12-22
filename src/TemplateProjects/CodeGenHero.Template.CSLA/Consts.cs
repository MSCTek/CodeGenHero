namespace CodeGenHero.Template.CSLA
{
    public static class Consts
    {
        public const string BlazorDataAccessNamespace_DEFAULTVALUE = "{baseNamespace}.Model";

        public const string OUT_Blazor_CSLA_EditBusiness = "CSLAEditBusinessOutputFilePath";
        public const string OUT_Blazor_CSLA_Razor_Edit = "CSLARazorEditOutputFilePath";
        public const string OUT_Blazor_CSLA_Razor_List = "CSLARazorListOutputFilePath";
        public const string OUT_Blazor_CSLA_ReadOnly_Info = "CSLAReadOnlyInfoOutputFilePath";
        public const string OUT_Blazor_CSLA_ReadOnly_List = "CSLAReadOnlyListOutputFilePath";
        public const string OUT_Blazor_DataAccess = "DataAccessOutputFilePath";

        public const string OUT_Blazor_DataAccess_CSLA_ReadOnlyEditBusiness_DEFAULTVALUE = "DataAccess\\{namespacePostfix}\\[entityname]Edit.cs";
        public const string OUT_Blazor_DataAccess_CSLA_ReadOnlyInfo_DEFAULTVALUE = "DataAccess\\{namespacePostfix}\\[entityname]Info.cs";
        public const string OUT_Blazor_DataAccess_CSLA_ReadOnlyList_DEFAULTVALUE = "DataAccess\\{namespacePostfix}\\[entityname]List.cs";

        public const string OUT_Blazor_DataAccess_DEFAULTVALUE = "DataAccess\\{namespacePostfix}\\[entityname].cs";
        public const string OUT_Blazor_DataAccess_Interface = "DataAccessInterfaceOutputFilePath";
        public const string OUT_Blazor_DataAccess_Interface_DEFAULTVALUE = "DataAccess\\{namespacePostfix}\\I[entityname]Dal.cs";
        public const string OUT_Blazor_DataAccessInterface = "DataAccessInterfaceOutputFilePath";
        public const string OUT_Blazor_Razor_CSLA_Edit_DEFAULT_Roles = "Admin";
        public const string OUT_Blazor_Razor_CSLA_Edit_DEFAULTVALUE = "DataAccess\\{namespacePostfix}\\Edit[entityname].razor";
        public const string OUT_Blazor_Razor_CSLA_List_DEFAULTVALUE = "DataAccess\\{namespacePostfix}\\List[entityname].razor";
        public const string OUT_DataAccessEF = "DataAccessEFOutputFilePath";
        public const string OUT_dataAccessEF_DBContextName_DEFAULTVALUE = "dbContext";

        public const string OUT_dataAccessEF_DEFAULTVALUE = "dataAccessEF\\{namespacePostfix}\\[entityname]EFDal.cs";

        public const string OUT_DataAccessMock = "DataAccessMockOutputFilePath";
        public const string OUT_dataAccessMock_DBContextName_DEFAULTVALUE = "dbContext";
        public const string OUT_dataAccessMock_DEFAULTVALUE = "dataAccessEF\\{namespacePostfix}\\[tablename]EFDal.cs";
        public const string STG_dataAccessEF_IncludeRelatedObjects_DEFAULTVALUE = "true";

        //public const string STG_dtoNamespace = "dtoNamespace";
        public const string STG_dataAccessEF_Namespace_DEFAULTVALUE = "{baseNamespace}.DataAccessEF.{namespacePostfix}";

        public const string STG_dataAccessEFNamespace = "DataAccessEFNamespace";
        public const string STG_dataAccessIncludeRelatedObjects_DEFAULTVALUE = "true";
        public const string STG_dataAccessMock_IncludeRelatedObjects_DEFAULTVALUE = "true";
        public const string STG_dataAccessMock_Namespace_DEFAULTVALUE = "DataAccessMock";
        public const string STG_dataAccessMockNamespace = "DataAccessMockNamespace";
        public const string TEMPLATE_DATAACCESS_EF = "DataAccess_EF";

        public const string TEMPLATE_DATAACCESS_MOCK = "DataAccess_MOCK";
    }
}