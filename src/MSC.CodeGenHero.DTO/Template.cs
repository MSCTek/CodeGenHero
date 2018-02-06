using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.CodeGenHero.DTO
{
    public class Template
    {
        public Template()
        {
            Settings = new List<TemplateSetting>();
            Usings = new List<NamespaceItem>();
        }

        public string Name { get; set; }
        public List<TemplateSetting> Settings { get; set; }
        public List<NamespaceItem> Usings { get; set; }
        public string Version { get; set; }
    }
}