using Employee.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Employee.Frontend.Controllers
{
    public class FrontendEmployeeController : Controller
    {
        private readonly HttpClient _httpClient;

        public FrontendEmployeeController(IHttpClientFactory httpClient) => _httpClient = httpClient.CreateClient("EmployeeAPIBase");

        public async Task<IActionResult> Index() => View(await GetAllCountry());

        public async Task<List<Employeess>> GetAllCountry()
        {
            var responce = await _httpClient.GetFromJsonAsync<List<Employeess>>("Employee");
            return responce is not null? responce: new List<Employeess>();
        }
    }
}
