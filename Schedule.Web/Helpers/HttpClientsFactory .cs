using Microsoft.Extensions.Options;
using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;

public class HttpClientsFactory : IHttpClientsFactory
{
    public static Dictionary<string, HttpClient> HttpClients { get; set; }
    private readonly IOptions<AppSettings> _appSettings;
    private static HttpClient _httpClient;

    public HttpClientsFactory(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings;
        HttpClients = new Dictionary<string, HttpClient>();
        //TODO: Ver si se instancia a cada rato este httpclient
        if (_httpClient == null)
            _httpClient = new HttpClient();
        Initialize();
    }

    private void Initialize()
    {
        // Agregamos la api
        var apiServer = _appSettings.Value.URLBaseAPI;
        _httpClient.BaseAddress = new Uri(apiServer);
        HttpClients.Add("ScheduleAPI", _httpClient);
    }

    public Dictionary<string, HttpClient> GetClients()
    {
        return HttpClients;
    }

    public HttpClient GetClient(string key)
    {
        return GetClients()[key];
    }
}