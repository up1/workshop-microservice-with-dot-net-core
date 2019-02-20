using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Consul;

namespace EmployeeClient
{
    public class ApiClient
    {
        private readonly List<Uri> serverConfigs;
        private readonly HttpClient apiClient;
        private AsyncRetryPolicy serverRetryPolicy;
        private int currentConfigIndex;
        private ConsulClient consulClient;

        private readonly IConfigurationRoot configuration;

        public ApiClient(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
            apiClient = new HttpClient();
            serverConfigs = new List<Uri>();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task Initialize()
        {
            consulClient = new ConsulClient(c =>
            {
                var uri = new Uri(configuration["consulConfig:address"]);
                c.Address = uri;
            });


            var services = await consulClient.Agent.Services();
            foreach (var service in services.Response)
            {
                var isEmployeesApi = service.Value.Tags.Any(t => t == "Employees");
                if (isEmployeesApi)
                {
                    var serviceUri = new Uri($"{service.Value.Address}:{service.Value.Port}");
                    Console.WriteLine($"Employee API: {serviceUri}");
                    serverConfigs.Add(serviceUri);
                }
            }

            var retries = serverConfigs.Count * 2 - 1;

            serverRetryPolicy = Policy.Handle<HttpRequestException>()
               .RetryAsync(retries, (exception, retryCount) =>
               {
                   chooseNextServer(retryCount);
               });
        }

        private void chooseNextServer(int retryCount)
        {
            Console.WriteLine($"Retry Count: {retryCount}\n");
            if (retryCount % 2 == 0)
            {
                currentConfigIndex++;

                if (currentConfigIndex > serverConfigs.Count() - 1)
                    currentConfigIndex = 0;
            }
        }

        public virtual Task<List<Employee>> GetEmployees()
        {
            return serverRetryPolicy.ExecuteAsync(async () =>
            {
                var serverUrl = serverConfigs[currentConfigIndex];
                var requestPath = $"{serverUrl}api/employees";

                var response = apiClient.GetStreamAsync(requestPath);
                var serializer = new DataContractJsonSerializer(typeof(List<Employee>));
                List<Employee> employees = serializer.ReadObject(await response) as List<Employee>;
                return employees;
            });
        }

        public async Task CheckServerHealth()
        {
            var checks = await consulClient.Health.Service(configuration["consulConfig:serviceName"]);
            foreach (var entry in checks.Response)
            {
                var check = entry.Checks.SingleOrDefault(c => c.ServiceName == "employee-api");
                if (check == null) continue;
                var isPassing = check.Status == HealthStatus.Passing;
                var serviceUri = new Uri($"{entry.Service.Address}:{entry.Service.Port}");
                if (isPassing)
                {
                    if (!serverConfigs.Contains(serviceUri))
                    {
                        serverConfigs.Add(serviceUri);
                    }
                }
                else
                {
                    if (serverConfigs.Contains(serviceUri))
                    {
                        serverConfigs.Remove(serviceUri);
                    }
                }
            }
        }
    }
}
