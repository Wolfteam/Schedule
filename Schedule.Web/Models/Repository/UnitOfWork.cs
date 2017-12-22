using System;
using System.Net.Http;
using Schedule.Web.Models.Repository;

namespace Schedule.Web.Models.Repositories
{
    public class UnitOfWork
    {
        private static HttpClient _httpclient;
        public DisponibilidadProfesorRepository DisponibilidadRepository { get; private set; }
        public ProfesorRepository ProfesorRepository { get; private set; }

        public UnitOfWork(HttpClient httpClient)
        {
            _httpclient = httpClient;
            DisponibilidadRepository = new DisponibilidadProfesorRepository(httpClient);
            ProfesorRepository = new ProfesorRepository(httpClient);
        }
    }
}
