using System;
using System.Collections.Generic;
using System.Text;
using Schedule.Entities;
using Schedule.DAO;

namespace Schedule.BLL
{
    public class TokenServices : ITokenService
    {
        private TokenDAO _tokenDAO { get; set; }
        const int _expiricyTime = 12000;

        public TokenServices()
        {
            _tokenDAO = new TokenDAO();
        }

        /// <summary>
        /// Elimina el token indicado de la base de datos
        /// </summary>
        /// <param name="tokenId">Token a eliminar</param>
        /// <returns>True en caso de exito</returns>
        public bool DeleteToken(string tokenId)
        {
            return _tokenDAO.DeleteToken(tokenId);
        }

        public bool DeleteTokenByUserId(int userId)
        {
            throw new NotImplementedException();
            //_unitOfWork.TokenRepository.Delete(x => x.UserId == userId);
            //_unitOfWork.Save();

            //var isNotDeleted = _unitOfWork.TokenRepository.GetMany(x => x.UserId == userId).Any();
            //return !isNotDeleted;
        }

        /// <summary>
        /// Genera un token al username indicado
        /// </summary>
        /// <param name="username">Username al cual se le generara el token</param>
        /// <returns>Objeto de tipo Token</returns>
        public Token GenerateToken(string username)
        {
            string token = Guid.NewGuid().ToString();

            Token tokenModel = new Token
            {
                AuthenticationToken = token,
                CreateDate = DateTime.Now,
                ExpiricyDate = DateTime.Now.AddSeconds(_expiricyTime),
                Username = username
            };
            _tokenDAO.SaveToken(tokenModel);

            return tokenModel;
        }

        /// <summary>
        /// Valida si un token existe o no en la base de datos
        /// y adiciona 20 min + en caso de existir
        /// </summary>
        /// <param name="tokenId">TokenId a validar</param>
        /// <returns>True en caso de existir</returns>
        public bool ValidateToken(string tokenId)
        {
            Token token = _tokenDAO.GetToken(tokenId);
            if (token != null && !(DateTime.Now > token.ExpiricyDate))
            {
                token.ExpiricyDate = token.ExpiricyDate.AddSeconds(_expiricyTime);
                _tokenDAO.UpdateToken(token);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Obtiene una lista de privilegios asociada a un token
        /// </summary>
        /// <param name="tokenID">Token</param>
        /// <returns>Lista de tipo Privilegios</returns>
        public List<Privilegios> GetAllPrivilegiosByToken(string tokenID)
        {
            return _tokenDAO.PrivilegiosByToken(tokenID);
        }
    }
}