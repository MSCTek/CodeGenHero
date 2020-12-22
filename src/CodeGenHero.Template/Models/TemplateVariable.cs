using CodeGenHero.Core;

namespace CodeGenHero.Template.Models
{
    public class TemplateVariable : BaseMarshalByRefObject
    {
        public virtual string ConfigLevelType { get; set; }
        public virtual string DefaultValue { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsHidden { get; set; }
        public virtual string Name { get; set; }
        public virtual System.Type PropertyType { get; set; }
        public virtual string Value { get; set; }
    }
}