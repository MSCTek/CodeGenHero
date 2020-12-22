using CodeGenHero.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.Blazor
{
    public abstract class BaseBlazorGenerator : BaseGenerator
    {
        public BaseBlazorGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }
    }
}