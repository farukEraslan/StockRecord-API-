using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrol_API.Entities.Entities;
using StokKontrol_API.Service.Abstract;

namespace StokKontrol_API.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<Product> _service;

        public ProductController(IGenericService<Product> service)
        {
            _service = service;
        }

        // GET: api/TumUrunleriGetir
        [HttpGet]
        public IActionResult TumUrunleriGetir()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/AktifUrunleriGetir
        [HttpGet]
        public IActionResult AktifUrunleriGetir()
        {
            return Ok(_service.GetActive());
        }

        // GET: api/IdyeGoreUrunleriGetir/5
        [HttpGet("{id}")]
        public IActionResult IdyeGoreUrunleriGetir(int id)
        {
            return Ok(_service.GetById(id));
        }

        // POST: api/supplier/UrunEkle
        [HttpPost]
        public IActionResult UrunEkle(Product supplier)
        {
            _service.Add(supplier);

            return CreatedAtAction("IdyeGoreUrunleriGetir", new { id = supplier.Id }, supplier);
        }

        // PUT: api/KategoriGuncelle/5
        [HttpPut("{id}")]
        public IActionResult UrunGuncelle(int id, Product supplier)
        {
            if (id != supplier.Id)
            {
                return BadRequest();
            }

            //_service.Entry(supplier).State = EntityState.Modified;

            try
            {
                _service.Update(supplier);
                return Ok(supplier);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!supplierExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // DELETE: api/UrunSil/5
        [HttpDelete("{id}")]
        public IActionResult UrunSil(int id)
        {
            var supplier = _service.GetById(id);
            if (supplier == null)
            {
                return NotFound();
            }
            try
            {
                _service.Remove(supplier);
                return Ok("Ürün Silindi");
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        private bool supplierExists(int id)
        {
            return _service.Any(e => e.Id == id);
        }

        [HttpGet("id")]
        public IActionResult UrunAktifleştir(int id)
        {
            var supplier = _service.GetById(id);
            if (supplier == null)
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
    }
}
