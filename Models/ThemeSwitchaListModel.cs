using System.Collections.Generic;

namespace Yamool.Nop.Plugin.ThemeSwitcha.Models
{
    public class ThemeSwitchaListModel
    {
        public IEnumerable<ThemeModel> Themes { get; set; }

        public ThemeModel ActiveTheme { get; set; }

        public string DefaultTheme { get; set; }
    }
}