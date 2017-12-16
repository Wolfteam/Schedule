using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace Schedule.API.Models.Repositories
{
    public class MateriasRepository : IRepository<Materias, MateriasDetailsDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Crea una nueva materia
        /// </summary>
        /// <param name="materia">Objeto de tipo materia</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(Materias materia)
        {
            try
            {
                _db.Materias.Add(materia);
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
        /// Borra una materia en especifico
        /// </summary>
        /// <param name="codigo">Id de la materia a eliminar</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int codigo)
        {
            try
            {
                var materia = _db.Materias.FirstOrDefault(x => x.Codigo == codigo);
                if (materia != null)
                {
                    _db.Materias.Remove(materia);
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
        /// Obtiene todas las materias
        /// </summary>
        /// <returns>Lista de materias</returns>
        public IEnumerable<MateriasDetailsDTO> Get()
        {
            return _db.Materias.ProjectTo<MateriasDetailsDTO>();
        }

        /// <summary>
        /// Obtiene una materia en particular
        /// </summary>
        /// <param name="codigo">Codigo de la materia a buscar</param>
        /// <returns>Objeto Materias</returns>
        public MateriasDetailsDTO Get(int codigo)
        {
            return _db.Materias.ProjectTo<MateriasDetailsDTO>().FirstOrDefault(x => x.Codigo == codigo);
        }

        /// <summary>
        /// Obtiene todas las materias para un semestre en particular
        /// </summary>
        /// <param name="idSemestre">Id del semestre</param>
        /// <returns>IEnumerable de MateriasDTO</returns>
        public IEnumerable<MateriasDTO> GetBySemestre(int idSemestre)
        {
            return _db.Materias.ProjectTo<MateriasDTO>().Where(m => m.IdSemestre == idSemestre);
        }

        /// <summary>
        /// Actualiza una materia en especifico
        /// </summary>
        /// <param name="codigo">Codigo de la materia a actualizar</param>
        /// <param name="materia">Objeto materia a actualizar</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(int codigo, Materias materia)
        {
            try
            {
                //esto se hace asi xq no puedo actualizar una columna Key
                Delete(codigo);
                _db.Materias.Add(materia);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool Update(Materias materia)
        {
            try
            {
                _db.Entry(materia).State = EntityState.Modified;
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
