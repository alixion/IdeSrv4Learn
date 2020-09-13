using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdeSrv4Learn.WebMvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace IdeSrv4Learn.WebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult Secured()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
        
        [Authorize]
        public async Task<IActionResult> Weather()
        {
            
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var content = await client.GetStringAsync("https://localhost:6001/identity");

                ViewBag.Json = JArray.Parse(content).ToString();
            }
            catch (Exception e)
            {
                ViewBag.Json = e.Message+"<br>"+accessToken;
            }
            
            return View();
        }
        
        
        public async Task<IActionResult> CallApiAsUser()
        {
            var client = _httpClientFactory.CreateClient("user_client");

            var response = await client.GetStringAsync("identity");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View("CallApi");
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> CallApiAsClient()
        {
            var client = _httpClientFactory.CreateClient("client");

            var response = await client.GetStringAsync("identity");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View("CallApi");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}