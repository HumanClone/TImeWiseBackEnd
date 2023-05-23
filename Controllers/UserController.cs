using Microsoft.AspNetCore.Mvc;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimeWise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "NShal8LlUUL8hxn6BBQ4874HNXCIO2tAgN8izMgK",
            BasePath = "https://timewise-605da-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client;
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost("AddUser")]
        public void AddUser([FromBody] User user)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = user;
            PushResponse response = client.Push("users/", data);
            data.Id = response.Result.name;
            SetResponse setResponse = client.Set("users/" + data.Id, data);
            Console.WriteLine("status Code: " + setResponse.StatusCode);
            if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ModelState.AddModelError(string.Empty, "Added Succesfully");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Something went wrong!!");
            }
        }

        [HttpGet("GetUser")]
        public User GetUser()
        {
            return new User
            {
                Name = "Dylan Hall",
                Email = "dylanscotthall@gmail.com",
                Job = "Dickhead",
                Password = "password",
            };
        }
        [HttpGet("GetAllUsers")]
        public User GetAllUsers()
        {
            return new User
            {
                Name = "Ted Hall",
                Email = "dylanscotthall@gmail.com",
                Job = "Dickhead",
                Password = "password",
            };
        }
    }
}