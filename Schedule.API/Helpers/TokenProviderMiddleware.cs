using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Schedule.API.Helpers
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private IOptions<AppSettings> _appSettings;
        private IUnitOfWork _db;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public Task Invoke(HttpContext context, IUnitOfWork uow, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            _db = uow;
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
            return GenerateTokenAsync(context);
        }

        /// <summary>
        /// Genera un JWT para un usuario en particular
        /// </summary>
        /// <param name="context">HttpContext</param>
        private async Task GenerateTokenAsync(HttpContext context)
        {
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];
            bool rememberMe = context.Request.Form["rememberme"] == "true" ? true : false;
            bool isMobile = context.Request.Form["ismobile"] == "true" ? true : false;
            
            if (!DateTime.TryParse(context.Request.Form["currentDate"], out DateTime now))
                now = DateTime.Now;

            if (!DateTime.TryParse(context.Request.Form["currentDate"], out DateTime now))
                now = DateTime.Now;

            var identity = await GetIdentityAsync(username, password);
            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            var claims = GetUserRoles(username, now);
            TimeSpan expiricyTime;
            string audience = isMobile ? _options.Audience[1] : _options.Audience[0];
            if (rememberMe)
                expiricyTime = TimeSpan.FromDays(365);
            else if (isMobile)
                expiricyTime = TimeSpan.FromDays(1095);
            else
                expiricyTime = TimeSpan.FromHours(_appSettings.Value.TokenSettings.ExpiricyTime);

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                   issuer: _options.Issuer,
                   audience: audience,
                   claims: claims,
                   notBefore: now,
                   expires: now.Add(expiricyTime),
                   signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new TokenDTO
            {
                AuthenticationToken = encodedJwt,
                CreateDate = now,
                ExpiricyDate = now.Add(expiricyTime),
                Username = username
            };
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
        private Task<ClaimsIdentity> GetIdentityAsync(string username, string password)
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

            var usuarioConPrivilegios = _db.UsuarioRepository
                .Get(x => x.Username == username, includeProperties: "IdPrivilegioNavigation, CedulaNavigation");
            //Saco los privilegios del usuario
            var userClaims = usuarioConPrivilegios.Select(u => u.IdPrivilegioNavigation.NombrePrivilegio);
            var usuario = usuarioConPrivilegios.FirstOrDefault();

            //Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.          
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToString(), ClaimValueTypes.Integer64)
            };
            //Nota como puedes definir un role o claim y luego usarlo en el authorize.Tambien es interesante ver el payload del jwt
            foreach (var userClaim in userClaims)
            {
                //claims.Add(new Claim(userClaim, "True"));
                claims.Add(new Claim(ClaimTypes.Role, userClaim));
            }
            claims.Add(new Claim(ClaimTypes.Name, usuario.Username));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, $"{usuario.Cedula}"));
            claims.Add(new Claim(ClaimTypes.GivenName, usuario.CedulaNavigation.Nombre));
            claims.Add(new Claim(ClaimTypes.Surname, usuario.CedulaNavigation.Apellido));
            return claims;
        }
    }
}