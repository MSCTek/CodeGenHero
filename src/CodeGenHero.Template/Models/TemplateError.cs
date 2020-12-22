using CodeGenHero.Core;
using CodeGenHero.Template.Models.Interfaces;
using System;

namespace CodeGenHero.Template.Models
{
    [Serializable]
    public class TemplateError : BaseMarshalByRefObject
    {
        public Enums.LogLevel ErrorLevel { get; set; }

        public int EventId { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public ITemplateIdentity TemplateIdentity { get; set; }
    }
}