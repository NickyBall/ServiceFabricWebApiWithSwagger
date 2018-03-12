using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace PersonApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, StorageClient Storage, string Url)
        {
            Configuration = configuration;
            this.Url = Url;
        }

        public IConfiguration Configuration { get; }
        public string Url { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "PersonApi.xml");
                c.IncludeXmlComments(xmlPath);
            });



            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                //.AddInMemoryApiResources(Config.GetApiResources())
                //.AddInMemoryClients(Config.GetClients());
                .AddResourceStore<ResourceStore>()
                .AddClientStore<ClientStore>();

            //services.AddTransient<IResourceStore, ResourceStore>();
            //services.AddTransient<IClientStore, ClientStore>();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Url;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";
                    
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, StorageClient Storage, string Url)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UsePathBase("/{test}");
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseIdentityServer();
            app.UseMvc();
        }
    }
}
