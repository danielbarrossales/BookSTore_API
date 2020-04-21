using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System.Reflection;
using System.IO;
using BookStore_API.Contracts;
using BookStore_API.Services;
using AutoMapper;
using BookStore_API.Mappings;

namespace BookStore_API
{
    public class Startup
    {
        public static string XML_DOC_PATH = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        public static string SWAGGER_TITLE = "Book Store API";
        public static string SWAGGER_VERSION = "v1";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new NpgsqlConnectionStringBuilder(Configuration.GetConnectionString("DefaultConnection"));
            var storageCredentials = Configuration.GetSection("BookStore-API-Credentials");

            builder.Username = storageCredentials["Username"];
            builder.Password = storageCredentials["Password"];

            services.AddCors(o => {
                o.AddPolicy("CorsPolicy",
                    constructor => constructor.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<BookStoreContext>(
                    options => options.UseNpgsql(builder.ConnectionString));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BookStoreContext>();

            services.AddAutoMapper(typeof(Maps));

            services.AddSwaggerGen (c => 
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = SWAGGER_TITLE,
                    Version = SWAGGER_VERSION,
                    Description = "Educational API for a Book Store",
                }
                );

                var xfile = XML_DOC_PATH;
                var xpath = Path.Combine(AppContext.BaseDirectory, xfile);

                c.IncludeXmlComments(xpath);
            });

            services.AddSingleton<ILoggerService, LoggerService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{SWAGGER_VERSION}/swagger.json", SWAGGER_TITLE);
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
