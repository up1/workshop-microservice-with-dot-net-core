using System;
using System.IO;
using EmployeeConsumer.Infrastructures;
using Microsoft.Extensions.Configuration;

namespace EmployeeConsumer
{
    class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var rabbitSettings = configuration.GetSection("rabbitmq-settings");

            var config = new QueueConfig
            {
                HostName = rabbitSettings["hostName"],
                UserName = rabbitSettings["userName"],
                Password = rabbitSettings["password"],
                QueueName = rabbitSettings["sendQueue"]
            };

            DisplayRabbitSettings(config);
            Console.WriteLine("Starting Employee Service Queue Processor....");
            Console.WriteLine();


            var processor = new QueueProcessor(config);
            processor.Start();
        }

        private static void DisplayRabbitSettings(QueueConfig config)
        {
            Console.WriteLine("*********************");
            Console.WriteLine("Host: {0}", config.HostName);
            Console.WriteLine("Username: {0}", config.UserName);
            Console.WriteLine("QueueName: {0}", config.QueueName);
            Console.WriteLine("*********************");
            Console.WriteLine();
        }
    }
}
