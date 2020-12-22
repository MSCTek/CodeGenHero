using System;

namespace CodeGenHero.Template.Models
{
    [AttributeUsage(AttributeTargets.Assembly)]
    [Serializable]
    public class TemplateAssemblyAttribute : Attribute
    {
        public TemplateAssemblyAttribute(string name, string uniqueAssemblyIdGuid,
            string description = null, string author = null, string version = null, string requiredMetadataSource = null)
        {
            Name = name;
            Description = description;
            Author = author;
            RequiredMetadataSource = requiredMetadataSource;
            Version = version;

            if (Guid.TryParse(uniqueAssemblyIdGuid, out Guid id))
            {
                Id = id;
            }
            else
            {
                throw new ArgumentException($"The format or value of the uniqueTemplateIdGuid parameter is invalid for template {name}.  Invalid value: {uniqueAssemblyIdGuid}");
            }
        }

        public virtual string Author
        {
            get;
            protected set;
        }

        public virtual string Description
        {
            get;
            protected set;
        }

        public virtual Guid Id
        {
            get;
            protected set;
        }

        public virtual string Name
        {
            get;
            protected set;
        }

        public virtual string RequiredMetadataSource
        {
            get;
            protected set;
        }

        public virtual string Version
        {
            get;
            protected set;
        }
    }
}