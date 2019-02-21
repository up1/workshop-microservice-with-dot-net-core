using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmployeeProducer
{
    class Program
    {
        private static IConfigurationRoot _configuration;
        private static ServiceClient _apiClient;
        public static void Main(string[] args)
        {
            LoadConfig();

            ILogger<ServiceClient> logger = new LoggerFactory().AddConsole().CreateLogger<ServiceClient>();
            _apiClient = new ServiceClient(_configuration, logger);

            using (_apiClient)
            {
                try
                {
                    ListEmployees();
                }
                catch (Exception)
                {
                    logger.LogError("Unable to request resource");
                }
                Console.ReadLine();
            }
        }

        private static void LoadConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
        }

        private static void ListEmployees()
        {
            var employees = _apiClient.GetEmployees();
            Console.WriteLine($"Employee Count: {employees.Count()}");
        }


    }
}
