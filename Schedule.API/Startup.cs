using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schedule.Entities;
using Schedule.API.Models;
using Schedule.API.Helpers;

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
            //Esto es necesario para que me deje incluir propiedades extras de un model de ef
            services.AddMvc().AddJsonOptions(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddAutoMapper();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //services.AddDbContext<HorariosContext>(options => options.UseMySql(Configuration.GetConnectionString("HorariosContext")));
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
