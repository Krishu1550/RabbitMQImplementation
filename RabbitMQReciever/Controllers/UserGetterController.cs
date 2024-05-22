using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Share.Model;


namespace RabbitMQReciever.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGetterController : ControllerBase
    {
        IBus _bus;
        IRequestClient<GetUserRequest> _client;
        public UserGetterController(IBus bus, IRequestClient<GetUserRequest> client)
        {

            _bus = bus;
            _client = client;
        }




        [HttpGet("id")]
        public async Task<User> GetUser(string id)
        {

            try
            {
                //var requestClient = _bus.CreateRequestClient<GetUserRequest>();
                //var response = await requestClient.GetResponse<User>(new GetUserRequest { UserId = id });
                //var request = _client.Create(new GetUserRequest { UserId = id });

                var reposone = await _client.GetResponse<User>(new GetUserRequest { UserId = id });
                 
                return reposone.Message;
            }
           
            catch (RequestTimeoutException ex)
            {
                // Handle timeout specifically
                return new User();
            }

            catch (Exception ex)
            {
                var mess = ex.Message;
            }

            return new User();
            
        }
    }


}