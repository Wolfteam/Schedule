using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Schedule.Entities;
using Schedule.Web.Models;
using Schedule.Web.Filters;
using Microsoft.Extensions.Options;
using Schedule.Web.ViewModels;
using Schedule.Web.Helpers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Schedule.Web.Models.Repository
{
    public class TokenRepository
    {
        private readonly HttpClient _httpClient = null;

        public TokenRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        /// <summary>
        /// Obtiene la informacion de un profesor en particular segun el token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>Objeto de tipo ProfesorDetailsDTO</returns>
        public async Task<ProfesorDetailsDTO> GetProfesorInfoByToken(string token)
        {
            ProfesorDetailsDTO profesor = null;
            HttpResponseMessage response = await _httpClient.GetAsync("api/Account/ProfesorInfo/"+ token);
            if (response.IsSuccessStatusCode)
            {
                profesor = await response.Content.ReadAsAsync<ProfesorDetailsDTO>();
            }
            return profesor;
        }
    }
}