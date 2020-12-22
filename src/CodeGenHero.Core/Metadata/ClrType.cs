using CodeGenHero.Core.Metadata.Interfaces;
using System;

namespace CodeGenHero.Core.Metadata
{
    [Serializable]
    public class ClrType
    {
        public string Assembly { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
    }
}