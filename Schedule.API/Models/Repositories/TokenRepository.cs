using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class TokenRepository : Repository<Tokens>, ITokenRepository
    {
        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }
        private const int _expiricyTime = 12000;

        public TokenRepository(DbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Guarda el token generado en la base de datos removiendo todos aquellos
        /// ya existentes para un usuario
        /// </summary>
        /// <param name="token">Objeto de tipo Token</param>
        public override void Add(Tokens token)
        {
            var tokens = base.Get(t => t.Username == token.Username);
            base.RemoveRange(tokens);
            base.Add(token);
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

        /// <summary>
        /// Obtiene un objeto Token
        /// </summary>
        /// <param name="token">Token a obtener</param>
        /// <returns>Objeto de la clase TokenDTO</returns>
        public TokenDTO Get(string token)
        {
            return HorariosContext.Tokens.ProjectTo<TokenDTO>()
                .FirstOrDefault(x => x.AuthenticationToken == token);
        }

        /// <summary>
        /// Obtiene los privilegios de un token en particular
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>Lista de tipo Privilegios</returns>
        public Entities.Privilegios GetAllPrivilegiosByToken(string token)
        {
            return (Entities.Privilegios)HorariosContext.Tokens.Include(p => p.Admin).FirstOrDefault(x => x.Token == token).Admin.IdPrivilegio;
        }

        /// <summary>
        /// Obtiene la informacion del profesor asociada a un token en particular
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>Objeto de tipo ProfesorDetailsDTO</returns>
        public ProfesorDetailsDTO GetProfesorInfoByToken(string token)
        {
            return HorariosContext.Tokens//source
                .Where(x => x.Token == token)
                .Join
                (
                    HorariosContext.Admin,//target
                    fk => fk.Username,//fk
                    pk => pk.Username,//pk
                    (fk, pk) => new { Token = fk, Admin = pk }//select result
                )
                .Join
                (
                    HorariosContext.Profesores,
                    fk => fk.Admin.Cedula,
                    pk => pk.Cedula,
                    (fk, pk) => new { Profesor = pk }
                )
                .Select(x => x.Profesor).ProjectTo<ProfesorDetailsDTO>().FirstOrDefault();
        }

        /// <summary>
        /// Elimina los token de una persona indicada
        /// </summary>
        /// <param name="cedula">Cedula de la persona a quien se eliminaran los tokens</param>
        /// <returns>True en caso de exito</returns>
        public void RemoveByCedula(int cedula)
        {
            var tokens = base.Get(x => x.Admin.Cedula == cedula);
            base.RemoveRange(tokens);
        }

        /// <summary>
        /// Elimina el token de un usuario
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>True en caso de exito</returns>
        public void RemoveByToken(string token)
        {
            var tokenToDelete = HorariosContext.Tokens.FirstOrDefault(t => t.Token == token);
            base.Remove(tokenToDelete);
        }

        /// <summary>
        /// Valida si el token existe en la base de datos
        /// </summary>
        /// <param name="token">Token de autenticacion</param>
        /// <returns>True en caso de existir</returns>
        public bool TokenExists(string token)
        {
            var t = HorariosContext.Tokens.FirstOrDefault(x => x.Token == token);
            if (t != null) return true;
            return false;
        }

        /// <summary>
        /// Actualiza un token en la base de datos
        /// </summary>
        /// <param name="token">Objeto de tipo Tokens</param>
        /// <returns>True en caso de exito</returns>
        public override void Update(Tokens token)
        {

            var t = HorariosContext.Tokens.FirstOrDefault(x => x.Token == token.Token);
            if (t != null)
            {
                t.FechaExpiracion = token.FechaExpiracion;
            }
        }
    }
}
