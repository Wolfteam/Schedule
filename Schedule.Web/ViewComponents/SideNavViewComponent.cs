using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Helpers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Schedule.Web.ViewComponents
{
    /// <summary>
    /// Esta componente permite renderizar un menu del SideNav
    /// acorde a si es Administrador o no
    /// </summary>
    public class SideNavViewComponent : ViewComponent
    {
        private readonly IOptions<AppSettings> _appSettings;
        private static HttpClient _httpClient;
        public SideNavViewComponent(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string token = Request.Cookies["Token"];
            var privilegios = await GetAllPrivilegiosByToken(token);
            return View(privilegios);
        }

        private async Task<IEnumerable<Privilegios>> GetAllPrivilegiosByToken(string token)
        {
            _httpClient = new HttpClient();
            HttpHelpers.InitializeHttpClient(_httpClient, _appSettings.Value.URLBaseAPI, token);
            return await HttpHelpers.GetAllPrivilegiosByToken(_httpClient, token);
        }
    }
}
