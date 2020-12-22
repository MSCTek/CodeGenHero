using CodeGenHero.Core.Metadata.Interfaces;
using System.Linq;
using System.Text;
using CodeGenHero.Inflector;

namespace CodeGenHero.Template.CSLA.Generators
{
    internal class CSLAEditRazorGenerator : BaseCSLAGenerator
    {
        public CSLAEditRazorGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        internal string GenerateRazor(string baseNamespace, string namespacePostfix, IEntityType entity, string AuthorizeRoles)
        {
            var sb = new StringBuilder();
            string entityName = Inflector.Humanize(entity.ClrType.Name);
            var k = entity.FindPrimaryKey();
            var kctype = k.Properties[0].ClrType;
            var primaryKey = entity.FindPrimaryKey();
            var ksimpleType = ConvertToSimpleType(kctype.Name);

            // Header area directives

            #region HeaderDirectives

            sb.AppendLine($"@page \"/Edit{entityName}\"");
            sb.AppendLine($"@page \"/EditPerson/{{{primaryKey.Properties[0].Name}}}\"");
            sb.AppendLine(string.IsNullOrWhiteSpace(namespacePostfix) ? $"@using {baseNamespace}.Shared" : $"@using {baseNamespace}.Shared.{namespacePostfix}");

            sb.AppendLine($"@inject Csla.Blazor.ViewModel<{entityName}Edit> vm");
            sb.AppendLine($"@inject NavigationManager NavigationManager");
            if (AuthorizeRoles != null && AuthorizeRoles.Any())
            {
                sb.AppendLine($"@attribute [Authorize(Roles = \"{AuthorizeRoles}\")]");
            }

            #endregion HeaderDirectives

            // Begin page body

            #region PageBody

            sb.AppendLine($"<h1>Edit {entityName}</h1>");
            sb.AppendLine($"");
            sb.AppendLine($"<p>This component demonstrates editing a BusinessBase-derived object.</p>");
            sb.AppendLine($"");
            sb.AppendLine($"<p style=\"background-color:red; color: white\">@vm.ViewModelErrorText</p>");
            sb.AppendLine($"");

            // Loading
            sb.AppendLine($"@if (vm.Model == null)");
            sb.AppendLine($"{{");
            sb.AppendLine($"\t\t<p>Loading {entityName}...</p>");
            sb.AppendLine($"}}");

            sb.AppendLine($"else");
            // Beginning of loading else
            sb.AppendLine($"{{");

            // Link back to index
            sb.AppendLine($"\t<p>");
            sb.AppendLine($"\t<a href=\"list{Inflector.Pluralize(entityName)}\">List of {Inflector.Pluralize(entityName)}</a>");
            sb.AppendLine($"\t</p>");
            sb.AppendLine($"");

            // Edit Table
            sb.AppendLine($"\t<table class=\"table\">");
            sb.AppendLine($"\t<thead>");
            sb.AppendLine($"\t\t<tr>");
            sb.AppendLine($"\t\t\t<th></th>");
            sb.AppendLine($"\t\t\t<th></th>");
            sb.AppendLine($"\t\t</tr>");
            sb.AppendLine($"\t</thead>");
            sb.AppendLine($"\t<tbody>");

            // begin iterate through properties for table
            var entityProperties = entity.GetProperties();
            foreach (var property in entityProperties)
            {
                string ctype = GetCType(property);
                var simpleType = ConvertToSimpleType(ctype);
                string propertyName = property.Name;

                if (propertyName == k.Properties[0].Name)
                {
                    //key
                    sb.AppendLine($"\t\t<tr>");
                    sb.AppendLine($"\t\t<td>{k.Properties[0].Name}</td>");
                    sb.AppendLine($"\t\t<td>@vm.Model.{k.Properties[0].Name}</td>");
                    sb.AppendLine($"\t\t</tr>");
                    sb.AppendLine($"\t\t");
                }
                else
                {
                    //non key
                    sb.AppendLine($"\t@if(vm.GetPropertyInfo(nameof(vm.Model.{propertyName})).CanRead)");
                    sb.AppendLine($"\t{{");
                    sb.AppendLine($"\t\t<tr>");
                    sb.AppendLine("\t\t<td>@(vm.GetPropertyInfo(nameof(vm.Model.Name)).FriendlyName)</td>");
                    sb.AppendLine($"\t\t<td>");
                    sb.AppendLine($"\t\t<TextInput Property = \"@(vm.GetPropertyInfo(nameof(vm.Model.{propertyName})))\"/>");
                    sb.AppendLine($"\t\t</td>");
                    sb.AppendLine($"\t\t</tr>");
                    sb.AppendLine($"");
                    sb.AppendLine($"\t}}");
                }
            }

            sb.AppendLine($"");

            // Is new
            sb.AppendLine($"\t<tr>");
            sb.AppendLine($"\t\t<td>Is New</td>");
            sb.AppendLine($"\t\t<td>@vm.Model.IsNew</td>");
            sb.AppendLine($"\t</tr>");
            sb.AppendLine($"");

            // Is savable
            sb.AppendLine($"\t<tr>");
            sb.AppendLine($"\t\t<td>Is Savable</td>");
            sb.AppendLine($"\t\t<td>@vm.Model.IsSavable</td>");
            sb.AppendLine($"\t</tr>");
            sb.AppendLine($"");

            sb.AppendLine($"\t</tbody>");
            sb.AppendLine($"\t</table>");

            // Save buuton
            sb.AppendLine($"");
            sb.AppendLine($"\t<button @onclick=\"vm.SaveAsync\" disabled=\"@(!vm.Model.IsSavable)\">Save {entityName}</button>");

            // End of loading else
            sb.AppendLine($"}}");

            #endregion PageBody

            GenerateRazorCodeSection(sb, entity);
            return sb.ToString();
        }

