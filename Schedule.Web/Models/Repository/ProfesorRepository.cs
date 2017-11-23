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
    public class ProfesorRepository
    {
        private readonly HttpClient _httpClient = null;

        public ProfesorRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        /// <summary>
        /// Obtiene una lista con todos los profesores
        /// </summary>
        /// <returns>Lista de tipo ProfesorDetailsDTO</returns>
        public async Task<List<ProfesorDetailsDTO>> GetAll()
        {
            List<ProfesorDetailsDTO> profesores = new List<ProfesorDetailsDTO>();
            HttpResponseMessage response = await _httpClient.GetAsync("api/Profesor");
            
            if (response.IsSuccessStatusCode)
            {
                profesores = await response.Content.ReadAsAsync<List<ProfesorDetailsDTO>>();
            }
            return profesores.OrderBy(x => x.Nombre);
        }

        /// <summary>
        /// Obtiene un profesor en particular
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>Objeto de tipo ProfesorDetailsDTO</returns>
        public async Task<ProfesorDetailsDTO> Get(int cedula)
        {
            ProfesorDetailsDTO profesor = null;
            HttpResponseMessage response = await _httpClient.GetAsync("api/Profesores/" + cedula);
            
            if (response.IsSuccessStatusCode)
            {
                profesor = await response.Content.ReadAsAsync<ProfesorDetailsDTO>();
            }
            return profesor;
        }
    }
}