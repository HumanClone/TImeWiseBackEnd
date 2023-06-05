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
            AuthSecret = "jIUM3jxE49wERUhPc5N4KeiIzDiyyLQ6aNKQWXLV",
            BasePath = "https://timewise-2ba0e-default-rtdb.firebaseio.com"
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
            FirebaseResponse FireResponse = client.Get("categories/" + CategoryId);
            Category data = JsonConvert.DeserializeObject<Category>(FireResponse.Body);
            data.CategoryId = CategoryId;
            if(category.UserId != null)
            {
                data.UserId = category.UserId;
            }
            if(category.Name != null)
            {
                data.Name = category.Name;
            }
            SetResponse response = client.Set("categories/" + CategoryId, data);

        }

        [HttpGet("GetCategory")]
        public Category GetCategory(string? CategoryId)
        {
            FirebaseResponse response = client.Get("categories/" + CategoryId);
            Category data = JsonConvert.DeserializeObject<Category>(response.Body);
            return data;
        }
        [HttpGet("GetAllUserCategories")]
        public List<Category> GetAllUserCategories(string? UserId)
        {
            FirebaseResponse response = client.Get("categories");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Category>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    Category temp = JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString());
                    if(temp.UserId == UserId)
                    {
                        list.Add(temp);
                    }
                }
            }
            return list;
        }
        [HttpGet("GetAllUserCategoriesWithHoursSum")]
        public List<Category> GetAllUserCategoriesWithHoursSum(string? UserId)
        {
            FirebaseResponse response = client.Get("categories");
            dynamic CategoryData = JsonConvert.DeserializeObject<dynamic>(response.Body);
            response = client.Get("timesheets");
            dynamic TimesheetData = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Category>();
            if (CategoryData != null)
            {
                foreach (var item in CategoryData)
                {
                    Category temp = JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString());
                    if (temp.UserId == UserId)
                    {
                        temp.TotalHours = 0;
                        foreach (var item2 in TimesheetData)
                        {
                            Timesheet timesheetTemp = JsonConvert.DeserializeObject<Timesheet>(((JProperty)item2).Value.ToString());
                            if(timesheetTemp.CategoryId == temp.CategoryId)
                            {
                                temp.TotalHours += timesheetTemp.Hours;
                            }
                        }
                        list.Add(temp);
                    }
                }
            }
            return list;
        }
        [HttpGet("GetAllUserCategoriesWithHoursSumWithinDateRange")]
        public List<Category> GetAllUserCategoriesWithHoursSumWithinDateRange(string? UserId, DateTime? start, DateTime? end)
        {
            FirebaseResponse response = client.Get("categories");
            dynamic CategoryData = JsonConvert.DeserializeObject<dynamic>(response.Body);
            response = client.Get("timesheets");
            dynamic TimesheetData = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Category>();
            if (CategoryData != null)
            {
                foreach (var item in CategoryData)
                {
                    Category temp = JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString());
                    if (temp.UserId == UserId)
                    {
                        temp.TotalHours = 0;
                        bool addTemp = false;
                        foreach (var item2 in TimesheetData)
                        {
                            Timesheet timesheetTemp = JsonConvert.DeserializeObject<Timesheet>(((JProperty)item2).Value.ToString());
                            if (timesheetTemp.CategoryId == temp.CategoryId)
                            {
                                if (end != null)
                                {
                                    if (timesheetTemp.StartDate.Value >= start.Value && timesheetTemp.StartDate <= end.Value)
                                    {
                                        addTemp = true;
                                        temp.TotalHours += timesheetTemp.Hours;
                                        
                                    }
                                }
                                else
                                {
                                    if (timesheetTemp.StartDate.Value >= start.Value)
                                    {
                                        addTemp = true;
                                        temp.TotalHours += timesheetTemp.Hours;
                                    }
                                }
                            }
                        }
                        if (addTemp)
                        {
                            list.Add(temp);
                        }
                    }
                }
            }
            return list;
        }
        [HttpGet("GetUserCategoryWithHoursSumWithinDateRange")]
        public List<Category> GetUserCategoryWithHoursSumWithinDateRange(string? UserId, string? CategoryId, DateTime? start, DateTime? end)
        {
            FirebaseResponse response = client.Get("categories");
            dynamic CategoryData = JsonConvert.DeserializeObject<dynamic>(response.Body);
            response = client.Get("timesheets");
            dynamic TimesheetData = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Category>();
            if (CategoryData != null)
            {
                foreach (var item in CategoryData)
                {
                    Category temp = JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString());
                    if (temp.UserId == UserId && temp.CategoryId == CategoryId)
                    {
                        temp.TotalHours = 0;
                        bool addTemp = false;
                        foreach (var item2 in TimesheetData)
                        {
                            Timesheet timesheetTemp = JsonConvert.DeserializeObject<Timesheet>(((JProperty)item2).Value.ToString());
                            if (timesheetTemp.CategoryId == temp.CategoryId)
                            {
                                if (end != null)
                                {
                                    if (timesheetTemp.StartDate.Value >= start.Value && timesheetTemp.StartDate <= end.Value)
                                    {
                                        addTemp = true;
                                        temp.TotalHours += timesheetTemp.Hours;
                                    }
                                }
                                else
                                {
                                    if (timesheetTemp.StartDate.Value >= start.Value)
                                    {
                                        addTemp = true;
                                        temp.TotalHours += timesheetTemp.Hours;
                                    }
                                }
                            }
                        }
                        if (addTemp)
                        {
                            list.Add(temp);
                        }
                    }
                }
            }
            return list;
        }
        [HttpGet("GetUserCategoryWithHoursSum")]
        public List<Category> GetUserCategoryWithHoursSum(string? UserId, string? CategoryId)
        {
            FirebaseResponse response = client.Get("categories/" + CategoryId);
            Category data = JsonConvert.DeserializeObject<Category>(response.Body);
            response = client.Get("timesheets");
            dynamic TimesheetData = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Category>();
            if (data.UserId == UserId)
            {
                data.TotalHours = 0;
                foreach (var item2 in TimesheetData)
                {
                    Timesheet timesheetTemp = JsonConvert.DeserializeObject<Timesheet>(((JProperty)item2).Value.ToString());
                    if (timesheetTemp.CategoryId == data.CategoryId)
                    {
                        data.TotalHours += timesheetTemp.Hours;
                    }
                }
                list.Add(data);
            }
            return list;
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
        public void Delete(string? CategoryId)
        {
            FirebaseResponse response = client.Delete("categories/" + CategoryId);
        }
    }
}