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

        public IList<IEntityNavigation> GetExcludedEntityNavigationsByRegEx(string excludeRegExPattern, string includeRegExPattern)
        {
            IList<IEntityNavigation> retVal = new List<IEntityNavigation>();
            IList<IEntityType> filteredEntityTypes = GetEntityTypesByRegEx(excludeRegExPattern: excludeRegExPattern, includeRegExPattern: includeRegExPattern);

            if (EntityTypes == null || string.IsNullOrWhiteSpace(excludeRegExPattern))
            {
                return retVal;
            }

            foreach (var entityType in EntityTypes)
            {
                if (filteredEntityTypes.Contains(entityType))
                {   // This entity is not specifically excluded, scan its Navigations property for links to entities that are excluded.
                    foreach (var entityTypeNavigation in entityType.Navigations)
                    {
                        string principalEntityTypeClrTypeName = entityTypeNavigation.ForeignKey.PrincipalEntityType.ClrType.Name;
                        var excludeMatch = Regex.Match(principalEntityTypeClrTypeName, excludeRegExPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                        var includeMatchSuccess = false;
                        if (!string.IsNullOrWhiteSpace(includeRegExPattern))
                        {
                            includeMatchSuccess = Regex.Match(principalEntityTypeClrTypeName, includeRegExPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant).Success;
                        }

                        if (excludeMatch.Success && !includeMatchSuccess)
                        {   // clrTypeName matches our exclude pattern and is not overridden by the include pattern.
                            retVal.Add(new EntityNavigation()
                            {
                                EntityType = entityType,
                                Navigation = entityTypeNavigation
                            });
                        }
                    }
                }
                else
                {   // This is one of our excluded entities, exclude all associated navigations it contains.
                    foreach (var entityTypeNavigation in entityType.Navigations)
                    {
                        retVal.Add(new EntityNavigation()
                        {
                            EntityType = entityType,
                            Navigation = entityTypeNavigation
                        });
                    }
                }
            }

            return retVal;
        }
    }
}