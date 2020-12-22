using System.Collections.Generic;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Server
{
    public class APIBaseControllerGenerator : BaseAPIFFGenerator
    {
        public APIBaseControllerGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public List<NamespaceItem> BuildBaseApiControllerUsings(
            string repositoryNamespace, string repositoryInterfaceNamespace,
            bool autoInvalidateCacheOutput)
        {
            List<NamespaceItem> retVal = new List<NamespaceItem>();
            int i = 0;
            retVal.Insert(i++, new NamespaceItem() { Namespace = "Microsoft.Extensions.Logging" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System.Collections.Generic" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System.Globalization" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System.Linq" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System.Net" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System.Net.Http" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System.Web.Http" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System.Runtime.CompilerServices" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System.Threading" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System.Threading.Tasks" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "System.Web.Http.Routing" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = repositoryNamespace });
            retVal.Insert(i++, new NamespaceItem() { Namespace = repositoryInterfaceNamespace });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "CodeGenHero.DataService" });
            retVal.Insert(i++, new NamespaceItem() { Namespace = "CodeGenHero.WebApi" });
            if (autoInvalidateCacheOutput)
            {
                retVal.Insert(i++, new NamespaceItem() { Namespace = "WebApi.OutputCache.V2" });
            }

            return retVal;
        }

        public string GenerateBaseApiController(
                    List<NamespaceItem> usings,
            string namespacePostfix,
            string className,
            string classNamespace,
            string interfaceRepository,
            string repositoryEntitiesNamespace,
            string dbContextName,
            bool autoInvalidateCacheOutput,
            bool useAuthorizedBaseController,
            bool? useIdentityBasicAuthenticationAttribute = null)
        {
            var sb = new IndentingStringBuilder();
            sb.Append(GenerateUsings(usings));

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {classNamespace}");
            sb.AppendLine("{");

            if (useAuthorizedBaseController)
            {
                if (useIdentityBasicAuthenticationAttribute.HasValue && useIdentityBasicAuthenticationAttribute.Value == true)
                {
                    sb.AppendLine("[IdentityBasicAuthentication] // Enable authentication via an user name and password");
                }
                else
                {
                    sb.AppendLine("[Authorize] // Require some form of authentication");
                }
            }

            if (!autoInvalidateCacheOutput)
                sb.Append("\t// ");

            sb.AppendLine("[AutoInvalidateCacheOutput]");

            sb.AppendLine($"[RoutePrefix(\"api/{namespacePostfix}\")]");
            sb.AppendLine($"public abstract partial class {className} : ApiController");
            sb.AppendLine("{");
            sb.AppendLine($"private {interfaceRepository} _repository;");
            sb.AppendLine("private readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);");
            sb.AppendLine(string.Empty);
            //sb.AppendLine($"public {className}()");
            //sb.AppendLine("{");
            //sb.AppendLine("try");
            //sb.AppendLine("{");
            //sb.AppendLine($"_log = new Log4NetLoggingService();");
            //sb.AppendLine(string.Empty);
            //sb.AppendLine($"_repository = new {namespacePostfix}Repository(");
            //sb.AppendLine($"\tnew {repositoryEntitiesNamespace}.{dbContextName}());");
            //sb.AppendLine(string.Empty);
            //sb.AppendLine($"RunCustomLogicAfterCtor();");
            //sb.AppendLine("}");
            //sb.AppendLine("catch (Exception ex)");
            //sb.AppendLine("{");
            //sb.AppendLine("if (_log != null)");
            //sb.AppendLine("{");
            //sb.AppendLine("_log.Error(message: $\"Failure to initialize repository in controller constructor.\",");
            //sb.AppendLine($"\tlogMessageType: LogMessageType.Instance.Exception_WebApi, ex: ex);");
            //sb.AppendLine("}");
            //sb.AppendLine("else");
            //sb.AppendLine("{");
            //sb.AppendLine("throw;");
            //sb.AppendLine("}");
            //sb.AppendLine("}");
            //sb.AppendLine("}");
            //sb.AppendLine(string.Empty);
            sb.AppendLine($"public {className}(ILogger log, {interfaceRepository} repository)");
            sb.AppendLine("{");
            sb.AppendLine("_repository = repository;");
            sb.AppendLine("Log = log;");
            sb.AppendLine("RunCustomLogicAfterCtor();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            //sb.AppendLine($"public CodeGenHero.Logging.Enums.LogLevel CurrentLogLevel");
            //sb.AppendLine("{");
            //sb.AppendLine("get");
            //sb.AppendLine("{");
            //sb.AppendLine("return Log.CurrentLogLevel;");
            //sb.AppendLine("}");
            //sb.AppendLine(string.Empty);
            //sb.AppendLine("set");
            //sb.AppendLine("{");
            //sb.AppendLine("throw new NotImplementedException();");
            //sb.AppendLine("}");
            //sb.AppendLine("}");
            //sb.AppendLine(string.Empty);
            sb.AppendLine($"protected {interfaceRepository} Repo {{ get {{ return _repository; }} }}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("/// <summary>");
            sb.AppendLine("///");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("/// <remarks>");
            sb.AppendLine("/// Using ILoggingService here in place of Log4NetLoggingService.");
            sb.AppendLine("/// However, doing so causes the CallerMemberName and CallerFilePath attributes to return null.");
            sb.AppendLine("/// This is because those attributes are used by the compiler (not at runtime) to do their magic.");
            sb.AppendLine("/// Thus, by abstracting them in an interface the compiler cannot tell which methods are calling.");
            sb.AppendLine("/// </remarks>");
            sb.AppendLine("protected ILogger Log { get; set; }");

            sb.AppendLine(string.Empty);
            sb.AppendLine("protected PageData BuildPaginationHeader(UrlHelper urlHelper, string routeName, int page, int totalCount, int pageSize, string sort)");
            sb.AppendLine("{   // calculate data for metadata");
            sb.AppendLine("var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);");
            sb.AppendLine("var prevLink = page > 1 ? urlHelper.Link(routeName,");
            sb.AppendLine("\tnew");
            sb.AppendLine("\t{");
            sb.AppendLine("\tpage = page - 1,");
            sb.AppendLine("\tpageSize = pageSize,");
            sb.AppendLine("\tsort = sort");
            sb.AppendLine("\t}) : \"\";");
            sb.AppendLine("var nextLink = page < totalPages ? urlHelper.Link(routeName,");
            sb.AppendLine("\tnew");
            sb.AppendLine("\t{");
            sb.AppendLine("\tpage = page + 1,");
            sb.AppendLine("\tpageSize = pageSize,");
            sb.AppendLine("\tsort = sort");
            sb.AppendLine("\t}) : \"\";");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return new PageData(currentPage: page, isSuccessStatusCode: true, nextPageLink: nextLink, pageSize: pageSize, previousPageLink: prevLink, totalCount: totalCount, totalPages: totalPages);");
            sb.AppendLine("}");

            sb.AppendLine(string.Empty);
            sb.AppendLine("protected string GetClientIpAddress(HttpRequestMessage request = null)");
            sb.AppendLine("{");
            sb.AppendLine("request = request ?? base.Request;");
            sb.AppendLine("return request.GetClientIpAddress();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("protected List<string> GetListByDelimiter(string fields, char delimiter = ',')");
            sb.AppendLine("{");
            sb.AppendLine("List<string> retVal = new List<string>();");
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (!string.IsNullOrEmpty(fields))");
            sb.AppendLine("{");
            sb.AppendLine("retVal = fields.ToLower().Split(delimiter).ToList();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return retVal;");
            sb.AppendLine("}");

            sb.AppendLine(string.Empty);
            sb.AppendLine("protected string GetUrl(HttpRequestMessage request = null)");
            sb.AppendLine("{");
            sb.AppendLine("request = request ?? base.Request;");
            sb.AppendLine("return request?.RequestUri?.PathAndQuery;");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("protected bool OnActionExecuting(out HttpStatusCode httpStatusCode, out string message, [CallerMemberName] string methodName = null)");
            sb.AppendLine("{");
            sb.AppendLine("httpStatusCode = HttpStatusCode.OK;");
            sb.AppendLine("message = null;");
            sb.AppendLine("RunCustomLogicOnActionExecuting(ref httpStatusCode, ref message, methodName);");
            sb.AppendLine("return (httpStatusCode == HttpStatusCode.OK);");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("public TResult RunSync<TResult>(Func<Task<TResult>> func)");
            sb.AppendLine("{");
            sb.AppendLine("var cultureUi = CultureInfo.CurrentUICulture;");
            sb.AppendLine("var culture = CultureInfo.CurrentCulture;");
            sb.AppendLine("return _myTaskFactory.StartNew(() =>");
            sb.AppendLine("{");
            sb.AppendLine("Thread.CurrentThread.CurrentCulture = culture;");
            sb.AppendLine("Thread.CurrentThread.CurrentUICulture = cultureUi;");
            sb.AppendLine("return func();");
            sb.AppendLine("}).Unwrap().GetAwaiter().GetResult();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("public void RunSync(Func<Task> func)");
            sb.AppendLine("{");
            sb.AppendLine("var cultureUi = CultureInfo.CurrentUICulture;");
            sb.AppendLine("var culture = CultureInfo.CurrentCulture;");
            sb.AppendLine("_myTaskFactory.StartNew(() =>");
            sb.AppendLine("{");
            sb.AppendLine("Thread.CurrentThread.CurrentCulture = culture;");
            sb.AppendLine("Thread.CurrentThread.CurrentUICulture = cultureUi;");
            sb.AppendLine("return func();");
            sb.AppendLine("}).Unwrap().GetAwaiter().GetResult();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicAfterCtor();");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"partial void RunCustomLogicOnActionExecuting(ref HttpStatusCode httpStatusCode, ref string message, string methodName);");
            sb.AppendLine(string.Empty);
            //sb.AppendLine("#region ILoggingService");
            //sb.AppendLine(string.Empty);

            //sb.AppendLine("/*");
            //sb.AppendLine("* This interface is implemented as a facade to ensure the CallerMemberName, CallerFilePath, and CallerLineNumber attributes work.");
            //sb.AppendLine("*/");
            //sb.AppendLine(string.Empty);

            //sb.Append(GenerateLogMethod("Debug", false));
            //sb.Append(GenerateLogMethod("Error", false));
            //sb.Append(GenerateLogMethod("Fatal", false));
            //sb.Append(GenerateLogMethod("Info", true));
            //sb.Append(GenerateLogMethod("Warn", true));

            //sb.AppendLine("#endregion ILoggingService");
            sb.AppendLine("}");
            sb.Append("}");

            return sb.ToString();
        }

        public string GenerateLogMethod(string name, bool exnull)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            string defaultnull = exnull ? " = null" : "";
            sb.AppendLine($"public Guid {name}(string message, int logMessageType, Exception ex{defaultnull}, string userName = null, string clientIPAddress = null,");
            sb.AppendLine("\t[CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0,");
            sb.AppendLine("\tDecimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null)");
            sb.AppendLine("{");
            sb.AppendLine("url = url ?? GetUrl();");
            sb.AppendLine("clientIPAddress = clientIPAddress ?? GetClientIpAddress();");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"return Log.{name}(message: message, logMessageType: logMessageType, ex: ex, userName: userName,");
            sb.AppendLine("\tclientIPAddress: clientIPAddress ?? GetClientIpAddress(), methodName: methodName, sourceFile: sourceFile,");
            sb.AppendLine("\tlineNumber: lineNumber, executionTimeInMilliseconds: executionTimeInMilliseconds, httpResponseStatusCode: httpResponseStatusCode, url: url ?? GetUrl());");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }
    }
}