﻿using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.API.Models.Repositories
{
    public class TokenRepository : IRepository<Tokens, TokenDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();
        const int _expiricyTime = 12000;


        /// <summary>
        /// Guarda el token generado en la base de datos
        /// </summary>
        /// <param name="token">Objeto de tipo Token</param>
        public bool Create(Tokens token)
        {
            try
            {
                Delete(token.Username);
                _db.Tokens.Add(token);
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
        /// Elimina los token de una persona indicada
        /// </summary>
        /// <param name="cedula">Cedula de la persona a quien se eliminaran los tokens</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int cedula)
        {
            try
            {
                var t = _db.Tokens.Where(x => x.Admin.Cedula == cedula);
                if (t != null)
                {
                    _db.Tokens.RemoveRange(t);
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
        /// Elimina los tokens de un usuario en particular
        /// </summary>
        /// <param name="username">Usuario</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(string username)
        {
            try
            {
                var t = _db.Tokens.Where(x => x.Token == username);
                if (t != null)
                {
                    _db.Tokens.RemoveRange(t);
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
        /// Genera un token al username indicado
        /// </summary>
        /// <param name="username">Username al cual se le generara el token</param>
        /// <returns>Objeto de tipo Token</returns>
        public Tokens GenerateToken(string username)
        {
            string token = Guid.NewGuid().ToString();

            Tokens tokenModel = new Tokens
            {
                Token = token,
                FechaCreacion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddSeconds(_expiricyTime),
                Username = username
            };
            return tokenModel;
        }

        public IQueryable<TokenDTO> Get()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene un objeto Token
        /// </summary>
        /// <param name="token">Token a obtener</param>
        /// <returns>Objeto de la clase TokenDTO</returns>
        public TokenDTO Get(string token)
        {
            return _db.Tokens.ProjectTo<TokenDTO>().FirstOrDefault(x => x.AuthenticationToken == token);
        }

        public TokenDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene los privilegios de un token en particular
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>Lista de tipo Privilegios</returns>
        public Entities.Privilegios GetAllPrivilegiosByToken(string token)
        {
            return (Entities.Privilegios)_db.Tokens.Include(p => p.Admin).FirstOrDefault(x => x.Token == token).Admin.IdPrivilegio;
        }

        public bool Update(int id, Tokens objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Actualiza un token en la base de datos
        /// </summary>
        /// <param name="token">Objeto de tipo Tokens</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(Tokens token)
        {
            try
            {
                var t = _db.Tokens.FirstOrDefault(x => x.Token == token.Token);
                if (t != null)
                {
                    t.FechaExpiracion = token.FechaExpiracion;
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
        /// Valida si el token existe en la base de datos
        /// </summary>
        /// <param name="token">Token de autenticacion</param>
        /// <returns>True en caso de existir</returns>
        public bool Validate(string token)
        {
            var t = _db.Tokens.FirstOrDefault(x => x.Token == token);
            if (t != null) return true;
            return false;
        }
    }
}