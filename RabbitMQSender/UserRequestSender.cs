using MassTransit;
using RabbitMQ.Share.Model;


namespace RabbitMQSender
{
	public class UserRequestSender : IConsumer<GetUserRequest>
    {
        private List<User> Users;

        public UserRequestSender()
        {

            Users = new List<User>()
            {
               new User()
               {
                   Id=new Guid("0d6dfff6-11c1-44d8-832c-0056441fe264"),
                   Name= "Shailesh"
               },
               new User()
               {
                   Id= new Guid("264c04dc-b2fd-4301-8956-01b5f9d0796d"),
                   Name= "Kumar"
               },
               new User()
               {
                   Id= new Guid("6121f648-32ec-4824-9354-81112316875b"),
                   Name= "Krishu"
               }
            };
        }

        public async Task Consume(ConsumeContext<GetUserRequest> context)
        {
            var userId = context.Message.UserId;

           var user=  Users.Where(x => x.Id == new Guid(userId)).FirstOrDefault();

            await context.RespondAsync<User>(user);

        }
    }

  
}

