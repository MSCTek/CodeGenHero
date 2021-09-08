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
        public const string OUT_BlazorAPIControllerFilePath_DEFAULTVALUE = "Controllers\\{NamespacePostfix}\\[tablename]Controller.cs";
    }
}