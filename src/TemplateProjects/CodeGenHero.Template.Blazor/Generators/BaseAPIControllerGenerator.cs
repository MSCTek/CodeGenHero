using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;
using System.Linq;

namespace CodeGenHero.Template.Blazor.Generators
{
    class BaseAPIControllerGenerator : BaseBlazorGenerator
    {
        public BaseAPIControllerGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            bool autoInvalidateCacheOutput,
            string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\t{(autoInvalidateCacheOutput ? "" : "// ")}[AutoInvalidateCacheOutput]");
            sb.AppendLine("\t[AllowAnonymous]");
            sb.AppendLine("\t[ApiController]");
            sb.AppendLine($"\t{GenerateRouteParameter(namespacePostfix)}");
            sb.AppendLine($"\tpublic abstract partial class {className} : Controller");
            sb.AppendLine("{");

            sb.Append(GenerateVariablesAndProperties(namespacePostfix));
            sb.Append(GenerateConstructor(className, namespacePostfix));
            sb.Append(GenerateImplementations(namespacePostfix));
            sb.Append(GenerateVirtualMethodSignatures());

            sb.Append(GenerateFooter());
            return sb.ToString();
        }

        private string GenerateRouteParameter(string namespacePostfix)
        {
            if (string.IsNullOrEmpty(namespacePostfix))
            {
                return "[Route(\"api/[controller]\")]";
            }
            else
            {
                return $"[Route(\"api/{namespacePostfix}/[controller]\")]";
            }
        }

        private string GenerateVariablesAndProperties(string namespacePostfix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("private readonly ILogger _logger;");
            sb.AppendLine($"private readonly I{namespacePostfix}Repository _repository;");
            sb.AppendLine("protected IHttpContextAccessor HttpContextAccessor { get; private set; }");
            sb.AppendLine("protected LinkGenerator LinkGenerator { get; private set; }");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"protected I{namespacePostfix}Repository Repo {{ get {{ return _repository; }} }}");
            sb.AppendLine($"protected ILogger Log {{ get {{ return _logger; }} }}");
            sb.AppendLine("protected IServiceProvider ServiceProvider { get; set; }");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateConstructor(string className, string namespacePostfix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("public WABaseApiController()");
            sb.AppendLine("{");
            sb.AppendLine("\t_logger = NullLogger.Instance;");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"public {className}(ILogger logger,");
            sb.AppendLine("\tIServiceProvider serviceProvider,");
            sb.AppendLine("\tIHttpContextAccessor httpContextAccessor,");
            sb.AppendLine("\tLinkGenerator linkGenerator,");
            sb.AppendLine($"\tI{namespacePostfix}Repository repository)");
            sb.AppendLine("{");

