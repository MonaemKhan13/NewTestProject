using Employee.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Employee.Frontend.Controllers
{
    public class FrontendCountryController : Controller
    {
        private readonly HttpClient _httpClient;

        public FrontendCountryController(IHttpClientFactory httpClient) => _httpClient = httpClient.CreateClient("EmployeeAPIBase");

        public async Task<IActionResult> Index() => View(await GetAllCountry());

        public async Task<List<Countrys>> GetAllCountry()
        {
            var responce = await _httpClient.GetFromJsonAsync<List<Countrys>>("Country");
            return responce is not null ? responce : new List<Countrys>();
        }

        [HttpGet]
        public async Task<IActionResult> AddorEdit(int Id)
        {
            if (Id == 0)
            {
                //create From
                ViewBag.SubmitName = "Create";
                return View(new Countrys());
            }
            else
            {
                var data = await _httpClient.GetAsync("Country/Id:int?Id=" + Id);
                if (data.IsSuccessStatusCode)
                {
                    var result = await data.Content.ReadFromJsonAsync<Countrys>();
                    ViewBag.SubmitName = "Save";
                    return View(result);
                }
            }

            return View(new Countrys());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddorEdit(int Id, Countrys con)
        {
            if (ModelState.IsValid)
            {
                if(Id == 0)
                {
                    var response = await _httpClient.PostAsJsonAsync("Country", con);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error creating Country.");
                        ViewBag.SubmitName = "Create";
                        return View(con);
                    }
                }
                else
                {
                    var response = await _httpClient.PutAsJsonAsync($"Country?Id={Id}", con);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Update Country UnSuccsfull.");
                        ViewBag.SubmitName = "Save";
                        return View(con);
                    }
                }
            }
            else
            {
                return NoContent();
            }            
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var responce = await _httpClient.DeleteAsync($"Country?Id={Id}");
            if(responce.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
