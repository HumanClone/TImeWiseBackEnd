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
    public class CategoryController : ControllerBase
    {
        static IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "NShal8LlUUL8hxn6BBQ4874HNXCIO2tAgN8izMgK",
            BasePath = "https://timewise-605da-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client = new FireSharp.FirebaseClient(config);

        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        [HttpPost("AddCategory")]
        public void AddCategory([FromBody] Category category)
        {

            var data = category;
            PushResponse response = client.Push("categories/", data);
            data.CategoryId = response.Result.name;
            SetResponse setResponse = client.Set("categories/" + data.CategoryId, data);
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
        [HttpPost("EditCategory")]
        public void EditCategory(string? CategoryId, [FromBody] Category category)
        {
            category.CategoryId = CategoryId;
            SetResponse response = client.Set("categories/" + CategoryId, category);

        }

        [HttpGet("GetCategory")]
        public Category GetCategory(string? id)
        {
            FirebaseResponse response = client.Get("categories/" + id);
            Category data = JsonConvert.DeserializeObject<Category>(response.Body);
            return data;
        }
        [HttpGet("GetAllCategories")]
        public List<Category> GetAllCategories()
        {
            FirebaseResponse response = client.Get("categories");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Category>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString()));
                }
            }
            return list;
        }
        [HttpDelete("DeleteCategory")]
        public void Delete(string? id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("categories/" + id);
        }
    }
}