            sb.AppendLine("\t_logger = logger ?? NullLogger.Instance;");
            sb.AppendLine("\tServiceProvider = serviceProvider;");
            sb.AppendLine("\tHttpContextAccessor = httpContextAccessor;");
            sb.AppendLine("\tLinkGenerator = linkGenerator;");
            sb.AppendLine("\t_repository = repository;");

            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GenerateImplementations(string namespacePostfix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            string buildPaginationHeader = @"
                protected PageData BuildPaginationHeader(string action, int page, int totalCount, int pageSize, string sort)
                {   // calculate data for metadata
                    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                    var prevLink = page > 1 ? LinkGenerator.GetUriByAction(
                        httpContext: HttpContextAccessor.HttpContext,
                        action: action,
                        values: new { page = page - 1, pageSize = pageSize, sort = sort }) : "";

                    var nextLink = page < totalPages ? LinkGenerator.GetUriByAction(
                        httpContext: HttpContextAccessor.HttpContext,
                        action: action,
                        values: new { page = page + 1, pageSize = pageSize, sort = sort }) : "";

                    return new PageData(currentPage: page, nextPageLink: nextLink, pageSize: pageSize, previousPageLink: prevLink, totalCount: totalCount, totalPages: totalPages);
                }";
            string getClientIPAddress = @"
                protected string GetClientIpAddress()
                {
                    return HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }";
            string getListByDelimiter = @"
                protected List<string> GetListByDelimiter(string fields, char delimiter = ',')
                {
                    List<string> retVal = new List<string>();

                    if (!string.IsNullOrEmpty(fields))
                    {
                        retVal = fields.ToLower().Split(delimiter).ToList();
                    }

                    return retVal;
                }";
            string getURL = @"
                protected string GetUrl(HttpRequest request = null)
                {
                    request = request ?? base.Request;
                    var path = request.Path;
                    var query = request.QueryString;
                    var pathAndQuery = path + query;

                    return pathAndQuery;
                }";
            string prepareExpectationFailedResponse = $@"
                protected IActionResult PrepareExpectationFailedResponse(Exception ex)
                {{
                    var args = new object[] {{ 
                        (int)StatusCodes.Status417ExpectationFailed,
                        HttpContext.Request.GetEncodedUrl() }};

                    Log.LogWarning(eventId: (int){namespacePostfix}Enums.EventId.Warn_WebApi,
                        exception: ex,
                        message: ""Web API action failed. {{httpResponseStatusCode}}:{{url}}"",
                        args: args);

                    var retVal = StatusCode(StatusCodes.Status417ExpectationFailed, ex);
                    return retVal;
                }}";
            string prepareInternalServerErrorResponse = $@"
                protected IActionResult PrepareInternalServerErrorResponse(Exception ex)
                {{
                    var args = new object[] {{
                        (int)StatusCodes.Status500InternalServerError,
                        HttpContext.Request.GetEncodedUrl() }};
                    Log.LogError(eventId: (int){namespacePostfix}Enums.EventId.Exception_WebApi,
                        exception: ex,
                        message: $""{{ex.Message}} {{httpResponseStatusCode}}:{{url}}"",
                        args: args);

                    var retVal = StatusCode(StatusCodes.Status500InternalServerError,
                        value: System.Diagnostics.Debugger.IsAttached ? ex : null);
                    return retVal;
                }}";
            string prepareNotFoundResponse = $@"
                protected IActionResult PrepareNotFoundResponse()
                {{
                    var args = new object[] {{
                        ""httpResponseStatusCode"", (int)StatusCodes.Status404NotFound ,
                        ""url"", HttpContext.Request.GetEncodedUrl() }};
                Log.LogWarning(eventId: (int){namespacePostfix}Enums.EventId.Warn_WebApi,
                    exception: null,
                    message: ""Unable to find requested object via Web API. {{httpResponseStatusCode}}:{{url}}"",
                    args: args);

                    return NotFound();
                }}";
            string OnActionExecuting = @"
                protected bool OnActionExecuting(out int httpStatusCode, out string message, [CallerMemberName] string methodName = null)
                {
                    httpStatusCode = (int)HttpStatusCode.OK;
                    message = null;
                    RunCustomLogicOnActionExecuting(ref httpStatusCode, ref message, methodName);
                    return (httpStatusCode == (int)HttpStatusCode.OK);
                }";

            sb.Append(buildPaginationHeader);
            sb.AppendLine(string.Empty);
            sb.Append(getClientIPAddress);
            sb.AppendLine(string.Empty);
            sb.Append(getListByDelimiter);
            sb.AppendLine(string.Empty);
            sb.Append(getURL);
            sb.AppendLine(string.Empty);
            sb.Append(prepareExpectationFailedResponse);
            sb.AppendLine(string.Empty);
            sb.Append(prepareInternalServerErrorResponse);
            sb.AppendLine(string.Empty);
            sb.Append(prepareNotFoundResponse);
            sb.AppendLine(string.Empty);
            sb.Append(OnActionExecuting);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateVirtualMethodSignatures()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("protected virtual void RunCustomLogicAfterCtor() { }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("protected virtual void RunCustomLogicOnActionExecuting(ref int httpStatusCode, ref string message, string methodName) { }");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }
    }
}
