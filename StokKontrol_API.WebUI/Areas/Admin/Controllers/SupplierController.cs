using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrol_API.Entities.Entities;
using System.Text;

namespace StokKontrol_API.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        string uri = "https://localhost:7150";
        public async Task<IActionResult> Index()
        {
            List<Supplier> tedarikciler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TumTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            return View(tedarikciler);
        }

        //[HttpDelete]
        public async Task<IActionResult> TedarikciSil(int id)
        {
            //List<Supplier> kategoriler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Supplier/TedarikciSil/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> TedarikciAktiflestir(int id)
        {
            //List<Supplier> kategoriler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TedarikciAktiflestir/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult TedarikciEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TedarikciEkle(Supplier supplier)
        {
            supplier.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/Supplier/TedarikciEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        static Supplier updatedSupplier;   // İlgili kategoriyi güncelleme işleminin devamındaki (yani OUT işlemindeki) kullanacağımız için o metottan da ulaşabilmek 

        [HttpGet]
        public async Task<IActionResult> TedarikciGuncelle(int id)   // id ile ilgili kategoriyi bul getir.
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/IdyeGoreTedarikcileriGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedSupplier = JsonConvert.DeserializeObject<Supplier>(apiCevap);
                }
            }
            return View(updatedSupplier);
        }

        [HttpPost]
        public async Task<IActionResult> TedarikciGuncelle(Supplier guncelTedarikci)   // Güncellenmiş kategori parametre olarak alınır.
        {
            using (var httpClient = new HttpClient())
            {
                guncelTedarikci.AddedDate = updatedSupplier.AddedDate;
                guncelTedarikci.IsActive = true;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelTedarikci), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PutAsync($"{uri}/api/Supplier/TedarikciGuncelle/{guncelTedarikci.Id}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
                //return View(updatedSupplier);
                return RedirectToAction("Index");
            }
        }
    }
}
