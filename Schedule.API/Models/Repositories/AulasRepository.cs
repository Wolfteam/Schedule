using System;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using Schedule.Entities;
using LinqKit;
using System.Linq.Expressions;

namespace Schedule.API.Models.Repositories
{
    public class AulasRepository : IRepository<Aulas, AulasDetailsDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Crea una nueva aula
        /// </summary>
        /// <param name="aula">Objeto de tipo Aula</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(Aulas aula)
        {
            try
            {
                _db.Aulas.Add(aula);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Borra un aula especifica
        /// </summary>
        /// <param name="id">Id del aula a eliminar</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int id)
        {
            try
            {
                Aulas aula = _db.Aulas.FirstOrDefault(x => x.IdAula == id);
                if (aula == null)
                    return false;

                _db.Aulas.Remove(aula);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool Delete()
        {
            try
            {
                _db.Aulas.RemoveRange(_db.Aulas.OrderBy(x => x.NombreAula));
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Obtiene todas las aulas
        /// </summary>
        /// <returns>IQueryable de aulas</returns>
        public IEnumerable<AulasDetailsDTO> Get()
        {
            return _db.Aulas.ProjectTo<AulasDetailsDTO>();
        }

        //Estos 2 metodos fueron usados para el procesamiento de lado servidor, el incoveniente yace en la parte del sort
        //debido a que quizas pueda implementarlo mejor
        public List<AulasDetailsDTO> GetTest(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = BuildDynamicWhereClause(searchBy);

            var result = _db.Aulas
                .AsExpandable()
                .ProjectTo<AulasDetailsDTO>()
                .Where(whereClause);

            switch (sortBy.ToLower())
            {
                case "nombreaula":
                    result = sortDir ? result.OrderBy(x => x.NombreAula) : result.OrderByDescending(x => x.NombreAula);
                    break;
                case "capacidad":
                    result = sortDir ? result.OrderBy(x => x.Capacidad) : result.OrderByDescending(x => x.Capacidad);
                    break;
                case "tipoaula.nombretipo":
                    result = sortDir ? result.OrderBy(x => x.TipoAula.NombreTipo) : result.OrderByDescending(x => x.TipoAula.NombreTipo);
                    break;
                default://IdAula
                    result = sortDir ? result.OrderBy(x => x.IdAula) : result.OrderByDescending(x => x.IdAula);
                    break;
            }
            result = result.Skip(skip).Take(take);

            filteredResultsCount = _db.Aulas.AsExpandable().ProjectTo<AulasDetailsDTO>().Where(whereClause).Count();
            totalResultsCount = _db.Aulas.Count();

            return result.ToList();
        }

        private Expression<Func<AulasDetailsDTO, bool>> BuildDynamicWhereClause(string searchValue)
        {
            // simple method to dynamically plugin a where clause
            var predicate = PredicateBuilder.New<AulasDetailsDTO>(true); // true -where(true) return all
            if (!String.IsNullOrWhiteSpace(searchValue))
            {
                List<string> searchTerms = searchValue.Split(' ').ToList().ConvertAll(x => x.ToLower());
                List<byte> search = new List<byte>();
                byte.TryParse(searchValue, out byte y);
                search.Add(y);
                predicate = predicate.Or(s => searchTerms.Any(srch => s.NombreAula.ToLower().Contains(srch)))
                                    .Or(s => search.Contains(s.Capacidad))
                                    .Or(s => searchTerms.Any(srch => s.TipoAula.NombreTipo.ToLower().Contains(srch)));
            }
            return predicate;
        }


        /// <summary>
        /// Obtiene un aula en particular
        /// </summary>
        /// <param name="id">Id del aula a buscar</param>
        /// <returns>Objeto tipo aula</returns>
        public AulasDetailsDTO Get(int id)
        {
            return _db.Aulas.ProjectTo<AulasDetailsDTO>().FirstOrDefault(aula => aula.IdAula == id);
        }

        /// <summary>
        /// Obtiene todas las aulas que pertenecen a un tipo en particular y tienen una
        /// capacidad mayor o igual a la suministrada
        /// </summary>
        /// <param name="idTipo">Tipo de aula</param>
        /// <param name="capacidad">Capacidad del aula</param>
        /// <returns>IEnumerable de AulasDTO</returns>
        public IEnumerable<AulasDTO> GetByTipoCapacidad(byte idTipo, byte capacidad)
        {
            return _db.Aulas.ProjectTo<AulasDTO>()
                .Where(au => au.IdTipo == idTipo && au.Capacidad >= capacidad)
                .OrderBy(au => au.Capacidad);
        }

        /// <summary>
        /// Actualiza una aula en especifico
        /// </summary>
        /// <param name="id">Id del aula</param>
        /// <param name="aulaUpdated">Objeto de tipo aula</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(int id, Aulas aulaUpdated)
        {
            try
            {
                var aula = _db.Aulas.FirstOrDefault(x => x.IdAula == id);
                if (aula == null)
                    return false;

                aula.Capacidad = aulaUpdated.Capacidad;
                aula.IdTipo = aulaUpdated.IdTipo;
                aula.NombreAula = aulaUpdated.NombreAula;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Actualiza una aula en especifico
        /// </summary>
        /// <param name="aula">Objeto aula a actualizar</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(Aulas aula)
        {
            try
            {
                _db.Entry(aula).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }
}
