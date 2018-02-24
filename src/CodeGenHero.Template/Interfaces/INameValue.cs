using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CodeGenHero.Template
{
	public partial interface INameValue<T> : INotifyPropertyChanged
	{
		string Name { get; set; }
		T Value { get; set; }
	}
}