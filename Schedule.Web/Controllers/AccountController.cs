using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Schedule.Entities;
using Schedule.Web.ViewModels;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Web.Controllers
{
    public class AccountController : Controller
    {
        string baseURL = "http://localhost:60340/";
       
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = false;
                var client = new HttpClient
                {
                    BaseAddress = new Uri(baseURL)
                };
                
                client.DefaultRequestHeaders.Clear();

                StringContent content = new StringContent(JsonConvert.SerializeObject(new Usuario()
                {
                    Username = model.Username,
                    Password = model.Password
                }), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/Account/Login", content);

                //Checking the response is successful or not which is sent using HttpClient  
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var userResponse = response.Content.ReadAsStringAsync().Result;

                    result = Convert.ToBoolean(userResponse);

                    if (result) return RedirectToAction("Index", "Home");
                }
            }
            return View("Index",model);
        }
    }
}
