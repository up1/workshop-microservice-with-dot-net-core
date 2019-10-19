using Jaeger;
using Jaeger.Samplers;
using Microsoft.Extensions.Logging;

namespace JaegerWrapper
{
    public static class Tracing
    {
        public static Tracer Init(string serviceName, ILoggerFactory loggerFactory)
        {
            var senderConfiguration = new Configuration.SenderConfiguration(loggerFactory)
                .WithAgentHost("jaeger").WithAgentPort(6831);
            
            var samplerConfiguration = new Configuration.SamplerConfiguration(loggerFactory)
                .WithType(ConstSampler.Type)
                .WithParam(1);

            var reporterConfiguration = new Configuration.ReporterConfiguration(loggerFactory)
                .WithSender(senderConfiguration)
                .WithLogSpans(true);

            //var config = Configuration.FromEnv(loggerFactory);
            //config.WithSampler(samplerConfiguration);
            //config.WithReporter(reporterConfiguration);

            //return (Tracer)config.GetTracer();

            return (Tracer) new Configuration(serviceName, loggerFactory)
                .WithSampler(samplerConfiguration)
                .WithReporter(reporterConfiguration)
                .GetTracer();
        }
    }
}