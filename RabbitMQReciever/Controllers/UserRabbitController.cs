using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Share.Model;

namespace RabbitMQReciever.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRabbitController : ControllerBase
    {

        [HttpGet("id")]
        public User GetUser(string userId)
        {




            return new User();
        }
    }
}
