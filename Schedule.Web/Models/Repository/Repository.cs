using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Schedule.Web.Models.Repository
{
    public class Repository<TEntity, TEntityDetails> : IRepository<TEntity, TEntityDetails>
        where TEntity : class
        where TEntityDetails : class
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _urlEntityApi;
        protected readonly IHttpClientsFactory _httpClientsFactory;
        public string Token
        {
            set { _httpClientsFactory.UpdateClientToken(_httpClient, value); }
        }

        public Repository(IHttpClientsFactory httpClientsFactory, string urlEntityApi)
        {
            _httpClientsFactory = httpClientsFactory;
            _httpClient = _httpClientsFactory.GetClient("ScheduleAPI");
            _urlEntityApi = urlEntityApi;
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_urlEntityApi, entity);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entity)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_urlEntityApi, entity);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<TEntityDetails> GetAsync(object id)
        {
            TEntityDetails entity = null;
            HttpResponseMessage response = await _httpClient.GetAsync($"{_urlEntityApi}/{id}");
            if (response.IsSuccessStatusCode)
            {
                entity = await response.Content.ReadAsAsync<TEntityDetails>();
            }
            return entity;
        }

        public async Task<List<TEntityDetails>> GetAllAsync()
        {
            List<TEntityDetails> entities = new List<TEntityDetails>();
            HttpResponseMessage response = await _httpClient.GetAsync(_urlEntityApi);
            if (response.IsSuccessStatusCode)
                entities = await response.Content.ReadAsAsync<List<TEntityDetails>>();
            return entities;
        }

        public async Task<bool> RemoveAsync(object id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_urlEntityApi}/{id}");
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<bool> UpdateAsync(object id, TEntity entity)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_urlEntityApi}/{id}", entity);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
    }
}