using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrol_API.Entities.Entities;
using System.Text;

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

        //[HttpDelete]
        public async Task<IActionResult> KategoriSil(int id)
        {
            //List<Category> kategoriler = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Category/KategoriSil/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> KategoriAktiflestir(int id)
        {
            //List<Category> kategoriler = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/KategoriAktiflestir/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> KategoriEkle(Category category)
        {
            category.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/Category/KategoriEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        static Category updatedCategory;   // İlgili kategoriyi güncelleme işleminin devamındaki (yani OUT işlemindeki) kullanacağımız için o metottan da ulaşabilmek 

        [HttpGet]
        public async Task<IActionResult> KategoriGuncelle(int id)   // id ile ilgili kategoriyi bul getir.
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/IdyeGoreKategorileriGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedCategory = JsonConvert.DeserializeObject<Category>(apiCevap);
                }
            }
            return View(updatedCategory);
        }

        [HttpPost]
        public async Task<IActionResult> KategoriGuncelle(Category guncelKategori)   // Güncellenmiş kategori parametre olarak alınır.
        {
            using (var httpClient = new HttpClient())
            {
                guncelKategori.AddedDate = updatedCategory.AddedDate;
                guncelKategori.IsActive = true;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelKategori), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PutAsync($"{uri}/api/Category/KategoriGuncelle/{guncelKategori.Id}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
                //return View(updatedCategory);
                return RedirectToAction("Index");
            }
        }
    }
}
