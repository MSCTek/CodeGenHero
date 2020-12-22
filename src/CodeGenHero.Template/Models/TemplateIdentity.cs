using CodeGenHero.Template.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Models
{
    [Serializable]
    public class TemplateIdentity : EqualityComparer<TemplateIdentity>, IEquatable<TemplateIdentity>,
        IComparable<TemplateIdentity>, IComparable,
        ITemplateIdentity
    {
        public TemplateIdentity(string templateName, Guid templateId, string templateVersion)
        {
            TemplateName = templateName;
            TemplateId = templateId;
            TemplateVersion = templateVersion;
        }

        public TemplateIdentity()
        {
        }

        public Guid TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string TemplateVersion { get; set; }

        public ITemplateIdentity Copy()
        {
            return new TemplateIdentity(TemplateName?.ToString(),
                TemplateId, // Guid is a value-type struct
                TemplateVersion?.ToString());
        }

        public override string ToString()
        {
            return $"{TemplateId}:{TemplateVersion}";
        }

        #region IComparable for sorting

        public int CompareTo(TemplateIdentity other)
        {
            // Sort by template version if name is equal.
            if (string.Equals(this.TemplateName, other.TemplateName, StringComparison.InvariantCultureIgnoreCase))
            {
                return this.TemplateVersion.CompareTo(other.TemplateVersion);
            }

            // Default to name sort. [Low to High]
            return this.TemplateName.CompareTo(other.TemplateName);
        }

        public int CompareTo(object obj)
        {
            TemplateIdentity otherObject = obj as TemplateIdentity;
            return this.CompareTo(otherObject);
        }

        #endregion IComparable for sorting

        #region EqualityComparer

        /// <summary>
        ///    Determines if the <paramref name="left" /> instance is considered unequal to the <paramref name="right" /> object.
        /// </summary>
        /// <param name="left"> The instance on the left of the inequality operator. </param>
        /// <param name="right"> The instance on the right of the inequality operator. </param>
        /// <returns> True if the instances are considered unequal, otherwise false. </returns>
        public static bool operator !=(TemplateIdentity left, TemplateIdentity right)
        {
            return !(left == right);
        }

        /// <summary>
        ///    Determines if the <paramref name="left" /> instance is considered equal to the <paramref name="right" /> object.
        /// </summary>
        /// <param name="left"> The instance on the left of the equality operator. </param>
        /// <param name="right"> The instance on the right of the equality operator. </param>
        /// <returns> True if the instances are considered equal, otherwise false. </returns>
        public static bool operator ==(TemplateIdentity left, TemplateIdentity right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(TemplateIdentity)) return false;
            return Equals((TemplateIdentity)obj);
        }

        public bool Equals(TemplateIdentity other)
        {
            bool retVal;
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            retVal = other.TemplateId.Equals(TemplateId) &&
                string.Equals(other.TemplateVersion, TemplateVersion);

            return retVal;
        }

        public override bool Equals(TemplateIdentity x, TemplateIdentity y)
        {
            Console.WriteLine($"\tEquals called on {x.ToString()} ({x.GetHashCode()})");
            bool retVal;

            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            retVal = x.TemplateId.Equals(y.TemplateId) &&
                string.Equals(x.TemplateVersion, y.TemplateVersion);

            return retVal;
        }

        public override int GetHashCode(TemplateIdentity obj)
        {
            int retVal = ($"{obj.TemplateId}{obj.TemplateVersion}").GetHashCode();

            return retVal;
        }

        public override int GetHashCode()
        {
            int retVal = GetHashCode(this);

            return retVal;
        }

        #endregion EqualityComparer
    }
}