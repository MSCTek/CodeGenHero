using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.CSLA.Generators
{
    internal class CSLAListRazorGenerator : BaseCSLAGenerator
    {
        public CSLAListRazorGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        internal string GenerateRazor(string baseNamespace, string namespacePostfix, IEntityType entity)
        {
            var sb = new StringBuilder();
            string entityName = Inflector.Humanize(entity.ClrType.Name);
            var k = entity.FindPrimaryKey();
            var kctype = k.Properties[0].ClrType;
            var primaryKey = entity.FindPrimaryKey();
            var ksimpleType = ConvertToSimpleType(kctype.Name);

            // Header area directives

            #region HeaderDirectives

            sb.AppendLine($"@page \"/List{Inflector.Pluralize(entityName)}\"");

            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"@using {baseNamespace}.Shared" : $"@using {baseNamespace}.Shared.{namespacePostfix}");

            sb.AppendLine($"@inject Csla.Blazor.ViewModel<{entityName}List> vm");

            #endregion HeaderDirectives

            // Begin page body

            #region PageBody

            sb.AppendLine($"<h1>List of {entityName}</h1>");
            sb.AppendLine($"");

            sb.AppendLine($"\t<p style=\"background-color:red; color: white\">@vm.ViewModelErrorText</p>");
            sb.AppendLine($"");

            // Loading, with link to add entity
            sb.AppendLine($"@if (vm.Model == null)");
            sb.AppendLine($"{{");
            sb.AppendLine($"\t\t<p>Loading List ...</p>");
            sb.AppendLine($"\t\t<p><a href=\"edit{entityName}\">Add {entityName}</a></p>");
            sb.AppendLine($"}}");

            sb.AppendLine($"else");
            // Beginning of loading else
            sb.AppendLine($"{{");

            // Link to add entity
            sb.AppendLine($"\t<p><a href=\"edit{entityName}\">Add {entityName}</a></p>");
            sb.AppendLine($"");

            // Main Table
            sb.AppendLine($"\t<table class=\"table\">");
            sb.AppendLine($"\t<thead>");
            sb.AppendLine($"\t\t<tr>");
            sb.AppendLine($"\t\t\t<th>Name</th>");
            sb.AppendLine($"\t\t\t<th></th>");
            sb.AppendLine($"\t\t</tr>");
            sb.AppendLine($"\t</thead>");
            sb.AppendLine($"\t<tbody>");

            // begin generated razor foreach

            sb.AppendLine($"\t\t@foreach (var item in vm.Model)");
            sb.AppendLine($"\t\t{{");

            // begin generated rows and columns
            var entityProperties = entity.GetProperties();

            foreach (var property in entityProperties)
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;

                if (propertyName == k.Properties[0].Name)
                {
                    //key, save for the end column
                }
                else
                {
                    sb.AppendLine($"\t\t\t<tr>");
                    sb.AppendLine($"\t\t\t<td>@item.{propertyName}</td>");
                }
            }

            sb.AppendLine($"\t\t\t<td><a href=\"edit{entityName}/@item.{k.Properties[0].Name}\">Edit</a></td>");
            sb.AppendLine($"\t\t\t</tr>");

            // end generated razor foreach
            sb.AppendLine($"\t\t}}");

            sb.AppendLine($"");

            sb.AppendLine($"\t</tbody>");
            sb.AppendLine($"\t</table>");

            // End of loading else
            sb.AppendLine($"}}");

            #endregion PageBody

            GenerateRazorCodeSection(sb, entity);
            return sb.ToString();
        }

        private void GenerateRazorCodeSection(StringBuilder sb, IEntityType entity)
        {
            // Beginning code section declaration
            sb.AppendLine($"@code {{");

            // OnInitialized
            sb.AppendLine($"\tprotected override async Task OnInitializedAsync()");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\t await vm.RefreshAsync();");

            sb.AppendLine($"\t}}");

            // End code section declaration
            sb.AppendLine($"}}");
        }
    }
}