using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StokKontrol_API.Entities.Entities;
using StokKontrol_API.Entities.Enums;
using StokKontrol_API.Service.Abstract;
using System.Linq;

namespace StokKontrol_API.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderDetails> _orderDetailService;
        private readonly IGenericService<Product> _productService;
        private readonly IGenericService<User> _userService;

        public OrderController(IGenericService<Order> orderService, IGenericService<OrderDetails> orderDetailService, IGenericService<Product> productService, IGenericService<User> userService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _productService = productService;
            _userService = userService;
        }

        // GET: api/TumSiparisleriGetir
        [HttpGet]
        public IActionResult TumSiparisleriGetir()
        {
            return Ok(_orderService.GetAll());
        }

        // GET: api/AktifSiparisleriGetir
        [HttpGet]
        public IActionResult AktifSiparisleriGetir()
        {
            return Ok(_orderService.GetActive());
        }

        // GET: api/IdyeGoreSiparisleriGetir/5
        [HttpGet("{id}")]
        public IActionResult IdyeGoreSiparisleriGetir(int id)
        {
            return Ok(_orderService.GetById(id));
        }

        // GET: api/BekleyenSiparisleriGetir/5
        [HttpGet]
        public IActionResult BekleyenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x=>x.Status == Status.Pending));
        }

        // GET: api/OnaylanmısSiparisleriGetir/5
        [HttpGet]
        public IActionResult OnaylanmisSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Confirmed));
        }

        // GET: api/IptalEdilenSiparisleriGetir/5
        [HttpGet]
        public IActionResult IptalEdilenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Cancelled));
        }

        [HttpPost]
        public IActionResult SiparisEkle(int userId, [FromQuery] int[] productId, [FromQuery] short[] quantity)
        {              
            Order newOrder = new Order();
            newOrder.UserId = userId;
            newOrder.Status = Status.Pending;
            _orderService.Add(newOrder);

            OrderDetails orderDetail = new OrderDetails();
            for (int i = 0; i < productId.Length; i++)
            {
                var orderedProduct = _productService.GetById(productId[i]);

                orderDetail.OrderId = newOrder.Id;
                orderDetail.ProductId = orderedProduct.Id;
                orderDetail.Quantity = quantity[i];
                orderDetail.UnitPrice = orderedProduct.UnitPrice;
                _orderDetailService.Add(orderDetail);
                
            }

            return Ok(orderDetail);
        }

        [HttpGet]
        public IActionResult SiparisOnayla(int siparisId)
        {
            Order confirmedOrder = _orderService.GetById(siparisId);
            if (confirmedOrder == null)
            {
                return NotFound();
            }
            else
            {
                confirmedOrder.Status = Status.Confirmed;
                confirmedOrder.IsActive = false;
                _orderService.Update(confirmedOrder);
                List<OrderDetails> detaylar = _orderDetailService.GetDefault(x => x.OrderId == confirmedOrder.Id);
                foreach (var item in detaylar)
                {
                    Product productInOrder = _productService.GetById(item.ProductId);
                    productInOrder.Stock -= item.Quantity;
                    _productService.Update(productInOrder);
                }
                return Ok(confirmedOrder);
            }
        }

        [HttpGet]
        public IActionResult SiparisReddet(int siparisId)
        {
            Order cancelledOrder = _orderService.GetById(siparisId);
            if (cancelledOrder == null)
            {
                return NotFound();
            }
            else
            {
                cancelledOrder.Status = Status.Cancelled;
                cancelledOrder.IsActive = false;
                _orderService.Update(cancelledOrder);
                return Ok(cancelledOrder);
            }
        }

        // DELETE: api/SiparisSil/5
        [HttpDelete]
        public IActionResult SiparisSil(int id)
        {
            var siparis = _orderService.GetById(id);
            if (siparis == null)
            {
                return NotFound();
            }
            try
            {
                _orderService.Remove(siparis);
                return Ok("Siparis Silindi");
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        private bool orderExists(int id)
        {
            return _orderService.Any(e => e.Id == id);
        }
    }
}
