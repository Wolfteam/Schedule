using AutoMapper.QueryableExtensions;
using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.API.Models.Repositories
{
    public class SeccionesRepository : IRepository<Secciones, SeccionesDetailsDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Crea una nueva seccion
        /// </summary>
        /// <param name="seccion">Objeto de tipo seccion</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(Secciones seccion)
        {
            try
            {
                _db.Secciones.Add(seccion);
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
        /// Borra todas las secciones
        /// </summary>
        /// <returns>True en caso de exito</returns>
        public bool Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Borra una seccion en especifico
        /// </summary>
        /// <param name="codigo">Codigo de la materia a la cual se le eliminaran las secciones</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int codigo)
        {
            try
            {
                var seccion = _db.Secciones.FirstOrDefault(x => x.Codigo == codigo);
                if (seccion != null)
                {
                    _db.Secciones.Remove(seccion);
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
        /// Obtiene una lista de todas las secciones
        /// </summary>
        /// <returns>Lista de secciones</returns>
        public IQueryable<SeccionesDetailsDTO> Get()
        {
            return _db.Secciones.ProjectTo<SeccionesDetailsDTO>();
        }

        /// <summary>
        /// Obtiene una seccion en particular
        /// </summary>
        /// <param name="codigo">Id de la materia a buscar</param>
        /// <returns>Objeto Secciones</returns>
        public SeccionesDetailsDTO Get(int codigo)
        {
            return _db.Secciones.ProjectTo<SeccionesDetailsDTO>().FirstOrDefault(x => x.Materia.Codigo == codigo);
        }

        /// <summary>
        /// Actualiza una seccion en especifico
        /// </summary>
        /// <param name="codigo">Codigo de la seccion a actualizar</param>
        /// <param name="seccion">Objeto seccion que contiene la nueva data</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(int codigo, Secciones seccion)
        {
            try
            {
                //esto se hace asi xq no puedo actualizar una columna Key
                Delete(codigo);
                _db.Secciones.Add(seccion);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool Update(Secciones objeto)
        {
            throw new NotImplementedException();
        }
    }
}
