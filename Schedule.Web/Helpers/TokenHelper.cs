using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Schedule.Entities;
using System;
using System.Text;

namespace Schedule.Web.Helpers
{
    public class TokenHelper
    {
        /// <summary>
        /// Obtiene los parametros de validacion del token, los cuales deben coincidir con
        /// los de la api
        /// </summary>
        /// <param name="secretKey">Secret key que debe coincidir con la api</param>
        /// <returns>Devuelve TokenValidationParameters</returns>
        public static TokenValidationParameters GetTokenValidationParameters(TokenSettings tokenSettings)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings.SecretKey));
            return new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = tokenSettings.Issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudiences = tokenSettings.Audiences,

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero,

                SaveSigninToken = true
            };
        }

        /// <summary>
        /// Obtiene las propiedades de autenticacion del token
        /// </summary>
        /// <param name="token">TokenDTO que contiene datos del token generado por la api</param>
        /// <returns>AuthenticationProperties</returns>
        public static AuthenticationProperties GetTokenAuthProperties(TokenDTO token)
        {
            AuthenticationProperties properties = new AuthenticationProperties
            {
                ExpiresUtc = token.ExpiricyDate,
                IsPersistent = true
            };
            //importante esto, con ello puedes usar await HttpContext.GetTokenAsync("access_token");
            properties.StoreTokens(new[]
            {
                new AuthenticationToken()
                {
                    Name = "access_token",
                    Value = token.AuthenticationToken
                }
            });
            return properties;
        }
    }
}