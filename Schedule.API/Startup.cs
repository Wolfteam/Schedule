﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Schedule.API.Helpers;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using Swashbuckle.AspNetCore.Swagger;
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
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.Indented;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Schedule API",
                    Description = "This is the schedule sample api",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Efrain Bastidas",
                        Email = "mimo4325@gmail.com",
                        Url = "https://github.com/Wolfteam"
                    }
                });
            });
            services.AddAutoMapper();
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
            services.AddDbContext<HorariosContext>(options =>
                options.UseMySql(Configuration.GetConnectionString(nameof(HorariosContext))));


            services.AddScoped<IAulasRepository, AulasRepository>();
            services.AddScoped<ICarrerasRepository, CarrerasRepository>();
            services.AddScoped<IDisponibilidadProfesorRepository, DisponibilidadProfesorRepository>();
            services.AddScoped<IHorarioProfesorRepository, HorarioProfesorRepository>();
            services.AddScoped<IMateriasRepository, MateriasRepository>();
            services.AddScoped<IPeriodoCarreraRepository, PeriodoCarreraRepository>();
            services.AddScoped<IPrioridadesRepository, PrioridadesRepository>();
            services.AddScoped<IPrivilegiosRepository, PrivilegiosRepository>();
            services.AddScoped<IProfesorMateriaRepository, ProfesorMateriaRepository>();
            services.AddScoped<IProfesorRepository, ProfesorRepository>();
            services.AddScoped<ISeccionesRepository, SeccionesRepository>();
            services.AddScoped<ISemestresRepository, SemestresRepository>();
            services.AddScoped<ITipoAulaMateriaRepository, TipoAulaMateriaRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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

            app.UseDefaultFiles();
            //Esto para que muestre las paginas de errores?
            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                // To serve the Swagger UI at (http://localhost:<random_port>/swagger)
                c.SwaggerEndpoint("../swagger/v1/swagger.json", $"Schedule Api V1 {(env.IsDevelopment() ? "Debug" : "Production")} mode");
                c.DocumentTitle = "Schedule API";
            });

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
