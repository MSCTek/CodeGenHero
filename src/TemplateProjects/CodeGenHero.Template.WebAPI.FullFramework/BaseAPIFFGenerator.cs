using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.WebAPI.FullFramework
{
    public abstract class BaseAPIFFGenerator : BaseGenerator
    {
        public BaseAPIFFGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }
    }
}