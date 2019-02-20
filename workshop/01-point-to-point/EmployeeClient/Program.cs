using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmployeeClient
{
    class Program
    {
        const string API_CONFIG_SECTION = "employee-api";
        const string API_CONFIG_NAME_BASEURL = "baseUrl";
        const string API_CONFIG_NAME_EMPLOYEES_RESOURCE = "employees";

        private static IConfigurationRoot configuration;
        private static HttpClient apiClient;

        static void Main(string[] args)
        {
            LoadConfig();
            SetupHttpClient();
            var employees = ListEmployeesAsync().Result;

            Console.WriteLine($"Employee Count: {employees.Count}\n");
            foreach (var employee in employees)
            {
                Console.WriteLine($"Employee Name: {employee.FirstName}");
            }

            Console.ReadLine();
            Console.ResetColor();
        }

        private static async System.Threading.Tasks.Task<List<Employee>> ListEmployeesAsync()
        {
            Console.WriteLine($"Making request to {apiClient.BaseAddress}{API_CONFIG_NAME_EMPLOYEES_RESOURCE}");
            var response = apiClient.GetStreamAsync(API_CONFIG_NAME_EMPLOYEES_RESOURCE);
            var serializer = new DataContractJsonSerializer(typeof(List<Employee>));
            List<Employee> employees = serializer.ReadObject(await response) as List<Employee>;
            return employees;
        }

        private static void SetupHttpClient()
        {
            apiClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetSection(API_CONFIG_SECTION)[API_CONFIG_NAME_BASEURL])
            };

            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static void LoadConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();
        }
    }
}
