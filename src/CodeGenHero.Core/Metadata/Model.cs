using CodeGenHero.Core.Metadata.Interfaces;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeGenHero.Core.Metadata
{
    [Serializable]
    public class Model : MetadataBase, IModel
    {
        public IList<IEntityType> EntityTypes { get; set; } = new List<IEntityType>();

        public string ModelFullyQualifiedTypeName { get; set; }

        public string ModelTypeName { get; set; }

        public IEntityType FindEntityType([NotNull] string name)
        {
            return EntityTypes.FirstOrDefault(x => x.Name == name);
        }

        public IEntityType FindEntityType([NotNull] string name, [NotNull] string definingNavigationName, [NotNull] IEntityType definingEntityType)
        {
            return EntityTypes.FirstOrDefault(x => x.Name == name && x.DefiningNavigationName == definingNavigationName && x.DefiningEntityType == x.DefiningEntityType);
        }

        public IList<IEntityType> GetEntityTypes()
        {
            return EntityTypes;
        }

        public IList<IEntityType> GetEntityTypesByRegEx(string excludeRegExPattern, string includeRegExPattern)
        {
            IList<IEntityType> retVal = GetEntityTypesFilteredByRegEx(excludeRegExPattern);
            retVal.AddRange(GetEntityTypesMatchingRegEx(includeRegExPattern));

            // Make sure we return a distinct list in case something does not get excluded and then is specifically included.
            retVal = retVal.GroupBy(elem => elem.Name).Select(group => group.First()).ToList();
            return retVal;
        }

        public IList<IEntityType> GetEntityTypesFilteredByRegEx(string regExPattern)
        {
            List<IEntityType> retVal = new List<IEntityType>();

            if (EntityTypes == null)
            {
                return retVal;
            }
            else if (string.IsNullOrWhiteSpace(regExPattern))
            {
                return EntityTypes;
            }

            foreach (var entityType in EntityTypes)
            {
                var match = Regex.Match(entityType.ClrType.Name, regExPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                if (!match.Success)
                {
                    retVal.Add(entityType);
                }
            }

            return retVal;
        }

        public IList<IEntityType> GetEntityTypesMatchingRegEx(string regExPattern)
        {
            List<IEntityType> retVal = new List<IEntityType>();

            if (EntityTypes == null || string.IsNullOrWhiteSpace(regExPattern))
            {
                return retVal;
            }

            foreach (var entityType in EntityTypes)
            {
                var match = Regex.Match(entityType.ClrType.Name, regExPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                if (match.Success)
                {
                    retVal.Add(entityType);
                }
            }

            return retVal;
        }
    }
}