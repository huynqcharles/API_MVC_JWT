using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using System.Text;

namespace Client.Controllers
{
    public class JWTsController : Controller
    {
        private readonly HttpClient _httpClient;
        public JWTsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7083/api/JWTs/login", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenResponse>(result).Token;
                HttpContext.Session.SetString("JWT", token);
                return RedirectToAction("Index", "Products");
            }
            return View(loginModel);
        }
    }
}
