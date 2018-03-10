﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeGenHero.BingoBuzz.Xam.Controls
{
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }
            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source, Assembly.GetExecutingAssembly());

            return imageSource;
        }
    }
}