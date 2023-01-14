using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrol_API.Entities.Entities;
using StokKontrol_API.Service.Abstract;

namespace StokKontrol_API.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User> _service;

        public UserController(IGenericService<User> service)
        {
            _service = service;
        }

        // GET: api/TumKullanicileriGetir
        [HttpGet]
        public IActionResult TumKullanicileriGetir()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/AktifKullanicileriGetir
        [HttpGet]
        public IActionResult AktifKullanicileriGetir()
        {
            return Ok(_service.GetActive());
        }

        // GET: api/IdyeGoreKullanicileriGetir/5
        [HttpGet("{id}")]
        public IActionResult IdyeGoreKullanicileriGetir(int id)
        {
            return Ok(_service.GetById(id));
        }

        // POST: api/user/KullaniciEkle
        [HttpPost]
        public IActionResult KullaniciEkle(User user)
        {
            _service.Add(user);

            return CreatedAtAction("IdyeGoreKullanicileriGetir", new { id = user.Id }, user);
        }

        // PUT: api/KullaniciGuncelle/5
        [HttpPut("{id}")]
        public IActionResult KullaniciGuncelle(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            //_service.Entry(user).State = EntityState.Modified;

            try
            {
                _service.Update(user);
                return Ok(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // DELETE: api/KullaniciSil/5
        [HttpDelete("{id}")]
        public IActionResult KullaniciSil(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                _service.Remove(user);
                return Ok("Kullanıcı Silindi");
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        private bool userExists(int id)
        {
            return _service.Any(e => e.Id == id);
        }

        [HttpGet("id")]
        public IActionResult KullaniciAktifleştir(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                _service.Activate(id);
                return Ok(_service.GetById(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Login(string email, string parola)
        {
            if (_service.Any(x=>x.Email == email && x.Password == parola))
            {
                User logged = _service.GetByDefault(x => x.Email == email && x.Password == parola);
                return Ok(logged);
            }

            return NotFound();
        }
    }
}
