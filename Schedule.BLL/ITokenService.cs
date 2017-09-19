using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schedule.BLL
{
    public interface ITokenService
    {
        #region Interface member methods.
        /// <summary>
        /// Genera un token al username indicado
        /// </summary>
        /// <param name="username">Username al cual se le generara el token</param>
        /// <returns>Objeto de tipo Token</returns>
        Token GenerateToken(string username);

        /// <summary>
        /// Valida si un token existe o no en la base de datos
        /// y adiciona 20 min + en caso de existir
        /// </summary>
        /// <param name="tokenId">TokenId a validar</param>
        /// <returns>True en caso de existir</returns>
        bool ValidateToken(string tokenId);

        /// <summary>
        /// Elimina el token indicado de la base de datos
        /// </summary>
        /// <param name="tokenId">Token a eliminar</param>
        /// <returns>True en caso de exito</returns>
        bool DeleteToken(string tokenId);

        bool DeleteTokenByUserId(int userId);

        /// <summary>
        /// Obtiene una lista de privilegios asociada a un token
        /// </summary>
        /// <param name="tokenID">Token</param>
        /// <returns>Lista de tipo Privilegios</returns>
        List<Privilegios> GetAllPrivilegiosByToken(string tokenID);
        #endregion
    }
}
