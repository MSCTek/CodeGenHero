using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template
{
	public partial interface ITemplateSetting
	{
		bool HiddenIndicator { get; set; }
		string Name { get; set; }
		string Value { get; set; }
	}
}