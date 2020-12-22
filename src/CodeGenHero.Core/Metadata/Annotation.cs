using CodeGenHero.Core.Metadata.Interfaces;
using System;

namespace CodeGenHero.Core.Metadata
{
    [Serializable]
    public class Annotation : IAnnotation
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }
}