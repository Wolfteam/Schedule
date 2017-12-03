using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.API.Models.Repositories
{
    public class ProfesorRepository : IRepository<Profesores, ProfesorDetailsDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Crea un nuevo profesor
        /// </summary>
        /// <param name="profesor">Objeto de tipo profesor</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(Profesores profesor)
        {
            try
            {
                _db.Profesores.Add(profesor);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Borra una profesor en especifico
        /// </summary>
        /// <param name="cedula">Id del profesor a eliminar</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int cedula)
        {
            try
            {
                var profesor = _db.Profesores.FirstOrDefault(x => x.Cedula == cedula);
                if (profesor != null)
                {
                    _db.Profesores.Remove(profesor);
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
        /// Obtiene una lista de profesores
        /// </summary>
        /// <returns>Lista de profesores</returns>
        public IEnumerable<ProfesorDetailsDTO> Get()
        {
            return _db.Profesores.ProjectTo<ProfesorDetailsDTO>();
        }

        /// <summary>
        /// Obtiene un profesor en particular
        /// </summary>
        /// <param name="cedula">Id del profesor a buscar</param>
        /// <returns>Objeto Profesor</returns>
        public ProfesorDetailsDTO Get(int cedula)
        {
            return _db.Profesores.ProjectTo<ProfesorDetailsDTO>().FirstOrDefault(x => x.Cedula == cedula);
        }

        /// <summary>
        /// Obtiene las horas a cumplir de un profesor en particular
        /// </summary>
        /// <param name="cedula">Cedula</param>
        /// <returns>Numero de horas que un profesor debe cumplir</returns>
        public byte GetHorasACumplir(int cedula)
        {
            return _db.Profesores.Include(x => x.PrioridadProfesor).FirstOrDefault(x => x.Cedula == cedula).PrioridadProfesor.HorasACumplir;
        }

        /// <summary>
        /// Actualiza un profesor en especifico
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <param name="profesor">Objeto profesor a actualizar</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(int cedula, Profesores profesor)
        {
            try
            {
                //esto se hace asi xq no puedo actualizar una columna Key
                Delete(cedula);
                _db.Profesores.Add(profesor);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool Update(Profesores objeto)
        {
            throw new NotImplementedException();
        }
    }
}
