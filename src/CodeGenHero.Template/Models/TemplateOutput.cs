using CodeGenHero.Core;
using CodeGenHero.Template.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Models
{
    public class TemplateOutput : BaseMarshalByRefObject
    {
        public List<TemplateError> Errors { get; set; } = new List<TemplateError>();

        public List<OutputFile> Files { get; set; } = new List<OutputFile>();

        public string LibraryName { get; set; } = string.Empty;

        public ITemplateIdentity TemplateIdentity { get; set; }
    }
}