using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ontap_Net104_319.Models;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BillController : ControllerBase
	{
		AppDbContext _context;
        public BillController()
        {
            _context = new AppDbContext();
        }
        [HttpGet("get-bill")]
		public ActionResult Get(string username)
		{
			var check = _context.Carts.FirstOrDefault(x => x.Username == username);
			if (check == null)
			{
				return BadRequest("chưa đăng nhâp em ơi");
			}
			else
			{
				var billItems = _context.Bills.Include(p => p.Details).Where(p => p.Username == username).ToList();
				return Ok(billItems);
			}
		}

		[HttpPut("Cancel-bill")]
		public IActionResult CancelBill( string username, string id)
		{
			try
			{
				//var check = HttpContext.Session.GetString("username");
				var bill = _context.Bills.Include(b => b.Details).FirstOrDefault(b => b.Id == id && b.Username == username);
				if (bill == null || bill.Status == 100)
				{
					return Ok();
				}
				bill.Status = 100;
				_context.Bills.Update(bill);

				foreach (var item in bill.Details)
				{
					var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
					if (product != null)
					{
						product.Amount += item.Quantity;
						_context.Products.Update(product);
					}
				}

				_context.SaveChanges();

				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPut("repurchase-bill")]
		public IActionResult RepurchaseBill(string username, string id)
		{
			try
			{
				//var check = HttpContext.Session.GetString("username");
				var bill = _context.Bills.Include(b => b.Details).FirstOrDefault(b => b.Id == id && b.Username == username);
				if (bill == null)
				{
					return RedirectToAction("Index", "Bill");
				}
				foreach (var item in bill.Details)
				{
					var billDetails = _context.BillDetails.Include(a => a.Product).FirstOrDefault(a => a.Id == item.Id);
					if (billDetails.Product.Amount < billDetails.Quantity || billDetails.Product.Amount == 0)
					{
						//TempData["Invalid"] = $"{billDetails.Product.Name}";
						return RedirectToAction("Index");
					}
				}
				foreach (var item in bill.Details)
				{
					var cartDetai = _context.CartDetails.FirstOrDefault(a => a.ProductId == item.ProductId);
					if (cartDetai != null)
					{
						cartDetai.Quantity = item.Quantity;
						_context.CartDetails.Update(cartDetai);
						continue;
					}
					var newCart = new CartDetails()
					{
						Username = bill.Username,
						ProductId = item.ProductId,
						Quantity = item.Quantity,
					};
					_context.CartDetails.Add(newCart);
				}
				_context.SaveChanges();
				return Ok();
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}
	}
}
