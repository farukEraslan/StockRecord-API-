using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StokKontrol_API.Entities.Entities;
using System.Text;

namespace StokKontrol_API.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        string uri = "https://localhost:7150";
        public async Task<IActionResult> Index()
        {
            List<Order> siparisler = new List<Order>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/TumSiparisleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    siparisler = JsonConvert.DeserializeObject<List<Order>>(apiCevap);
                }
            }
            return View(siparisler);
        }

        //[HttpDelete]
        public async Task<IActionResult> SiparisSil(int id)
        {
            //List<Order> siparisler = new List<Order>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Order/SiparisSil/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //siparisler = JsonConvert.DeserializeObject<List<Order>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> SiparisOnayla(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/SiparisOnayla/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> SiparisReddet(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/SiparisReddet/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> SiparisDetayiGor(int id)
        {
            Order siparis = new Order();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/IdyeGoreSiparisleriGetir/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
    }
}
