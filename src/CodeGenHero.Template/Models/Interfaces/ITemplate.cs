using CodeGenHero.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Models.Interfaces
{
    public interface ITemplate : IDisposable
    {
        string Description { get; }

        Guid Id { get; }

        // string LibraryName { get; set; }
        string Name { get; }

        //TemplateStatus Status { get; }

        List<TemplateVariable> TemplateVariables
        {   // Note: Leave this as a concrete TemplateVariable and not ITemplateVariable
            get;
        }

        string Version { get; }

        void AddError(ref TemplateOutput templateOutput, Exception ex, Enums.LogLevel logLevel);

        void AddTemplateVariablesManagerErrorsToRetVal(ref TemplateOutput retVal, Enums.LogLevel logLevel);

        T DeserializeJsonObject<T>(string json);

        TemplateOutput Generate();

        /// <summary>
        /// Appends navigations to the exclusion list when the navigation ClrType is not available as an allowedEntity.
        /// This may happen when excludeRegEx and includeRegEx patterns are used to filter out some entities that exist in metadata, but will not be generated.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="allowedEntities"></param>
        /// <param name="excludedNavigationProperties"></param>
        /// <returns>A superset of excluded navigation properties</returns>
        IList<IEntityNavigation> GetExcludedNavigationProperties(
            IEntityType entity,
            IList<IEntityType> allowedEntities,
            IList<IEntityNavigation> excludedNavigationProperties);

        List<TemplateError> Initialize(IProcessModel processModel);

        //List<TemplateError> Initialize(string processModelSerializedAsJson);

        /// <summary>
        ///
        /// </summary>
        /// <param name="templateVariableName"></param>
        /// <param name="value"></param>
        /// <returns>true if value was set successfully, false if variable was not found or value could not be set.</returns>
        bool SetTemplateVariable(string templateVariableName, object value);

        void Stop();
    }
}