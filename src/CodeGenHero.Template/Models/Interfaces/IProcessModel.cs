using System.Collections.Generic;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.Models.Interfaces
{
    public interface IProcessModel
    {
        string BaseWritePath { get; set; }

        IList<string> Errors { get; set; }

        /// <summary>
        /// Navigation properties that the user wishes to exclude
        /// </summary>
        IList<IEntityNavigation> ExcludedNavigationProperties { get; set; }

        IModel MetadataSourceModel { get; set; }

        IMetadataSourceProperties MetadataSourceProperties { get; set; }

        string RunName { get; set; }

        string SchemaText { get; set; }

        ITemplateIdentity TemplateIdentity { get; set; }

        IDictionary<string, string> TemplateVariables { get; set; }
    }
}