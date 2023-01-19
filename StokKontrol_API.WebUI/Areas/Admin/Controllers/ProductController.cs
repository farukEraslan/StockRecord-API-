using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrol_API.Entities.Entities;
using System.Text;

namespace StokKontrol_API.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        string uri = "https://localhost:7150";
        public async Task<IActionResult> Index()
        {
            List<Product> urunler = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/TumUrunleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            return View(urunler);
        }

        //[HttpDelete]
        public async Task<IActionResult> UrunSil(int id)
        {
            //List<Product> urunler = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Product/UrunSil/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UrunAktiflestir(int id)
        {
            //List<Product> urunler = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/UrunAktiflestir/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UrunEkle()
        {
            List<Category> aktifKategoriler = new List<Category>();
            List<Supplier> aktifTedarikciler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifKategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/AktifTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifTedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }

            ViewBag.AktifKategoriler = aktifKategoriler;
            ViewBag.AktifTedarikciler = aktifTedarikciler;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UrunEkle(Product product)
        {
            product.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/Product/UrunEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))

                }
            }
            return RedirectToAction("Index");
        }

        static Product updatedProduct;   // İlgili urun güncelleme işleminin devamındaki (yani OUT işlemindeki) kullanacağımız için o metottan da ulaşabilmek. 

        [HttpGet]
        public async Task<IActionResult> UrunGuncelle(int id)   // id ile ilgili urunyi bul getir.
        {
            int categoryId = 0;
            int supplierId = 0;
            string kategoriAdi;
            string tedarikciAdi;

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/IdyeGoreUrunleriGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedProduct = JsonConvert.DeserializeObject<Product>(apiCevap);
                }
                categoryId = updatedProduct.CategoryId;
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/IdyeGoreKategorileriGetir/{categoryId}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    var kategori = JsonConvert.DeserializeObject<Category>(apiCevap);
                    kategoriAdi = kategori.CategoryName;
                }
                supplierId = updatedProduct.SupplierId;
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/IdyeGoreTedarikcileriGetir/{supplierId}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    var tedarikci = JsonConvert.DeserializeObject<Supplier>(apiCevap);
                    tedarikciAdi = tedarikci.SupplierName;
                }

            }
            return View(updatedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> UrunGuncelle(Product guncelUrun)   // Güncellenmiş urun parametre olarak alınır.
        {
            using (var httpClient = new HttpClient())
            {
                guncelUrun.AddedDate = updatedProduct.AddedDate;
                guncelUrun.IsActive = true;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelUrun), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PutAsync($"{uri}/api/Product/UrunGuncelle/{guncelUrun.Id}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
                //return View(updatedProduct);
                return RedirectToAction("Index");
            }
        }
    }
}
