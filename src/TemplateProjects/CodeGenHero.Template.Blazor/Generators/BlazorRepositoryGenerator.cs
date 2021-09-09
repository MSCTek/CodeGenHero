using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;

namespace CodeGenHero.Template.Blazor.Generators
{
    public class BlazorRepositoryGenerator : BaseBlazorGenerator
    {
        public BlazorRepositoryGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }
    }
}
