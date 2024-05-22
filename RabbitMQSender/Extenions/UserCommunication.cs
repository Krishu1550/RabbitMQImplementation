using System;
using System.Xml.Linq;
using RabbitMQ.Share.Model;

namespace RabbitMQSender.Extenions
{
	public class UserCommunication
	{

        private List<User> Users;

        public UserCommunication()

        {
            Users = new List<User>()
            {
               new User()
        {
            Id = new Guid("efefvefv"),
                   Name = "Shailesh"
               },
               new User()
        {
            Id = new Guid("dwdcdc"),
                   Name = "Kumar"
               },
               new User()
        {
            Id = new Guid("Fjfklrj"),
                   Name = "Krishu"
               }
    };
        }

}
}

