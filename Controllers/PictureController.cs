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
    public class PictureController : ControllerBase
    {
        static IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "NShal8LlUUL8hxn6BBQ4874HNXCIO2tAgN8izMgK",
            BasePath = "https://timewise-605da-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client = new FireSharp.FirebaseClient(config);

        private readonly ILogger<PictureController> _logger;

        public PictureController(ILogger<PictureController> logger)
        {
            _logger = logger;
        }

        [HttpPost("AddPicture")]
        public void AddPicture([FromBody] Picture picture)
        {

            var data = picture;
            PushResponse response = client.Push("pictures/", data);
            data.PictureId = response.Result.name;
            SetResponse setResponse = client.Set("pictures/" + data.PictureId, data);
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
        [HttpPost("EditPicture")]
        public void EditPicture(string? PictureId, [FromBody] Picture picture)
        {
            picture.PictureId = PictureId;
            SetResponse response = client.Set("pictures/" + PictureId, picture);

        }

        [HttpGet("GetPicture")]
        public Picture GetPicture(string? id)
        {
            FirebaseResponse response = client.Get("pictures/" + id);
            Picture data = JsonConvert.DeserializeObject<Picture>(response.Body);
            return data;
        }
        [HttpGet("GetAllPictures")]
        public List<Picture> GetAllPictures()
        {
            FirebaseResponse response = client.Get("pictures");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Picture>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Picture>(((JProperty)item).Value.ToString()));
                }
            }
            return list;
        }
        [HttpDelete("DeletePicture")]
        public void Delete(string? id)
        {
            FirebaseResponse response = client.Delete("pictures/" + id);
        }
    }
}