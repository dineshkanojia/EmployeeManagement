using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class LoginController : Controller
    {
        private IConfiguration _config;
        Uri baseUrl;
        HttpClient httpClient;
        public LoginController(IConfiguration config)
        {
            _config = config;
            baseUrl = new Uri(_config["APIService:Employee"]);
            httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }
        //[HttpGet("login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmployeeLogin(string returnURL)
        {
            ViewData["ReturnUrl"] = returnURL;
            return View();
        }

        [HttpPost("EmployeeLogin")]
        public async Task<ActionResult> ValidateLogin(string Username, string Password, string returnURL)
        {
            ViewData["ReturnUrl"] = returnURL;
            TempData["userEmail"] = Username;
            EmployeeLogin employeeLogin = new EmployeeLogin();
            employeeLogin.Email = Username;
            employeeLogin.Password = Password;

            string data = JsonConvert.SerializeObject(employeeLogin);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = httpClient.PostAsync(httpClient.BaseAddress + "Login", stringContent).Result;
            if (httpResponse.IsSuccessStatusCode)
            {
                var token = httpResponse.Content.ReadAsStringAsync().Result;
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpContext.Session.SetString("token", "Bearer " + token);
                httpResponse = httpClient.GetAsync(httpClient.BaseAddress + "Employee/GetAllEmployeeByEmail/?UserEmail=" + Username).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    EmployeeDetails employeeDetails = new EmployeeDetails();
                    string userdata = httpResponse.Content.ReadAsStringAsync().Result;

                    employeeDetails = JsonConvert.DeserializeObject<EmployeeDetails>(userdata);

                    var roleName = employeeDetails.RoleMapping.FirstOrDefault(r => r.UserId == employeeDetails.Id).Roles.RoleName;


                    HttpContext.Session.SetString("userdetails", userdata);

                    var claims = new List<Claim>();
                    claims.Add(new Claim("username", employeeDetails.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, employeeDetails.UserName));
                    claims.Add(new Claim(ClaimTypes.Name, employeeDetails.FirstName + employeeDetails.LastName));
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return Redirect("Employee/index");
                }
                TempData["error"] = "User not found.";
                return View("EmployeeLogin");
            }
            TempData["error"] = "Incorrect email address or password.";
            return View("EmployeeLogin");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return View("EmployeeLogin");
        }
    }
}
