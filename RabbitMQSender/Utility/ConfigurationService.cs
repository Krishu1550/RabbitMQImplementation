using System;
using MassTransit;
using RabbitMQ.Share.Service;
using RabbitMQ.Share.Service.IService;

namespace RabbitMQSender.Utility
{
    public  class ConfigurationService
	{

        ISubscriber _subscriber;

        public ConfigurationService()
        {
            
        }



        public Task StartAsync(CancellationToken cancellationToken)
        {
            var message= _subscriber.SubscribeAsync(ProcessMessage);
            return Task.CompletedTask;
        }


        private async  Task<string> ProcessMessage(string message, IDictionary<string, object> headers)
        {


            return await Task.FromResult(message);
        }
    }
}

