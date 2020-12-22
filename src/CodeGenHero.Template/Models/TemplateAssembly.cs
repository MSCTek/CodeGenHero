using CodeGenHero.Core;
using CodeGenHero.Template.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Models
{
    public class TemplateAssembly : BaseMarshalByRefObject, ITemplateAssembly
    {
        public TemplateAssembly(TemplateAssemblyAttribute templateAssemblyAttribute, string importBundleIdentifier = null)
        {
            Templates = new List<ITemplate>();
            TemplateAssemblyAttribute = templateAssemblyAttribute;
            ImportBundleIdentifier = importBundleIdentifier;
        }

        public string Author
        {
            get
            {
                return TemplateAssemblyAttribute?.Author;
            }
        }

        public string Description
        {
            get
            {
                return TemplateAssemblyAttribute?.Description;
            }
        }

        public Guid Id
        {
            get
            {
                return TemplateAssemblyAttribute == null ? Guid.Empty : TemplateAssemblyAttribute.Id;
            }
        }

        public string ImportBundleIdentifier { get; set; }

        public string Name
        {
            get
            {
                return TemplateAssemblyAttribute?.Name;
            }
        }

        public string RequiredMetadataSource
        {
            get
            {
                return TemplateAssemblyAttribute?.RequiredMetadataSource;
            }
        }

        public TemplateAssemblyAttribute TemplateAssemblyAttribute
        {
            get;
            private set;
        }

        public IList<ITemplate> Templates
        {
            get;
            private set;
        }

        public string Version
        {
            get
            {
                return TemplateAssemblyAttribute?.Version;
            }
        }

        public void Dispose()
        {
        }
    }
}