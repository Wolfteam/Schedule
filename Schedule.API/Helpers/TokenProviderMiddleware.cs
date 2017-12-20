using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Schedule.Entities;

namespace Schedule.API.Helpers
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private UnitOfWork _db;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options
            )
        {
            _next = next;
            _options = options.Value;
        }

        public Task Invoke(HttpContext context, HorariosContext db)
        {
            _db = new UnitOfWork(db);
            // If the request path doesn't match, skip
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
               || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }
            return GenerateToken(context);
        }

        /// <summary>
        /// Genera un JWT para un usuario en particular
        /// </summary>
        /// <param name="context">HttpContext</param>
        private async Task GenerateToken(HttpContext context)
        {
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];

            var identity = await GetIdentity(username, password);
            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            DateTime now = DateTime.Now;
            var claims = GetUserRoles(username, now);

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new TokenDTO
            {
                AuthenticationToken = encodedJwt,
                CreateDate = now,
                ExpiricyDate = now.Add(_options.Expiration),
                Username = username
            };
            // var response = new
            // {
            //     authenticationToken = encodedJwt,
            //     expires_in = (int)_options.Expiration.TotalSeconds
            // };
            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        /// <summary>
        /// Obtiene un ClaimsIdentity correspondiente al usuario y password dados
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Devuelve ClaimsIdentity</returns>
        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var user = _db.UsuarioRepository
                        .Get(u => u.Username == username && u.Password == password)
                        .FirstOrDefault();
            if (user != null)
            {
                return Task.FromResult(new ClaimsIdentity(new System.Security.Principal.GenericIdentity(username, "Token"), new Claim[] { }));
            }
            // Credentials are invalid, or account doesn't exist
            return Task.FromResult<ClaimsIdentity>(null);
        }

        /// <summary>
        /// Obtiene un IEnumerable de Claim acorde a los permisos/roles del usuario
        /// </summary>
        /// <param name="username">Usuario a obtener roles/permisos</param>
        /// <param name="now">Fecha actual, usada para registrar un claim</param>
        /// <returns>Devuelve IEnumerable de Claim </returns>
        private IEnumerable<Claim> GetUserRoles(string username, DateTime now)
        {
            //Saco los privilegios del usuario
            var userClaims = _db.UsuarioRepository
                .Get(x => x.Username == username, includeProperties: "IdPrivilegioNavigation")
                .Select(u => u.IdPrivilegioNavigation.NombrePrivilegio);
            // Specifically add the jti (random nonce), iat (issued timestamp), 
            //and sub (subject/user) claims.          
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToString(), ClaimValueTypes.Integer64)
            };
            //Nota como puedes definir un role o claim y luego usarlo en el authorize
            //Tambien es interesante ver el payload del jwt
            foreach (var userClaim in userClaims)
            {
                //claims.Add(new Claim(userClaim, "True"));
                claims.Add(new Claim(ClaimTypes.Role, userClaim));
            }
            var usuario = _db.UsuarioRepository
                            .Get(x => x.Username == username, includeProperties:"CedulaNavigation")
                            .FirstOrDefault();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, usuario.Cedula.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, usuario.Username));
            claims.Add(new Claim(ClaimTypes.GivenName, usuario.CedulaNavigation.Nombre));
            claims.Add(new Claim(ClaimTypes.Surname, usuario.CedulaNavigation.Apellido));
            return claims;
        }
    }
}