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
            AuthSecret = "NShal8LlUUL8hxn6BBQ4874HNXCIO2tAgN8izMgK",
            BasePath = "https://timewise-605da-default-rtdb.firebaseio.com"
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
            timesheet.TimesheetId = TimesheetId;
            SetResponse response = client.Set("timesheets/" + TimesheetId, timesheet);

        }

        [HttpGet("GetTimeSheet")]
        public Timesheet GetTimesheet(string? id)
        {
            FirebaseResponse response = client.Get("timesheets/" + id);
            Timesheet data = JsonConvert.DeserializeObject<Timesheet>(response.Body);
            return data;
        }
        [HttpGet("GetAllTimesheet")]
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
        public void Delete(string? id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("timesheets/" + id);
        }
    }
}