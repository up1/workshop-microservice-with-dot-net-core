using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmployeeApi.Infrastructures
{
    public class ConsulHostedService : IHostedService
    {
        private Task executingTask;
        private CancellationTokenSource cts;
        private readonly IConsulClient consulClient;
        private readonly IOptions<ConsulConfig> consulConfig;
        private readonly ILogger<ConsulHostedService> logger;
        private readonly IServer server;
        private string registrationID;

        public ConsulHostedService(IConsulClient consulClient, IOptions<ConsulConfig> consulConfig, ILogger<ConsulHostedService> logger, IServer server)
        {
            this.server = server;
            this.logger = logger;
            this.consulConfig = consulConfig;
            this.consulClient = consulClient;

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a linked token so we can trigger cancellation outside of this token's cancellation
            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            var features = server.Features;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();

            var uri = new Uri(address);
            registrationID = $"{consulConfig.Value.ServiceID}-{uri.Port}";

            var registration = new AgentServiceRegistration()
            {
                ID = registrationID,
                Name = consulConfig.Value.ServiceName,
                //Address = $"{uri.Scheme}://{uri.Host}",
                Address = $"{uri.Scheme}://employee",
                Port = uri.Port,
                Tags = new[] { "Employees" }
            };

            logger.LogInformation("Registering in Consul");
            await consulClient.Agent.ServiceDeregister(registration.ID, cts.Token);
            await consulClient.Agent.ServiceRegister(registration, cts.Token);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            cts.Cancel();
            logger.LogInformation("Unregistering from Consul");
            try
            {
                await consulClient.Agent.ServiceDeregister(registrationID, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Deregisteration failed");
            }
        }
    }
}
