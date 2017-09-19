using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Schedule.BLL;

namespace Schedule.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Con esto puedes hacer el ajax
            services.AddCors();
            services.AddMvc();
            //Para que se registre nuestro servicio de tokens
            services.AddSingleton<ITokenService, TokenServices>();
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
            app.UseMvc();

        }
    }
}
