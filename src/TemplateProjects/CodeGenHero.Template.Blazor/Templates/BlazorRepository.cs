using CodeGenHero.Template.Blazor.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor.Templates
{
    [Template(name: "BlazorRepository", version: "1.0", uniqueTemplateIdGuid: "9E1967A4-B36C-4318-8282-F6066E43283C",
        description: "Creates standard repositories to be accessed via API.")]
    public class BlazorRepository : BaseBlazorTemplate
    {
    }
}
