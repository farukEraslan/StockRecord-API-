using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrol_API.Entities.Entities;
using StokKontrol_API.Repositories.Context;
using StokKontrol_API.Service.Abstract;

namespace StokKontrol_API.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericService<Category> _service;

        public CategoryController(IGenericService<Category> service)
        {
            _service = service;
        }

        // GET: api/TumKategorileriGetir
        [HttpGet]
        public IActionResult TumKategorileriGetir()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/AktifKategorileriGetir/5
        [HttpGet]
        public IActionResult AktifKategorileriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreKategorileriGetir(int id)
        {
            return Ok(_service.GetById(id));
        }

        // POST: api/category/KategoriEkle
        [HttpPost]
        public IActionResult KategoriEkle(Category category)
        {
            _service.Add(category);

            return CreatedAtAction("IdyeGoreKategorileriGetir", new { id = category.Id }, category);
        }

        // PUT: api/KategoriGuncelle/5
        [HttpPut("{id}")]
        public IActionResult KategoriGuncelle(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            //_service.Entry(category).State = EntityState.Modified;

            try
            {
                _service.Update(category);
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!categoryExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // DELETE: api/category/5
        [HttpDelete("{id}")]
        public IActionResult KategoriSil(int id)
        {
            var category = _service.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _service.Remove(category);
                return Ok("Kategori Silindi");
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
        }

        private bool categoryExists(int id)
        {
            return _service.Any(e => e.Id == id);
        }

        [HttpGet("{id}")]
        public IActionResult KategoriAktiflestir(int id)
        {
            var category = _service.GetById(id);
            if (category == null)
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
