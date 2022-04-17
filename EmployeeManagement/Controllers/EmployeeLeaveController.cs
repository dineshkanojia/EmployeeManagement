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
    public class EmployeeLeaveController : Controller
    {
        Uri baseUrl;
        HttpClient httpClient;
        
        private IConfiguration _config;


        public EmployeeLeaveController(IConfiguration config)
        {
            _config = config;
            baseUrl = new Uri(_config["APIService:EmployeeLeave"]);
            httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
        }
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("token");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage httpResponse = httpClient.GetAsync(httpClient.BaseAddress + "LeaveRequest/GetAllRequests").Result;
            List<EmployeeLeave> leaveRequets = new List<EmployeeLeave>();
            if (httpResponse.IsSuccessStatusCode)
            {

                string leaveData = httpResponse.Content.ReadAsStringAsync().Result;
                
                leaveRequets = JsonConvert.DeserializeObject<List<EmployeeLeave>>(leaveData);
               
                    foreach (EmployeeLeave leaveRequest in leaveRequets)
                {
                    if (leaveRequest.IsApproved == false || leaveRequest.IsApproved == null)
                    {
                        leaveRequest.Approved_By = string.Empty;
                    }
                    else
                    {
                        HttpResponseMessage httpempResponse = httpClient.GetAsync(httpClient.BaseAddress + "Employee/GetAllEmployee").Result;
                        List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            string userdata = httpResponse.Content.ReadAsStringAsync().Result;
                            employeeDetails = JsonConvert.DeserializeObject<List<EmployeeDetails>>(userdata);
                            var user = employeeDetails.FirstOrDefault(u => u.Id == leaveRequest.ApprovedBy);

                            leaveRequest.Approved_By = user.FirstName + " " + user.LastName;
                        }
                    }
                    leaveRequest.LeaveType = GetValueFromEnum(Convert.ToInt32(leaveRequest.LeaveType));

                }
            }
           
            return View(leaveRequets);
        }
        public IActionResult ApplyLeave()
        {
            TempData["userEmail"] = "admin@gmail.com";
            return View(new EmployeeLeave());
        }

        [HttpPost]
        public IActionResult ApplyLeave(EmployeeLeave leaveRequest)
        {
            var user = HttpContext.Session.GetString("userdetails");

            var userDetails = JsonConvert.DeserializeObject<EmployeeLogin>(user);

            string data = JsonConvert.SerializeObject(userDetails);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = httpClient.PostAsync("https://localhost:44390/api/" + "Login", stringContent).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                var email = "admin@gmail.com";
                var token = httpResponse.Content.ReadAsStringAsync().Result;
                httpResponse = httpClient.GetAsync("https://localhost:44390/api" + "Employee/GetAllEmployeeByEmail/?UserEmail=" + email).Result;
               
                if (httpResponse.IsSuccessStatusCode)
                {
                    EmployeeDetails employee = new EmployeeDetails();
                    string empData = httpResponse.Content.ReadAsStringAsync().Result;
                    employee = JsonConvert.DeserializeObject<EmployeeDetails>(empData);
                    var roleName = employee.RoleMapping.FirstOrDefault(r => r.UserId == employee.Id).Roles.RoleName;
                   
                    leaveRequest.EmployeeId = employee.Id;
                    leaveRequest.EmployeeName = employee.FirstName;
                    leaveRequest.LeaveType = LeaveTypeEnum.SickLeave.ToString();
                    leaveRequest.RequestDate = DateTime.Now;
                    string leaveData = JsonConvert.SerializeObject(leaveRequest);
                    StringContent strContent = new StringContent(leaveData, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress + "LeaveRequest/CreateLeaveRequest", strContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["sucessleavemessage"] = "Leave Request Submitted sucessfully.";
                        TempData["userEmail"] = email;
                        TempData["role"] = roleName;
                        return RedirectToAction("index");
                    }
                }
            }

            return View();

        }

        public IActionResult EditLeaveRequest(int id)
        {
            HttpResponseMessage httpResponse = httpClient.GetAsync(httpClient.BaseAddress + "LeaveRequest/GetAllRequests").Result;
            List<EmployeeLeave> employeeLeaves = new List<EmployeeLeave>();
            if (httpResponse.IsSuccessStatusCode)
            {
                string leaveData = httpResponse.Content.ReadAsStringAsync().Result;
                employeeLeaves = JsonConvert.DeserializeObject<List<EmployeeLeave>>(leaveData);
                var user = employeeLeaves.FirstOrDefault(u => u.Id == id);
                return View(user);
            }
            return View();
        }

        [HttpPost]
        public IActionResult EditLeaveRequest(EmployeeLeave employeeLeave)
        {
            HttpResponseMessage httpResponse = httpClient.GetAsync(httpClient.BaseAddress + "LeaveRequest/GetAllRequests").Result;
            List<EmployeeLeave> leaveRequets = new List<EmployeeLeave>();

            if (httpResponse.IsSuccessStatusCode)
            {
                string userdata = httpResponse.Content.ReadAsStringAsync().Result;
                leaveRequets = JsonConvert.DeserializeObject<List<EmployeeLeave>>(userdata);
                var leave = leaveRequets.FirstOrDefault(u => u.Id == employeeLeave.Id);
                leave.EmployeeId = leave.EmployeeId;
                leave.EmployeeName = leave.EmployeeName;
                leave.StartDate = employeeLeave.StartDate;
                leave.EndDate = employeeLeave.EndDate;
                leave.LeaveType = employeeLeave.LeaveType;
                leave.Comments = employeeLeave.Comments;

                string leaveData = JsonConvert.SerializeObject(leave);
                StringContent strContent = new StringContent(leaveData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress + "LeaveRequest/UpdateLeaveRequest", strContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["sucessleavemessage"] = "Leave Request Updated sucessfully.";
                    return RedirectToAction("index");
                }
            }
            return View();
        }

        public IActionResult CancelLeaveRequest (int id)
        {

            HttpResponseMessage response = httpClient.DeleteAsync(httpClient.BaseAddress + "LeaveRequest/DeleteRequest?Id=" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["sucessmessage"] = "Leave Request cancelled sucessfully.";
            }

            return RedirectToAction("index");

        }

        public IActionResult ApproveRequest(int Id, int approvedBy)
        {
            try
            {
                HttpResponseMessage httpResponse = httpClient.GetAsync(httpClient.BaseAddress + "LeaveRequest/GetRequestById?Id=" + Id).Result;
                EmployeeLeave employeeLeave = new EmployeeLeave();
                if (httpResponse.IsSuccessStatusCode)
                {
                    string leaveData = httpResponse.Content.ReadAsStringAsync().Result;
                    employeeLeave = JsonConvert.DeserializeObject<EmployeeLeave>(leaveData);
                    employeeLeave.ApprovedBy = approvedBy;
                    employeeLeave.ApprovedDate = DateTime.Now.Date;
                    string leaveRequest = JsonConvert.SerializeObject(employeeLeave);
                    StringContent strContent = new StringContent(leaveRequest, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress + "LeaveRequest/UpdateLeaveRequest", strContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["sucessmessage"] = "Leave Request Approved sucessfully.";
                        return RedirectToAction("index");
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error Occured while Approving!.";
            }
            return View();
        }

        public IActionResult RejectRequest(int Id, int rejectedBy)
        {
            try
            {
                HttpResponseMessage httpResponse = httpClient.GetAsync(httpClient.BaseAddress + "LeaveRequest/GetRequestById?Id=" + Id).Result;
                EmployeeLeave employeeLeave = new EmployeeLeave();
                if (httpResponse.IsSuccessStatusCode)
                {
                    string leaveData = httpResponse.Content.ReadAsStringAsync().Result;
                    employeeLeave = JsonConvert.DeserializeObject<EmployeeLeave>(leaveData);
                    employeeLeave.ApprovedBy = rejectedBy;
                    employeeLeave.ApprovedDate = DateTime.Now.Date;
                    string leaveRequest = JsonConvert.SerializeObject(employeeLeave);
                    StringContent strContent = new StringContent(leaveRequest, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress + "LeaveRequest/UpdateLeaveRequest", strContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["sucessmessage"] = "Leave Request Rejected sucessfully.";
                        return RedirectToAction("index");
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error Occured while Rejecting!.";
            }
            return View();
        }

        public string GetValueFromEnum(int id)
        {
            var enumDisplayStatus = (LeaveTypeEnum)id;
            string stringValue = enumDisplayStatus.ToString();
            return stringValue;
        }
    }
}
