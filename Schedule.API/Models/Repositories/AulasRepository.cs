using System;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using Schedule.Entities;

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
                if (aula != null)
                {
                    _db.Aulas.Remove(aula);
                    _db.SaveChanges();
                }
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
        public IQueryable<AulasDetailsDTO> Get()
        {
            return _db.Aulas.ProjectTo<AulasDetailsDTO>(); 
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
                if (aula != null)
                {
                    aula.Capacidad = aulaUpdated.Capacidad;
                    aula.IdTipo = aulaUpdated.IdTipo;
                    aula.NombreAula = aulaUpdated.NombreAula;
                    _db.SaveChanges();
                }
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
