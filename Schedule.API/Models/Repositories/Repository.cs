using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Schedule.API.Models.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext _context;
        private DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Agrega una entidad de tipo <paramref name="entity"/>  a la tabla TEntity
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Agrega varias entidades a la tabla TEntity
        /// </summary>
        /// <param name="entities">IEnumerable de Entidades a agregar</param>
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        /// <summary>
        /// Indica si existe una entidad que concuerde con la expresion pasada
        /// </summary>
        /// <param name="predicate">Predicado de filtrado</param>
        /// <returns>True en caso de existir(=1), False en caso contrario </returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet
                .AsNoTracking()
                .Where(predicate)
                .Count() == 1;
        }

        /// <summary>
        /// Obtiene un IEnumerable de TEntity acorde al predicado
        /// </summary>
        /// <param name="predicate">Predicado de filtrado</param>
        /// <returns>IEnumerable de TEntity</returns>
        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        /// <summary>
        /// Obtiene una entidad particular con el <paramref name="id"/> de la tabla TEntity
        /// </summary>
        /// <param name="id">Id de la entidad a obtener(Debe ser PK)</param>
        /// <returns>Entidad de tipo TEntity</returns>
        public virtual TEntity Get(object id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Obtiene un IEnumerable de TEntity acorde al filtro,
        /// al orderBy y a las propiedades incluidas
        /// </summary>
        /// <param name="filter">Filtro a aplicar</param>
        /// <param name="orderBy">Ordenamiento a aplicar</param>
        /// <param name="includeProperties">Propiedades de navegacion a incluir separadas por coma</param>
        /// <returns>IEnumerable de TEntity</returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Obtiene todas las entidades de la tabla TEntity. No se incluyen
        /// propiedades mapeadas
        /// </summary>
        /// <returns>Devuelve un IEnumerable de tipo TEntity</returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        /// <summary>
        /// Elimina una entidad en particular en base al id indicado
        /// </summary>
        /// <param name="id">Id de la entidad a eliminar(Debe ser PK)</param>
        public virtual void Remove(object id)
        {
            var entity = Get(id);
            if (entity != null)
                Remove(entity);
        }

        /// <summary>
        /// Elimina una entidad en particular
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        public virtual void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Elimina varias entidades
        /// </summary>
        /// <param name="entity">IEnumerable de Entidades a eliminar</param>
        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Actualiza un entidad eliminandola primero debido a que se debe
        /// pudiese actualizar su id, lo malo de este metodo es que usa 
        /// SaveChanges
        /// </summary>
        /// <param name="id">Id de la entidad vieja</param>
        /// <param name="entity">Entidad con nuevos parametros</param>
        public virtual void Update(object id, TEntity entity)
        {
            //No se si este update es valido para todos, se usa cuando nitas modificar el id 
            //el cual es PK en la tabla
            var entityToDelete = Get(id);
            if (entityToDelete != null)
            {
                Remove(entityToDelete);
                _context.SaveChanges();
            }
            Add(entity);
        }

        /// <summary>
        /// Actualiza un entidad en particular
        /// </summary>
        /// <param name="entity">Entidad a actualizar</param>
        public virtual void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
