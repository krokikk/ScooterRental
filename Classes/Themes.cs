using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScooterRental.Classes
{
    internal class Themes
    {
        private static bool isDayTheme = true;

        public static void ChangeTheme()
        {
            string path;

            if (isDayTheme)
            {
                path = "Styles/StyleNight.xaml";
            }
            else
            {
                path = "Styles/StyleDay.xaml";
            }

            var uri = new Uri(path, UriKind.Relative);

            var resourceDict =
                Application.LoadComponent(uri) as ResourceDictionary;

            if (resourceDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Clear();

                Application.Current.Resources.MergedDictionaries.Add(resourceDict);

                isDayTheme = !isDayTheme;
            }
        }
    
    }
}
