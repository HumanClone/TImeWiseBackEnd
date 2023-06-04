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
    public class TimesheetController : ControllerBase
    {
        static IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "jIUM3jxE49wERUhPc5N4KeiIzDiyyLQ6aNKQWXLV",
            BasePath = "https://timewise-2ba0e-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client = new FireSharp.FirebaseClient(config);

        private readonly ILogger<TimesheetController> _logger;

        public TimesheetController(ILogger<TimesheetController> logger)
        {
            _logger = logger;
        }

        [HttpPost("AddTimesheet")]
        public void AddTimesheet([FromBody] Timesheet timesheet)
        {

            var data = timesheet;
            PushResponse response = client.Push("timesheets/", data);
            data.TimesheetId = response.Result.name;
            SetResponse setResponse = client.Set("timesheets/" + data.TimesheetId, data);
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
        [HttpPost("EditTimesheet")]
        public void EditTimesheet(string? TimesheetId, [FromBody] Timesheet timesheet)
        {
            FirebaseResponse FireResponse = client.Get("timesheets/" + TimesheetId);
            Timesheet data = JsonConvert.DeserializeObject<Timesheet>(FireResponse.Body);
            data.TimesheetId = TimesheetId;
            if(timesheet.CategoryId != null)
            {
                data.CategoryId = timesheet.CategoryId;
            }
            if(timesheet.PictureId != null)
            {
                data.PictureId = timesheet.PictureId;
            }
            if(timesheet.Description != null)
            {
                data.Description = timesheet.Description;
            }
            if(timesheet.Hours != null)
            {
                data.Hours = timesheet.Hours;
            }
            SetResponse response = client.Set("timesheets/" + TimesheetId, data);

        }

        [HttpGet("GetTimeSheet")]
        public Timesheet GetTimesheet(string? TimesheetId)
        {
            FirebaseResponse response = client.Get("timesheets/" + TimesheetId);
            Timesheet data = JsonConvert.DeserializeObject<Timesheet>(response.Body);
            return data;
        }
        [HttpGet("GetAllUserTimesheets")]
        public List<Timesheet> GetAllUserTimesheets(string? UserId)
        {
            FirebaseResponse response = client.Get("timesheets");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Timesheet>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    Timesheet temp = JsonConvert.DeserializeObject<Timesheet>(((JProperty)item).Value.ToString());
                    if (temp.UserId == UserId)
                    {
                        list.Add(temp);
                    }
                }
            }
            return list;
        }
        [HttpGet("GetAllTimesheetsOnWeeks")]
        public List<Timesheet> GetAllTimesheetsOnWeeks(string? UserId, DateTime? date)
        {
            FirebaseResponse response = client.Get("timesheets");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Timesheet>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    Timesheet temp = JsonConvert.DeserializeObject<Timesheet>(((JProperty)item).Value.ToString());
                    if(temp.UserId == UserId)
                    {
                        if ((int)((temp.StartDate.Value.DayOfYear / 365.25) * 52) == (int)((date.Value.DayOfYear / 365.25) * 52))
                        {
                            list.Add(temp);
                        }
                    }
                }
            }
            return list;
        }
        [HttpGet("GetAllTimesheetsOnMonths")]
        public List<Timesheet> GetAllTimesheetsOnMonths(string? UserId, DateTime? date)
        {
            FirebaseResponse response = client.Get("timesheets");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Timesheet>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    Timesheet temp = JsonConvert.DeserializeObject<Timesheet>(((JProperty)item).Value.ToString());
                    if(temp.UserId == UserId)
                    {
                        if (temp.StartDate.Value.Month == date.Value.Month)
                        {
                            list.Add(temp);
                        }
                    }
                }
            }
            return list;
        }
        [HttpGet("GetAllTimesheetsInRange")]
        public List<Timesheet> GetAllTimesheetsInRange(DateTime? start, DateTime? end, string? UserId)
        {
            FirebaseResponse response = client.Get("timesheets");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Timesheet>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    Timesheet temp = JsonConvert.DeserializeObject<Timesheet>(((JProperty)item).Value.ToString());
                    if (temp.UserId == UserId)
                    {
                        if(end != null)
                        {
                            if(temp.StartDate.Value >= start.Value && temp.StartDate <= end.Value)
                            {
                                list.Add(temp);
                            }
                        }
                        else
                        {
                            if(temp.StartDate.Value >= start.Value)
                            {
                                list.Add(temp);
                            }
                        }
                    }
                }
            }
            return list;
        }
        [HttpGet("GetAllTimesheetsOfUserCategory")]
        public List<Timesheet> GetAllTimesheetOfUserCategory(string? UserId, string? CategoryId)
        {
            FirebaseResponse response = client.Get("timesheets");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Timesheet>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    Timesheet temp = JsonConvert.DeserializeObject<Timesheet>(((JProperty)item).Value.ToString());
                    if (temp.UserId == UserId && temp.CategoryId == CategoryId)
                    {
                        list.Add(temp);
                    }
                }
            }
            return list;
        }
        [HttpGet("GetAllTimesheetsInRangeAndCategory")]
        public List<Timesheet> GetAllTimesheetsInRangeAndCategory(DateTime? start, DateTime? end, string? UserId, string? CategoryId)
        {
            FirebaseResponse response = client.Get("timesheets");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Timesheet>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    Timesheet temp = JsonConvert.DeserializeObject<Timesheet>(((JProperty)item).Value.ToString());
                    if (temp.UserId == UserId && temp.CategoryId == CategoryId)
                    {
                        if (end != null)
                        {
                            if (temp.StartDate.Value >= start.Value && temp.StartDate <= end.Value)
                            {
                                list.Add(temp);
                            }
                        }
                        else
                        {
                            if (temp.StartDate.Value >= start.Value)
                            {
                                list.Add(temp);
                            }
                        }
                    }
                }
            }
            return list;
        }
        [HttpGet("GetAllTimesheets")]
        public List<Timesheet> GetAllTimesheets()
        {
            FirebaseResponse response = client.Get("timesheets");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Timesheet>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Timesheet>(((JProperty)item).Value.ToString()));
                }
            }
            return list;
        }
        [HttpDelete("DeleteTimesheet")]
        public void Delete(string? TimesheetId)
        {
            FirebaseResponse response = client.Delete("timesheets/" + TimesheetId);
        }
    }
}