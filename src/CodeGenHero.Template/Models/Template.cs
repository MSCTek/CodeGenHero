using System.Collections.Generic;

namespace CodeGenHero.Template
{
	public class Template : ITemplate
	{
		public Template()
		{
			Settings = new List<ITemplateSetting>();
			Usings = new List<INamespaceItem>();
		}

		public string Name { get; set; }
		public List<ITemplateSetting> Settings { get; set; }
		public List<INamespaceItem> Usings { get; set; }
		public string Version { get; set; }
	}
}