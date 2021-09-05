using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DartMaster9000.Tools
{
    public static class FontHelper
    {
        public static FontFamily Carnivale = Fonts.GetFontFamilies(new Uri("pack://application:,,,/"), "./Fonts/").First(x => x.Source.Contains("#Carnivalee Freakshow"));
        public static FontFamily SherlockHolmes = Fonts.GetFontFamilies(new Uri("pack://application:,,,/"), "./Fonts/").First(x => x.Source.Contains("#Carnivalee Freakshow"));
        public static FontFamily VeganPersonal = Fonts.GetFontFamilies(new Uri("pack://application:,,,/"), "./Fonts/").First(x => x.Source.Contains("#Carnivalee Freakshow"));

        public static FontFamily GetFont(string fontName)
        {
            FontFamily myFont = Fonts.GetFontFamilies(new Uri("pack://application:,,,/"), "./Fonts/").First(x => x.Source.Contains($"#{fontName}"));
            return myFont;
        }

    }
}
