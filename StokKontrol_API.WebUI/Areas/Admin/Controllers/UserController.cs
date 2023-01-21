using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrol_API.Entities.Entities;
using System.Text;

[Area("Admin")]
public class UserController : Controller
{
    string uri = "https://localhost:7150";
    public async Task<IActionResult> Index()
    {
        List<User> kullanicilar = new List<User>();
        using (var httpClient = new HttpClient())
        {
            using (var cevap = await httpClient.GetAsync($"{uri}/api/User/TumKullanicilariGetir"))
            {
                string apiCevap = await cevap.Content.ReadAsStringAsync();
                kullanicilar = JsonConvert.DeserializeObject<List<User>>(apiCevap);
            }
        }
        return View(kullanicilar);
    }

    [HttpGet]
    public async Task<IActionResult> KullaniciAktiflestir(int id)
    {
        using (var httpClient = new HttpClient())
        {
            using (var cevap = await httpClient.GetAsync($"{uri}/api/User/KullaniciAktiflestir/{id}"))
            {

            }
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> KullaniciSil(int id)
    {
        using (var httpClient = new HttpClient())
        {
            using (var cevap = await httpClient.DeleteAsync($"{uri}/api/User/KullaniciSil/{id}"))
            {

            }
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult KullaniciEkle()
    {
        return View(); // Sadece ekleme view ını gösterecek
    }

    [HttpPost]
    public async Task<IActionResult> KullaniciEkle(User user)
    {
        user.IsActive = true;
        using (var httpClient = new HttpClient())
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            using (var cevap = await httpClient.PostAsync($"{uri}/api/User/KullaniciEkle", content))
            {
                string apiCevap = await cevap.Content.ReadAsStringAsync();
                //kategoriler = JsonConvert.DeserializeObject<List<User>>(apiCevap);
            }
        }
        return RedirectToAction("Index");
    }


}  