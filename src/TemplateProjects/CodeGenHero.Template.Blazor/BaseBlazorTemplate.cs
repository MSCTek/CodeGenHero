using Newtonsoft.Json;
using System;
using CodeGenHero.Core;
using CodeGenHero.Template.Models;
using CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.Blazor
{
    public abstract class BaseBlazorTemplate : BaseTemplate
    {
        protected string TokenReplacements(string tokenString, IEntityType entity)
        {

            string retVal = tokenString
                .Replace("[tablename]", Inflector.Humanize(entity.ClrType.Name))
                .Replace("[tablepluralname]", Inflector.Pluralize(entity.ClrType.Name));

            return retVal;
        }
    }
}