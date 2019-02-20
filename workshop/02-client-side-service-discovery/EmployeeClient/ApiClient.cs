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

namespace EmployeeClient
{
    public class Config
    {
        public string BaseUrl { get; set; }
        public string EmployeeResource { get; set; }
    }

    public class ApiClient
    {
        private const string API_CONFIG_SECTION = "employee-api";

        private readonly List<Config> serverConfigs;
        private readonly HttpClient apiClient;
        private readonly AsyncRetryPolicy serverRetryPolicy;
        private int currentConfigIndex;

        public ApiClient(IConfigurationRoot configuration)
        {
            apiClient = new HttpClient();

            serverConfigs = new List<Config>();
            configuration.GetSection(API_CONFIG_SECTION).Bind(serverConfigs);

            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var retries = serverConfigs.Count() * 2 - 1;
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
                var config = serverConfigs[currentConfigIndex];
                var requestPath = $"{config.BaseUrl}{config.EmployeeResource}";

                var response = apiClient.GetStreamAsync(requestPath);
                var serializer = new DataContractJsonSerializer(typeof(List<Employee>));
                List<Employee> employees = serializer.ReadObject(await response) as List<Employee>;
                return employees;
            });
        }

        public static implicit operator HttpClient(ApiClient v)
        {
            throw new NotImplementedException();
        }
    }
}
