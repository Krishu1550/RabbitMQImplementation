using System;
using MassTransit;

namespace RabbitMQReciever.Extenions
{
	public class UserCommunicator
	{
		IBus _bus;
		IRequestClient<string> _client;
		
	}
}

