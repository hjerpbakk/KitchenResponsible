using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KitchenResponsibleService.Configuration;
using KitchenResponsibleService.Db;
using KitchenResponsibleService.Services;
using KitchenResponsibleService.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Hjerpbakk.ServiceDiscovery.Client;

namespace KitchenResponsibleService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // TODO: Add end-to-end tests like: https://github.com/seesharper/Blog.AspNetCoreUnitTesting
            // TODO: Add metrics https://github.com/Recognos/Metrics.NET
            // TODO: Add dashboard https://github.com/grafana/grafana
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc();

            var configuration = ReadConfig();
            var httpClient = new HttpClient {
                Timeout = TimeSpan.FromSeconds(15D)
            };
            var serviceDiscoveryClient = new ServiceDiscoveryClient(httpClient, configuration.ServiceDiscoveryUrl, configuration.ApiKey);
            configuration.ComicsServiceURL = serviceDiscoveryClient.GetServiceURL(configuration.ComicsServiceName).GetAwaiter().GetResult();

            services.AddSingleton<IComicsConfiguration>(configuration);
            services.AddSingleton<IBlobStorageConfiguration>(configuration);
            services.AddSingleton<IStorage, BlobStorage>();
            services.AddSingleton(serviceDiscoveryClient);
            services.AddSingleton<KitchenService>();
            services.AddSingleton(httpClient);
            services.AddSingleton<ComicsClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseStaticFiles();
        }

		static AppConfiguration ReadConfig()
		{
			return JsonConvert.DeserializeObject<AppConfiguration>(File.ReadAllText("config.json"));
		}
    }
}
