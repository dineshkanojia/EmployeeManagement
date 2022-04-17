using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerApiController : ControllerBase
    {
        Uri baseUrl;
        HttpClient httpClient;
        private IConfiguration _config;

        public SchedulerApiController(IConfiguration config)
        {
            _config = config;
            baseUrl = new Uri(_config["APIService:Meeting"]);
            httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
        }

        // GET: api/<SchedulerApiController>
        [HttpGet]
        public IEnumerable<MeetingData> Get()
        {
            return GetLatestSchedule();
        }

        // GET api/<SchedulerApiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SchedulerApiController>
        [HttpPost]
        public ObjectResult Post([FromForm] MeetingData MeetingData)
        {
            MeetingEvent meetingEvent = new MeetingEvent();

            meetingEvent.Description = MeetingData.text;
            meetingEvent.EndDate = MeetingData.end_date;
            meetingEvent.StartDate = MeetingData.start_date;
            meetingEvent.UserEmail = "sample.text";

            string data = JsonConvert.SerializeObject(meetingEvent);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = httpClient.PostAsync(httpClient.BaseAddress + "Meeting/AddMeeting", stringContent).Result;
            if (httpResponse.IsSuccessStatusCode)
            {
                return Ok(new
                {
                    action = "inserted"
                });
            }
            return Ok(new
            {
                action = "inserted"
            });
        }

        // PUT api/<SchedulerApiController>/5
        [HttpPut("{id}")]
        public ObjectResult Put(int id, [FromForm] MeetingData MeetingData)
        {
            MeetingEvent meetingEvent = new MeetingEvent();

            meetingEvent.Description = MeetingData.text;
            meetingEvent.EndDate = MeetingData.end_date;
            meetingEvent.StartDate = MeetingData.start_date;
            meetingEvent.UserEmail = "sample.text";
            meetingEvent.Id = id;
            string data = JsonConvert.SerializeObject(meetingEvent);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = httpClient.PostAsync(httpClient.BaseAddress + "Meeting/UpdateMeeting", stringContent).Result;
            if (httpResponse.IsSuccessStatusCode)
            {
                return Ok(new
                {
                    action = "updated"
                });
            }
            return Ok(new
            {
                action = "updated"
            });
        }

        // DELETE api/<SchedulerApiController>/5
        [HttpDelete("{id}")]
        public IEnumerable<MeetingData> Delete(int id)
        {
            List<MeetingData> meetingDatas = new List<MeetingData>();
            HttpResponseMessage response = httpClient.DeleteAsync(httpClient.BaseAddress + "Meeting/DeleteMeeting?Id=" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return GetLatestSchedule();
            }

            return GetLatestSchedule();
        }

        private IEnumerable<MeetingData> GetLatestSchedule()
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
            return meetingDatas;
        }
    }
}
