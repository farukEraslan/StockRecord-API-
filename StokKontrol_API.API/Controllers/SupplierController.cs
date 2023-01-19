using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrol_API.Entities.Entities;
using StokKontrol_API.Service.Abstract;

namespace StokKontrol_API.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<Supplier> _service;

        public SupplierController(IGenericService<Supplier> service)
        {
            _service = service;
        }

        // GET: api/TumTedarikcileriGetir
        [HttpGet]
        public IActionResult TumTedarikcileriGetir()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/AktifTedarikcileriGetir
        [HttpGet]
        public IActionResult AktifTedarikcileriGetir()
        {
            return Ok(_service.GetActive());
        }

        // GET: api/IdyeGoreTedarikcileriGetir/5
        [HttpGet("{id}")]
        public IActionResult IdyeGoreTedarikcileriGetir(int id)
        {
            return Ok(_service.GetById(id));
        }

        // POST: api/supplier/TedarikciEkle
        [HttpPost]
        public IActionResult TedarikciEkle(Supplier supplier)
        {
            _service.Add(supplier);

            return CreatedAtAction("IdyeGoreTedarikcileriGetir", new { id = supplier.Id }, supplier);
        }

        // PUT: api/KategoriGuncelle/5
        [HttpPut("{id}")]
        public IActionResult TedarikciGuncelle(int id, Supplier supplier)
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

        // DELETE: api/TedarikciSil/5
        [HttpDelete("{id}")]
        public IActionResult TedarikciSil(int id)
        {
            var supplier = _service.GetById(id);
            if (supplier == null)
            {
                return NotFound();
            }
            try
            {
                _service.Remove(supplier);
                return Ok("Tedarikçi Silindi");
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

        [HttpGet("{id}")]
        public IActionResult TedarikciAktiflestir(int id)
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
