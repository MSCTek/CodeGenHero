using CodeGenHero.Core.Metadata.Interfaces;
using System;

namespace CodeGenHero.Core.Metadata
{
    [Serializable]
    public class TypeBase : ITypeBase
    {
        public ClrType ClrType { get; set; }

        //public IModel Model { get; set; }
        public string Name { get; set; }
    }
}