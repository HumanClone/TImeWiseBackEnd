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
        static IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "NShal8LlUUL8hxn6BBQ4874HNXCIO2tAgN8izMgK",
            BasePath = "https://timewise-605da-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client = new FireSharp.FirebaseClient(config);

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost("AddUser")]
        public void AddUser([FromBody] User user)
        {

            var data = user;
            PushResponse response = client.Push("users/", data);
            data.UserId = response.Result.name;
            SetResponse setResponse = client.Set("users/" + data.UserId, data);
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
        [HttpPost("EditUser")]
        public void EditUser(string? UserId, [FromBody] User user)
        {
            user.UserId = UserId;
            SetResponse response = client.Set("users/" + UserId, user);

        }

        [HttpGet("GetUser")]
        public User GetUser(string? id)
        {
            FirebaseResponse response = client.Get("users/" + id);
            User data = JsonConvert.DeserializeObject<User>(response.Body);
            return data;
        }
        [HttpGet("GetAllUsers")]
        public List<User> GetAllUsers()
        {
            FirebaseResponse response = client.Get("users");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<User>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<User>(((JProperty)item).Value.ToString()));
                }
            }
            return list;
        }
        [HttpDelete("DeleteUser")]
        public void Delete(string? id)
        {
            FirebaseResponse response = client.Delete("users/" + id);
        }
    }
}