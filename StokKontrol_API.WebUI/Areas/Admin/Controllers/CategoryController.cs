using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrol_API.Entities.Entities;

namespace StokKontrol_API.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        string uri = "https://localhost:7150";
        public async Task<IActionResult> Index()
        {
            List<Category> kategoriler = new List<Category>();
            using (var httpClient = new HttpClient()) 
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/TumKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return View(kategoriler);
        }
    }
}
