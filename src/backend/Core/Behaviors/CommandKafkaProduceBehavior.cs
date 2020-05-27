using Confluent.Kafka;
using Core.Commands;
using Core.Configurations;
using Core.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Behaviors
{
    public class CommandKafkaProduceBehavior<TCommand, TResponse, TKafkaConfigurations> : CommandPipelineBehaviorBase<TCommand, TResponse>
        where TCommand : ICommandBase
        where TKafkaConfigurations : IKafkaConfigurations
    {
        private readonly Notifiable _notifiable;
        private readonly IKafkaConfigurations _kafkaConfigurations;
        
        public CommandKafkaProduceBehavior(Notifiable notifiable, IKafkaConfigurations kafkaConfigurations, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _notifiable = notifiable;
            _kafkaConfigurations = kafkaConfigurations;
        }

        protected override async Task<TResponse> Process(TCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var nextHandler = await next();

            if (_notifiable.IsValid())
            {
                var conf = new ProducerConfig { 
                    BootstrapServers = _kafkaConfigurations.BootstrapServers
                };

                void handler(DeliveryReport<Null, string> report)
                {
                    if (report.Error.IsError)
                    {
                        _notifiable.Notify("kafka", $"erro ao enviar mensagem : {report.Error.Reason}");
                        Logger.LogWarning($"kafka - erro ao enviar mensagem : {report.Error.Reason}");
                        return;
                    }

                    Logger.LogInformation($"kafka - Mensagem entrege: {report.TopicPartitionOffset}");
                }

                var value = System.Text.Json.JsonSerializer.Serialize(request);

                using var producer = new ProducerBuilder<Null, string>(conf).Build();

                producer.Produce(_kafkaConfigurations.Topic, new Message<Null, string> { Value = value }, handler);

                producer.Flush(TimeSpan.FromSeconds(10));
            }

            return nextHandler;
        }

    }
}
