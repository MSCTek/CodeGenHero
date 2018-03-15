using CodeGenHero.BingoBuzz.Xam.Controls;
using CodeGenHero.BingoBuzz.Xam.iOS.CustomRenderers;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(IconLabel), typeof(IconLabelRenderer))]

namespace CodeGenHero.BingoBuzz.Xam.iOS.CustomRenderers
{
    public class IconLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
            {
                return;
            }
            try
            {
                if (Control.Text.Length == 0) return;

                var myFont = UIFont.FromName("fontawesome", Control.Font.PointSize);
                Control.Font = myFont;
                Element.FontFamily = myFont.FamilyName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }
    }
}