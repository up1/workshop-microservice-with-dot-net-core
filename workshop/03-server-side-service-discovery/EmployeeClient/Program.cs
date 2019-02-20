using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmployeeClient
{
    class Program
    {
        private static IConfigurationRoot configuration;
        private static ApiClient apiClient;

        static void Main(string[] args)
        {
            LoadConfig();
            apiClient = new ApiClient(configuration);
            try
            {
                apiClient.Initialize().Wait();
                ListEmployees().Wait();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to request resource");
            }
        }

        private static async Task ListEmployees()
        {
            var employees = await apiClient.GetEmployees();
            Console.WriteLine($"Employee Count: {employees.Count()}\n");
            foreach (var employee in employees)
            {
                Console.WriteLine($"Employee Name: {employee.FirstName}");
            }

            Console.ReadLine();
            Console.ResetColor();
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
