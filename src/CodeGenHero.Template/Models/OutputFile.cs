using CodeGenHero.Core;

namespace CodeGenHero.Template.Models
{
    public class OutputFile : BaseMarshalByRefObject
    {
        public string Content { get; set; }
        public string Name { get; set; }
    }
}