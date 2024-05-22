using System;
namespace RabbitMQ.Share.Service.IService
{
    public interface ISubscriber : IDisposable
    {
        void Subscribe(Func<string, IDictionary<string, object>, bool> callback);
        Task<string> SubscribeAsync(Func<string, IDictionary<string, object>, Task<string>> callback);
    }
}

