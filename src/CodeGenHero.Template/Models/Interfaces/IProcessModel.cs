using CodeGenHero.Core.Metadata.Interfaces;
using System.Collections.Generic;

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

        /// <summary>
        /// Provides a list of entity navigations that should not be generated.
        /// Comprised of the explictly excluded navigations in the ExcludedNavigationProperties property
        /// combined with navigations that should be excluded based upon the
        /// provided <paramref name="excludeRegExPattern"/> and <paramref name="includeRegExPattern"/> parameters..
        /// </summary>
        /// <param name="excludeRegExPattern"></param>
        /// <param name="includeRegExPattern"></param>
        /// <returns></returns>
        IList<IEntityNavigation> GetAllExcludedEntityNavigations(string excludeRegExPattern, string includeRegExPattern);
    }
}