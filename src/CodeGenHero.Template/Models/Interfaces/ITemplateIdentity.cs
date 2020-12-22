using System;

namespace CodeGenHero.Template.Models.Interfaces
{
    public interface ITemplateIdentity
    {
        Guid TemplateId { get; set; }
        string TemplateName { get; set; }
        string TemplateVersion { get; set; }

        ITemplateIdentity Copy();
    }
}