using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ontap_Net104_319.Models;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        AppDbContext _context;
        public ProductController()
        {
            _context = new AppDbContext();
        }

        // GET: api/<ProductController>
        [HttpGet("get-all")]
        public ActionResult Get()
        {
            var data = _context.Products.ToList();
            return Ok(data);
        }

        // GET api/<ProductController>/5
        [HttpGet("get-by-id")]
        public ActionResult GetById(Guid id)
        {
            var data = _context.Products.Find(id);
            return Ok(data);
        }

        // POST api/<ProductController>
        [HttpPost("create-product")]
        public ActionResult Create(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("update-product")]
        public ActionResult Edit(Product product)
        {
            try
            {
                var item = _context.Products.Find(product.Id);
                item.Name = product.Name;
                item.Description = product.Description;
                item.Price = product.Price;
                item.Amount = product.Amount;
                _context.Products.Update(item);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        // DELETE api/<ProductController>/5
        [HttpDelete("delete-by-id")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var item = _context.Products.Find(id);
                _context.Products.Remove(item);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPut("Add-to-Cart")]
		public IActionResult AddtoCart(string username, Guid id, int quantity)
		{
            try
            {
				// Kiểm tra dữ liệu đăng nhập
				//var check = HttpContext.Session.GetString("username");
				if (String.IsNullOrEmpty(username)) // chưa đăng nhập => bắt đăng nhập
				{
					return BadRequest("Chưa đăng nhập em ơi!!!");
				}
				else
				{
					// Kiểm tra xem tài trong giỏ hàng của user này đã có sản phẩm này hay chưa?
					var cartItem = _context.CartDetails.FirstOrDefault(p => p.ProductId == id && p.Username == username);
					var productItem = _context.Products.FirstOrDefault(p => p.Id == id);
					if (cartItem == null)
					{ // Nếu giỏ hàng của user này chưa có sản phẩm đó
						CartDetails cartDetails = new CartDetails()
						{
							Id = Guid.NewGuid(),
							ProductId = id,
							Quantity = quantity,
							Status = 1,
							Username = username,
						};

						_context.CartDetails.Add(cartDetails); _context.SaveChanges();

					}

					else
					{

						//check số lượng thêm vào giỏ hàng vượt quá số lượng của sản phẩm
						if (cartItem.Quantity + quantity > productItem.Amount)
						{
							//TempData["Invalid"] = $"{cartItem.Product.Name}";
						}
						else
						{
							// Cập nhật số lượng
							cartItem.Quantity = cartItem.Quantity + quantity;
							_context.CartDetails.Update(cartItem);
							_context.SaveChanges();
						}

					}
					return Ok();
				}
			}
            catch (Exception)
            {

                return BadRequest(0);
            }
		}
	}
}
