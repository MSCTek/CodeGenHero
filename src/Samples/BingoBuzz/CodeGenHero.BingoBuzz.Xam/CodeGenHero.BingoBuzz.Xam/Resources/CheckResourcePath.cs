using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using CodeGenHero.BingoBuzz.Xam.Views;

namespace CodeGenHero.BingoBuzz.Xam.Resources
{
    public class CheckResourcePath
    {
        public void GetMyPath()
        {
            // ...
            // NOTE: use for debugging, not in released app code!
            var assembly = typeof(WelcomePage).GetTypeInfo().Assembly;

            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
        }
    }
}