using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Schedule.Web.Models.Repository
{
    public interface IRepository<TEntity, TEntityDetails>
        where TEntity : class
        where TEntityDetails : class
    {
        /// <summary>
        /// Crea una entidad mediante la api
        /// </summary>
        /// <param name="entity">TEntity a agregar</param>
        /// <returns>True en caso de exito</returns>
        Task<bool> AddAsync(TEntity entity);

        /// <summary>
        /// Crea varias entidades mediante la api
        /// </summary>
        /// <param name="entity">IEnumerable de TEntity a agregar</param>
        /// <returns>True en caso de exito</returns>
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entity);

        /// <summary>
        /// Obtiene todas las TEntityDetails de la api
        /// </summary>
        /// <returns>List de TEntityDetails</returns>
        Task<List<TEntityDetails>> GetAllAsync();

        /// <summary>
        /// Obtiene una entidad en particular de la api
        /// </summary>
        /// <param name="id">Id de la entidad</param>
        /// <returns>TEntityDetails</returns>
        Task<TEntityDetails> GetAsync(object id);

        /// <summary>
        /// Elimina una entidad en particular en la api
        /// </summary>
        /// <param name="id">Id de la entitdad</param>
        /// <returns>True en caso de exito</returns>
        Task<bool> RemoveAsync(object id);

        /// <summary>
        /// Actualiza una entidad en particular en la api
        /// </summary>
        /// <param name="id">Id de la entitdad</param>
        /// <param name="entity">TEntity a actualizar</param>
        /// <returns>True en caso de exito</returns>
        Task<bool> UpdateAsync(object id, TEntity entity);
    }
}
