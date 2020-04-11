using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiPractice.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace CoreApiPractice
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ShopContext>(options => options.UseInMemoryDatabase("shop"));
            services.AddControllers().ConfigureApiBehaviorOptions(options=> {

                // This filter check the paramater values are tur of not.... 
                   //if not then run the default behaviour query..
                options.SuppressModelStateInvalidFilter = true;
            
            });

            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", option =>
            {
                option.Authority = "http://localhost:52277";
                option.RequireHttpsMetadata = false;


                option.Audience = "htp-api";

            });

            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(builder =>
            //    {
            //        builder.WithOrigins("https://localhose:44332")
            //        .AllowAnyHeader().AllowAnyMethod();
            //    });
            //});

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
                {
                    builder.WithOrigins("https://localhost:44332").AllowAnyHeader().AllowAnyMethod();
                });
            });

            // to Configure the Api Versioning 
            services.AddApiVersioning(options =>
            {

                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1,0);
                options.AssumeDefaultVersionWhenUnspecified = true;

                //when we define ApiVersionReader then we don't need to write the version in Url, 
                // we write the version in the header

              //  options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
              
               // If you want to Implement Api vERion in Query STring Simply Remove the About live ApiVersionReader

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
