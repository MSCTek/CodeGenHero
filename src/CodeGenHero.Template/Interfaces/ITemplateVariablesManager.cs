using CodeGenHero.Template.Models.Interfaces;
using System.Collections.Generic;

namespace CodeGenHero.Template.Interfaces
{
    public interface ITemplateVariablesManager
    {
        List<string> Errors { get; set; }

        string GetOutputFile(ITemplateIdentity templateIdentity, string fileName);

        string GetValue(string name);

        List<string> GetValueList(string name);
    }
}