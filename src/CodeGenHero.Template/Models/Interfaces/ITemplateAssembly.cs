using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Models.Interfaces
{
    public interface ITemplateAssembly
    {
        string Author { get; }

        string Description { get; }

        Guid Id { get; }

        string ImportBundleIdentifier { get; set; }

        string Name { get; }

        string RequiredMetadataSource { get; }

        TemplateAssemblyAttribute TemplateAssemblyAttribute { get; }

        IList<ITemplate> Templates { get; }

        string Version { get; }

        void Dispose();
    }
}