using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;
using CodeGenHero.BingoBuzz.Xam.Controls;
using CodeGenHero.BingoBuzz.Xam.UWP.CustomRenderers;

[assembly: ExportRenderer(typeof(IconLabel), typeof(IconLabelRenderer))]

namespace CodeGenHero.BingoBuzz.Xam.UWP.CustomRenderers
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

                Element.FontFamily = (@"/Assets/fontawesome-webfont.ttf#fontawesome");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }
    }
}