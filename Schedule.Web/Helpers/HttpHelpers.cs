using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Schedule.Web.Helpers
{
    public class HttpHelpers
    {
        /// <summary>
        /// Inicializa el cliente http con la urlBase que le paases
        /// </summary>
        /// <param name="httpClient">Cliente http</param>
        /// <param name="urlBaseAPI">Url base de la api</param>
        public static void InitializeHttpClient(HttpClient httpClient ,string urlBaseAPI)
        {
            httpClient.BaseAddress = new Uri(urlBaseAPI);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Obtiene los privilegios de un usario autenticado
        /// </summary>
        /// <param name="httpClient">Cliente http</param>
        /// <param name="token">Token del usuario autenticado</param>
        /// <returns>Lista de privilegios del usuario autenticado</returns>
        public static async Task<List<Privilegios>> GetAllPrivilegiosByToken(HttpClient httpClient, string token)
        {
            List<Privilegios> listaPrivilegios = null;
            if (String.IsNullOrEmpty(token)) return listaPrivilegios;

            HttpResponseMessage response = await httpClient.GetAsync(String.Format("api/Account/Privilegios/{0}", token));

            if (response.IsSuccessStatusCode)
            {
                listaPrivilegios = await response.Content.ReadAsAsync<List<Privilegios>>();
            }
            return listaPrivilegios;
        }
    }
}
