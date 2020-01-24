// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Linq;
using System.Collections.Generic;

namespace CodeGenHero.Core.Metadata
{
	public class MetadataBase : IAnnotatable
	{
		public IList<KeyValuePair<string, IAnnotation>> Annotations { get; set; } = new List<KeyValuePair<string, IAnnotation>>();

		//object IAnnotatable.this[string name] { get { return this.Annotations[name]; } }

		//public object this[string name] { get { return this.Annotations[name]; } }

		public IAnnotation FindAnnotation(string name)
		{
			IAnnotation retVal = null;

			name = name?.ToLowerInvariant();
			retVal = Annotations.FirstOrDefault(x => x.Key.ToLowerInvariant() == name).Value;
			//if (Annotations.ContainsKey(name))
			//{
			//	retVal = Annotations[name];
			//}

			return retVal;
		}

		public IEnumerable<IAnnotation> GetAnnotations()
		{
			var retVal = new List<IAnnotation>();

			foreach (var annotation in Annotations)
			{
				retVal.Add(annotation.Value);
			}

			return retVal;
		}
	}
}