using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Schedule.API.Models;
using Schedule.API.Helpers;
using Schedule.Entities;
using System;
using System.Text;

namespace Schedule.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // The secret key every token will be signed with.
        // In production, you should store this securely in environment variables
        // or a key management tool. Don't hardcode this into your application!
        private static string secretKey;
        private AppSettings _appSettings;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Con esto puedes hacer el ajax
            services.AddCors();
            //Esto es necesario para que me deje incluir propiedades extras de un model de ef
            services.AddMvc().AddJsonOptions(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddAutoMapper();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddDbContext<HorariosContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("HorariosContext")));

            _appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            secretKey = _appSettings.TokenSettings.SecretKey; //Configuration.GetSection("AppSettings").Get<AppSettings>().Token.SecretKey;
            //Con estas lineas le decimos como debe validar en los Authorize
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = _appSettings.TokenSettings.Issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudiences = _appSettings.TokenSettings.Audiences,

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(o =>
                {
                    //o.Authority = "ExampleIssuer";
                    //o.Audience = "ExampleAudience";
                    // You also need to update /wwwroot/app/scripts/app.js
                    //o.Authority = Configuration["jwt:authority"]; // Your Configuration
                    //o.Audience = Configuration["jwt:audience"]; // // Your Configuration
                    o.TokenValidationParameters = tokenValidationParameters; // your tokenValidationParameter
                });
            //Como estamos generando claims basados en lo que traiga de la db
            //aca registramos el nombre de la Policy y el valor que debe tener
            //para que pase el Authorize(Policy = "Profesor")
            // services.AddAuthorization(options =>
            //         options.AddPolicy("Profesor", policy => policy.RequireClaim("Profesor", "True")))
            //     .AddAuthorization(options => 
            //         options.AddPolicy("Administrador", policy => policy.RequireClaim("Administrador","True"))
            //);
            //o puedes agregar roles Authorize(Roles = "Profesor, Administrador")
            // services
            //     .AddAuthorization(options => 
            //         options.AddPolicy("Administrador", policy => policy.RequireRole("Administrator")))
            //     .AddAuthorization(options => 
            //         options.AddPolicy("Profesor",policy => policy.RequireRole("Profesor"))
            // );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Con esto puedes hacer el ajax, abria q checar bien las opciones
            app.UseCors(builder
                => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());
            //Esto para que muestre las paginas de errores?
            app.UseStaticFiles();

            // Add JWT generation endpoint:
            //Con estas lineas se generan los tokens, notese que se usa la clase personalizada
            //TokenProviderOptions
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var options = new TokenProviderOptions
            {
                Audience = _appSettings.TokenSettings.Audiences,
                Issuer = _appSettings.TokenSettings.Issuer,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };
            //y con esto se registra el middleware que escucha por las peticiones
            //para un token
            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));

            //Esta linea habilita que se pueda usar Authorize en los metodos
            app.UseAuthentication();

            app.UseMvc();

        }
    }
}
