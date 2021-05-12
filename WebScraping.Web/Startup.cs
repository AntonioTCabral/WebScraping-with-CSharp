using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebScraping.Infrastructure.Data;
using WebScraping.Infrastructure.Repository;

namespace WebScraping.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            var connection = Configuration["ConexaoMySql:MySqlConnectionString"];
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connection)
            );
            
            var assembly = AppDomain.CurrentDomain.Load("WebScraping.Core");
            services.AddMediatR(assembly);
            
            services.AddHttpClient();
            
            services.AddSwaggerGen(c =>
            {
                c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "sApi", Version = "v1" });
            });
            
            return BuildDependencyInjectionProvider(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1 - " + env.EnvironmentName);
                c.RoutePrefix = "";
                c.DocExpansion(DocExpansion.None);
            });
            
            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                            }
                        });
                });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private static IServiceProvider BuildDependencyInjectionProvider(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            var webAssembly = Assembly.GetExecutingAssembly();
            var empresaRepositoryAssembly = Assembly.GetAssembly(typeof(FileInformationRepository));
          
            builder.RegisterAssemblyTypes(webAssembly, empresaRepositoryAssembly).AsImplementedInterfaces();

            var applicationContainer = builder.Build();

            return new AutofacServiceProvider(applicationContainer);
        }
    }
}