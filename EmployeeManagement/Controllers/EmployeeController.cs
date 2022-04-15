using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        Uri baseUrl;
        HttpClient httpClient;
        private IConfiguration _config;
        public EmployeeController(IConfiguration config)
        {
            _config = config;
            baseUrl = new Uri(_config["APIService:Employee"]);
            httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
        }

        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("token");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage httpResponse = httpClient.GetAsync(httpClient.BaseAddress + "Employee/GetAllEmployee").Result;
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            if (httpResponse.IsSuccessStatusCode)
            {

                string userdata = httpResponse.Content.ReadAsStringAsync().Result;
                employeeDetails = JsonConvert.DeserializeObject<List<EmployeeDetails>>(userdata);
            }

            return View(employeeDetails);
        }

        public IActionResult AddEmployee()
        {
            return View(new EmployeeDetails());
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeDetails employeeDetails)
        {
            var user = HttpContext.Session.GetString("userdetails");

            var userDetails = JsonConvert.DeserializeObject<EmployeeLogin>(user);

            string data = JsonConvert.SerializeObject(userDetails);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = httpClient.PostAsync(httpClient.BaseAddress + "Login", stringContent).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                var token = httpResponse.Content.ReadAsStringAsync().Result;
                employeeDetails.IsDelete = false;
                employeeDetails.Status = 1;
                //employeeDetails.Photo = "0";
                string employeedata = JsonConvert.SerializeObject(employeeDetails);
                StringContent strContent = new StringContent(employeedata, Encoding.UTF8, "application/json");
                //var token = HttpContext.Session.GetString("token");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress + "Employee/AddEmployee", strContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["sucessmessage"] = "Employee Added sucessfully.";
                    return RedirectToAction("index");
                }
            }

            return View();

        }

        public IActionResult EditEmployee(int id)
        {
            HttpResponseMessage httpResponse = httpClient.GetAsync(httpClient.BaseAddress + "Employee/GetAllEmployee").Result;
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            if (httpResponse.IsSuccessStatusCode)
            {
                string userdata = httpResponse.Content.ReadAsStringAsync().Result;
                employeeDetails = JsonConvert.DeserializeObject<List<EmployeeDetails>>(userdata);
                var user = employeeDetails.FirstOrDefault(u => u.Id == id);
                return View(user);
            }
            return View();
        }

        [HttpPost]
        public IActionResult EditEmployee(EmployeeDetails employeeDetail)
        {
            HttpResponseMessage httpResponse = httpClient.GetAsync(httpClient.BaseAddress + "Employee/GetAllEmployee").Result;
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();

            if (httpResponse.IsSuccessStatusCode)
            {
                string userdata = httpResponse.Content.ReadAsStringAsync().Result;
                employeeDetails = JsonConvert.DeserializeObject<List<EmployeeDetails>>(userdata);
                var user = employeeDetails.FirstOrDefault(u => u.Id == employeeDetail.Id);
                employeeDetail.Photo = user.Photo;
                employeeDetail.Password = user.Password;
                employeeDetail.UserName = user.UserName;

                string employeedata = JsonConvert.SerializeObject(employeeDetail);
                StringContent strContent = new StringContent(employeedata, Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress + "Employee/UpdateEmployee", strContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["sucessmessage"] = "Employee Updated sucessfully.";
                    return RedirectToAction("index");
                }
            }
            return View();
        }

        public IActionResult DeleteEmployee(int id)
        {

            HttpResponseMessage response = httpClient.DeleteAsync(httpClient.BaseAddress + "Employee/DeleteEmployee?Id=" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["sucessmessage"] = "Employee deleted sucessfully.";
            }

            return RedirectToAction("index");

        }
    }
}
