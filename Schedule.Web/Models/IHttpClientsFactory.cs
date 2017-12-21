using System.Collections.Generic;
using System.Net.Http;

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
}