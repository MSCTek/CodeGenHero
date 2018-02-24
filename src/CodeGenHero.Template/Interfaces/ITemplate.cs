using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template
{
	public partial interface ITemplate
	{
		string Name { get; set; }
		List<ITemplateSetting> Settings { get; set; }
		List<INamespaceItem> Usings { get; set; }
		string Version { get; set; }
	}
}