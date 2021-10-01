using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain;
using Nop.Services.Security;
using Nop.Services.Themes;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Themes;
using Yamool.Nop.Plugin.ThemeSwitcha.Models;

namespace Yamool.Nop.Plugin.ThemeSwitcha.Components
{
    [ViewComponent(Name = "WidgetsThemeSwitchaList")]
    public class WidgetsThemeSwitchaListViewComponent : NopViewComponent
    {
        private readonly IPermissionService _permissionService;
        private readonly IThemeProvider _themeProvider;
        private readonly IThemeContext _themeContext;
        private readonly StoreInformationSettings _storeInformationSettings;

        public WidgetsThemeSwitchaListViewComponent(IThemeContext themeContext,
            IThemeProvider themeProvider,
            IPermissionService permissionService,
            StoreInformationSettings storeInformationSettings)
        {
            _themeProvider = themeProvider;
            _themeContext = themeContext;
            _storeInformationSettings = storeInformationSettings;
            _permissionService = permissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            // only admin user can see this button.
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel))
            {
                return Content(string.Empty);
            }

            var model = new ThemeSwitchaListModel()
            {
                Themes = new List<ThemeModel>(),
                DefaultTheme = _storeInformationSettings.DefaultStoreTheme
            };

            var themes = (await _themeProvider.GetThemesAsync()).Select(k => new ThemeModel()
            {
                FriendlyName = k.FriendlyName,
                SystemName = k.SystemName,
                PreviewImageUrl = k.PreviewImageUrl,
                IsDefault = (k.SystemName == _storeInformationSettings.DefaultStoreTheme)
            });
            model.Themes = themes;
            var themeName = await _themeContext.GetWorkingThemeNameAsync();
            model.ActiveTheme = themes.Where(k => k.SystemName == themeName).First();
            return View("~/Plugins/Yamool.Plugin.ThemeSwitcha/Views/WidgetsThemeSwitchaList.cshtml", model);
        }
    }
}