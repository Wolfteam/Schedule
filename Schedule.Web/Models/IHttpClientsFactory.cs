using System.Collections.Generic;
using System.Net.Http;

namespace Schedule.Web.Models
{
    public interface IHttpClientsFactory
    {
        /// <summary>
        /// Obtiene todos los clientes http
        /// </summary>
        /// <returns>Dictionary de string:httpclient</returns>
        Dictionary<string, HttpClient> GetClients();

        /// <summary>
        /// Obtiene un httpclient en particular
        /// </summary>
        /// <param name="key">Nombre con el cual se creo en el diccionario</param>
        /// <returns>HttpClient</returns>
        HttpClient GetClient(string key);

        /// <summary>
        /// Actualiza el token de un cliente http en el diccionario
        /// </summary>
        /// <param name="key">Nombre con el cual se creo en el diccionario</param>
        /// <param name="token">Token</param>
        void UpdateClientToken(string key, string token);


        /// <summary>
        /// Actualiza el token de un cliente http indicado
        /// </summary>
        /// <param name="key">Nombre con el cual se creo en el diccionario</param>
        /// <param name="httpClient">HttpClient a usar</param>
        /// <param name="token">Token</param>
        void UpdateClientToken(HttpClient httpClient, string token);
    }
}