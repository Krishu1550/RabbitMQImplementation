using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Share.Service.IService;

namespace RabbitMQ.Share.Service
{
    public class Subscriber : ISubscriber
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly string _exchange;
        private readonly string _queue;
        private readonly IModel _model;
        private bool _disposed;

        public Subscriber(
            IConnectionProvider connectionProvider,
            string exchange,
            string queue,
            string routingKey,
            string exchangeType,
            int timeToLive = 30000,
            ushort prefetchSize = 10)
        {
            _connectionProvider = connectionProvider;
            _exchange = exchange;
            _queue = queue;
            _model = _connectionProvider.GetConnection().CreateModel();
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };
            _model.ExchangeDeclare(_exchange, exchangeType, arguments: ttl);
            _model.QueueDeclare(_queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _model.QueueBind(_queue, _exchange, routingKey);
            _model.BasicQos(0, prefetchSize, false);
        }

        public void Subscribe(Func<string, IDictionary<string, object>, bool> callback)
        {
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                bool success = callback.Invoke(message, e.BasicProperties.Headers);
                if (success)
                {
                    _model.BasicAck(e.DeliveryTag, true);
                }
            };

            _model.BasicConsume(_queue, false, consumer);
        }

        public async Task<string> SubscribeAsync(Func<string, IDictionary<string, object>, Task<string>> callback)
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            var tcs = new TaskCompletionSource<string>();

            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                if (!string.IsNullOrEmpty(message))
                {
                    var result = await callback.Invoke(message, e.BasicProperties.Headers);
                    _model.BasicAck(e.DeliveryTag, true);
                    tcs.SetResult(result); // Set the result of the task completion source
                }
                else
                {
                    tcs.SetResult(null); // Set null if message is empty
                }
            };

            _model.BasicConsume(_queue, false, consumer);

            // Return the task from the task completion source
            return await tcs.Task;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _model?.Close();

            _disposed = true;
        }
    }
}

