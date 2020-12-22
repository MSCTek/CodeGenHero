using System;

namespace CodeGenHero.Template.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [Serializable]
    public class TemplateVariableAttribute : Attribute
    {
        public TemplateVariableAttribute(string defaultValue, bool hiddenIndicator = false, string description = null)
        {
            DefaultValue = defaultValue;
            Description = description;
            IsHidden = hiddenIndicator;
        }

        public string DefaultValue
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public bool IsHidden
        {
            get;
            private set;
        }
    }
}