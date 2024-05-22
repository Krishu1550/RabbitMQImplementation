using System;
using RabbitMQ.Client;

namespace RabbitMQ.Share.Service.IService
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}

