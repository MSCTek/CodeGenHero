using System.Collections.Generic;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.WebAPI.FullFramework.Generators.Server
{
    public class APIStatusControllerGenerator : BaseAPIFFGenerator
    {
        public APIStatusControllerGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string GenerateApiStatusController(List<NamespaceItem> usingNamespaceItems, string classnamePrefix, string classNamespace, string repositoryname)
        {
            string className = $"{classnamePrefix}APIStatusController";
            var sb = new IndentingStringBuilder();
            sb.Append(GenerateUsings(usingNamespaceItems));

            sb.AppendLine(string.Empty);
            sb.AppendLine($"namespace {classNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"public partial class {className} : {classnamePrefix}BaseApiController");
            sb.AppendLine("{");

            sb.AppendLine($"public {classnamePrefix}APIStatusController(ILogger<{className}> log, {repositoryname} repository)");
            sb.AppendLine("\t: base(log, repository)");
            sb.AppendLine("{");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("[HttpGet]");
            sb.AppendLine($"[VersionedRoute(template: \"APIStatus\", allowedVersion: 1, Name = \"{classnamePrefix}APIStatus\")]");
            sb.AppendLine("#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously");
            sb.AppendLine("public async Task<IHttpActionResult> Get()");
            sb.AppendLine("#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously");

            sb.AppendLine("{");
            sb.AppendLine("try");
            sb.AppendLine("{");
            sb.AppendLine("var version = this.GetType().Assembly.GetName().Version;");
            sb.AppendLine("return Ok(version);");
            sb.AppendLine("}");
            sb.AppendLine("catch (Exception ex)");
            sb.AppendLine("{");
            sb.AppendLine($"Log.LogError(eventId: (int)coreEnums.EventId.Exception_WebApi,");
            sb.AppendLine("\texception: ex,");
            sb.AppendLine("\tmessage: \"Unable to get status via Web API for RequestUri {{RequestUri}}\",");
            sb.AppendLine("\tRequest.RequestUri.ToString());");
            sb.AppendLine(string.Empty);
            sb.AppendLine("if (System.Diagnostics.Debugger.IsAttached)");
            sb.AppendLine("\tSystem.Diagnostics.Debugger.Break();");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return InternalServerError();");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}