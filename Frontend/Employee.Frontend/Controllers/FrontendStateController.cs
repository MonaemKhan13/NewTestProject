using Employee.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Employee.Frontend.Controllers
{
    public class FrontendStateController : Controller
    {
        private readonly HttpClient _httpClient;

        public FrontendStateController(IHttpClientFactory httpClient) => _httpClient = httpClient.CreateClient("EmployeeAPIBase");

        public async Task<IActionResult> Index()
        {
            var data = await GetAllState();
            return View(data);
        }
        private async Task<IEnumerable<States>> GetAllState()
        {
            var responce = await _httpClient.GetAsync("State");
            if (responce.IsSuccessStatusCode)
            {
                var content = await responce.Content.ReadAsStringAsync();
                var countries = JsonConvert.DeserializeObject<IEnumerable<States>>(content);
                return countries!;
            }
            return new List<States>();
        }
    }
}
