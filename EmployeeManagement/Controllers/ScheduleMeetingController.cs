using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class ScheduleMeetingController : Controller
    {
        Uri baseUrl;
        HttpClient httpClient;
        private IConfiguration _config;

        public ScheduleMeetingController(IConfiguration config)
        {
            _config = config;
            baseUrl = new Uri(_config["APIService:Meeting"]);
            httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
        }



        public IActionResult Index()
        {
            return View(new List<MeetingData>());
        }

        public ActionResult MeetingData()
        {
            List<MeetingEvent> meetingEvents = new List<MeetingEvent>();

            List<MeetingData> meetingDatas = new List<MeetingData>();
            HttpResponseMessage httpResponse = httpClient.GetAsync(httpClient.BaseAddress + "Meeting/GetAllMeeting").Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                string meetingdata = httpResponse.Content.ReadAsStringAsync().Result;
                meetingEvents = JsonConvert.DeserializeObject<List<MeetingEvent>>(meetingdata);
                foreach (var meeting in meetingEvents)
                {
                    MeetingData meetingData = new MeetingData();
                    meetingData.id = meeting.Id;
                    meetingData.start_date = meeting.StartDate;
                    meetingData.end_date = meeting.EndDate;
                    meetingData.text = meeting.Description;

                    meetingDatas.Add(meetingData);
                }
            }

            return View("Index", meetingDatas);
        }
    }
}
