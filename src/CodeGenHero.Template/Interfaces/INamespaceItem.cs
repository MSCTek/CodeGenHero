using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template
{
	public partial interface INamespaceItem
	{
		string Name { get; set; }
		string Namespace { get; set; }
		string NamespacePrefix { get; set; }
	}
}