        private void GenerateRazorCodeSection(StringBuilder sb, IEntityType entity)
        {
            var primaryKey = entity.FindPrimaryKey();
            var primaryKeyCType = primaryKey.Properties[0].ClrType;
            var primatyKeySimpleType = ConvertToSimpleType(primaryKeyCType.Name);

            // Beginning code section declaration
            sb.AppendLine($"@code {{");

            // Parameter
            sb.AppendLine("\t[Parameter]");

            // In the original razor page, the parameter was declared as a string even though the key type was int
            // May want to revisit this
            sb.AppendLine($"\tpublic string {primaryKey.Properties[0].Name} {{ get; set; }}");

            // OnInitialized
            sb.AppendLine($"\tprotected override void OnInitialized()");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tvm.Saved += () => NavigationManager.NavigateTo(\"list{ Inflector.Pluralize(entity.ClrType.Name) }\");");
            sb.AppendLine("\t\tvm.ModelChanging += (o, n) =>");
            sb.AppendLine($"\t\t{{");
            sb.AppendLine("\t\t\tif (o != null)");
            sb.AppendLine("\t\t\t\to.PropertyChanged -= async (s, e) => await InvokeAsync(() => StateHasChanged());");
            sb.AppendLine("\t\t\tif (n != null)");
            sb.AppendLine("\t\t\t\tn.PropertyChanged += async (s, e) => await InvokeAsync(() => StateHasChanged());");

            sb.AppendLine("");
            sb.AppendLine($"\t\t}};");

            sb.AppendLine($"\t}}");

            // OnParametersSetAsync
            sb.AppendLine($"\tprotected override async Task OnParametersSetAsync()");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tif (string.IsNullOrWhiteSpace({primaryKey.Properties[0].Name}))");
            sb.AppendLine("\t\t\tawait vm.RefreshAsync();");
            sb.AppendLine("\t\telse");
            sb.AppendLine($"\t\t\tawait vm.RefreshAsync(int.Parse({primaryKey.Properties[0].Name}));");
            sb.AppendLine("");

            sb.AppendLine($"\t}}");

            // End code section declaration
            sb.AppendLine($"}}");
        }
    }
}