using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Schedule.API.Models.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Agrega una entidad de tipo <paramref name="id"/>  a la tabla TEntity
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        void Add(TEntity entity);

        /// <summary>
        /// Agrega varias entidades a la tabla TEntity
        /// </summary>
        /// <param name="entities">Entidades a agregar</param>
        void AddRange(IEnumerable<TEntity> entity);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtiene una entidad particular con el <paramref name="id"/> de la tabla TEntity
        /// </summary>
        /// <param name="id">Id de la entidad a obtener</param>
        /// <returns>Entidad de tipo TEntity</returns>
        TEntity Get(object id);

        /// <summary>
        /// Obtiene todas las entidades de la tabla TEntity mapeadas 
        /// a TDestinationDTO
        /// </summary>
        /// <returns>Devuelve un IEnumerable de tipo TDestinationDTO</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Elimina una entidad en particular en base al id indicado
        /// </summary>
        /// <param name="id">Id de la entidad a eliminar</param>
        void Remove(object id);

        /// <summary>
        /// Elimina una entidad en particular
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Elimina varias entidades
        /// </summary>
        /// <param name="entity">Entidades a eliminar</param>
        void RemoveRange(IEnumerable<TEntity> entity);

        /// <summary>
        /// Actualiza un entidad eliminandola primero debido a que se debe
        /// pudiese actualizar su id
        /// </summary>
        /// <param name="id">Id de la entidad vieja</param>
        /// <param name="entity">Entidad con nuevos parametros</param>
        void Update(object id, TEntity entity);

        /// <summary>
        /// Actualiza un entidad
        /// </summary>
        /// <param name="entity">Entidad a actualizar</param>
        void Update(TEntity entity);
    }
}
