using System;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace PersonApi
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class PersonApi : StatelessService
    {
        public PersonApi(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new HttpSysCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        string host = serviceContext.NodeContext.IPAddressOrFQDN;
                        var endpointconfig = serviceContext.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
                        int port = endpointconfig.Port;
                        var scheme = endpointconfig.Protocol.ToString();
                        string UrlEndpoint = $"{scheme}://{host}:{port}";
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");
                        StorageClient Storage = new StorageClient();
                        return new WebHostBuilder()
                                    //.UseKestrel()
                                    .UseHttpSys(options =>
                                    {
                                        //options.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.Negotiate | Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.NTLM;
                                        //options.Authentication.AllowAnonymous = true;
                                    })
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatelessServiceContext>(serviceContext)
                                            .AddSingleton(Storage)
                                            .AddSingleton(UrlEndpoint)
                                    )
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url + "/xxx")
                                    //.Start(url + "/xxx");
                                    .Build();
                    }))
            };
        }
    }
}
