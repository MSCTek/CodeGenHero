using System;

namespace CodeGenHero.Template.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [Serializable]
    public class TemplateAttribute : Attribute
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="uniqueTemplateIdGuid">A System.String that contains a GUID in one of the following formats ('d'
        ///     represents a hexadecimal digit whose case is ignored): 32 contiguous digits:
        ///     dddddddddddddddddddddddddddddddd -or- Groups of 8, 4, 4, 4, and 12 digits
        ///     with hyphens between the groups. The entire GUID can optionally be enclosed
        ///     in matching braces or parentheses: dddddddd-dddd-dddd-dddd-dddddddddddd -or-
        ///     {dddddddd-dddd-dddd-dddd-dddddddddddd} -or- (dddddddd-dddd-dddd-dddd-dddddddddddd)
        ///     -or- Groups of 8, 4, and 4 digits, and a subset of eight groups of 2 digits,
        ///     with each group prefixed by "0x" or "0X", and separated by commas. The entire
        ///     GUID, as well as the subset, is enclosed in matching braces: {0xdddddddd,
        ///     0xdddd, 0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}} All braces, commas,
        ///     and "0x" prefixes are required. All embedded spaces are ignored. All leading
        ///     zeroes in a group are ignored.  The digits shown in a group are the maximum
        ///     number of meaningful digits that can appear in that group. You can specify
        ///     from 1 to the number of digits shown for a group. The specified digits are
        ///     assumed to be the low order digits of the group.
        /// </param>
        /// <param name="description"></param>
        public TemplateAttribute(string name, string version,
            string uniqueTemplateIdGuid, string description)
        {
            Name = name;
            Version = version;
            Description = description;
            //Author = author;

            if (Guid.TryParse(uniqueTemplateIdGuid, out Guid id))
            {
                Id = id;
            }
            else
            {
                throw new ArgumentException($"The format or value of the uniqueTemplateIdGuid parameter is invalid for template {name}.  Invalid value: {uniqueTemplateIdGuid}");
            }
        }

        public string Description
        {
            get;
            private set;
        }

        public Guid Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Version
        {
            get;
            private set;
        }
    }
}