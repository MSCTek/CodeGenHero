using Newtonsoft.Json;
using System;
using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.Blazor
{
    public abstract class BaseBlazorTemplate : BaseTemplate
    {
        protected string FilepathTokenReplacements(string filepath, IEntityType entity)
        {

            string retVal = filepath
                .Replace("[tablename]", Inflector.Humanize(entity.ClrType.Name))
                .Replace("[tablepluralname]", Inflector.Pluralize(entity.ClrType.Name));

            return retVal;
        }
    }
}