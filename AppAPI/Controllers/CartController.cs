using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ontap_Net104_319.Models;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]


	public class CartController : ControllerBase
	{
		AppDbContext _context;
		public CartController()
		{
			_context = new AppDbContext();
		}
		[HttpGet("get-cart")]
		public IActionResult Get(string username)
		{
			// Check xem đã đăng nhập chưa
			//var check = HttpContext.Session.GetString("username");
			//if (String.IsNullOrEmpty(check)) // chưa đăng nhập => bắt đăng nhập
			//{
			//	return BadRequest("chưa đăng nhâp em ơi");
			//}
			//else
			//{
			//	var cartItems = _context.CartDetails.Where(p => p.Username == check).ToList();
			//	return Ok(cartItems); // truyền dữ liệu lấy được sang bên view
			//}
			var check = _context.Carts.FirstOrDefault(x => x.Username == username);
			if (check == null)
			{
				return BadRequest("chưa đăng nhâp em ơi");
			}
			else
			{
				var cartItems = _context.CartDetails.Where(p => p.Username == username).ToList();
				//Console.WriteLine(cartItems.Count);
				return Ok(cartItems);
				
			}
		}

		//[HttpPost("pay")]
		//public IActionResult Pay(string username)
		//{
		//	//var check = HttpContext.Session.GetString("username");
		//	var data = _context.CartDetails.Include(a => a.Product).Include(a => a.Cart).Where(a => a.Cart.Username == username).ToList();
		//	if (username == null)
		//	{
		//		return BadRequest("chưa đăng nhâp em ơi");
		//	}
		//	else
		//	{
		//		decimal total = 0;

		//		Bill bills = new Bill()
		//		{
		//			Id = Guid.NewGuid().ToString(),
		//			CreateDate = DateTime.Now,
		//			Status = 1,
		//			Username = username,
		//		};
		//		List<BillDetails> lstBill = new List<BillDetails>();
		//		foreach (var item in data)
		//		{
		//			var cartDetails = _context.CartDetails.Include(a => a.Product).FirstOrDefault(a => a.Id == item.Id);
		//			if (cartDetails.Product.Amount < cartDetails.Quantity || cartDetails.Product.Amount == 0)
		//			{
		//				return BadRequest("Số lượng sản phẩm nike hiện tại không đủ , vui lòng chỉnh lại số lượng !");

		//			}
		//		}
		//		foreach (var item in data)
		//		{
		//			BillDetails billDetails = new BillDetails()
		//			{
		//				Id = Guid.NewGuid(),
		//				ProductId = item.ProductId,
		//				BillId = bills.Id,
		//				ProductPrice = item.Product.Price,
		//				Quantity = item.Quantity,
		//				Status = 1,
		//			};
		//			total += billDetails.ProductPrice * billDetails.Quantity;

		//			lstBill.Add(billDetails);
		//			item.Product.Amount -= billDetails.Quantity;
		//			_context.Products.Update(item.Product);
		//		}
		//		_context.BillDetails.AddRange(lstBill);
		//		bills.Money = total;
		//		_context.CartDetails.RemoveRange(data);

		//		_context.Bills.Add(bills); _context.SaveChanges();

		//	}
		//	return Ok();
		//}

		[HttpDelete("delete-by-id")]
		public IActionResult Delete(Guid id)
		{
			try
			{
				var delete = _context.CartDetails.FirstOrDefault(a => a.Id == id);
				if (delete != null)
				{
					_context.CartDetails.Remove(delete);
					_context.SaveChanges();
				}
				return Ok();
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}
		[HttpPut("pay/{pvc}")]
		public IActionResult Pay([FromBody] string username, decimal pvc)
		{
			Console.WriteLine(pvc);
			//var check = HttpContext.Session.GetString("username");
			var data = _context.CartDetails.Include(a => a.Product).Include(a => a.Cart).Where(a => a.Cart.Username == username).ToList();
			if (username == null)
			{
				return BadRequest("chưa đăng nhâp em ơi");
			}
			else
			{
				decimal total = 0;

				Bill bills = new Bill()
				{
					Id = Guid.NewGuid().ToString(),
					CreateDate = DateTime.Now,
					Status = 1,
					Username = username,
				};
				List<BillDetails> lstBill = new List<BillDetails>();
				foreach (var item in data)
				{
					var cartDetails = _context.CartDetails.Include(a => a.Product).FirstOrDefault(a => a.Id == item.Id);
					if (cartDetails.Product.Amount < cartDetails.Quantity || cartDetails.Product.Amount == 0)
					{
						return BadRequest("Số lượng sản phẩm nike hiện tại không đủ , vui lòng chỉnh lại số lượng !");

					}
				}
				foreach (var item in data)
				{
					BillDetails billDetails = new BillDetails()
					{
						Id = Guid.NewGuid(),
						ProductId = item.ProductId,
						BillId = bills.Id,
						ProductPrice = item.Product.Price,
						Quantity = item.Quantity,
						Status = 1,
					};
					total += billDetails.ProductPrice * billDetails.Quantity;
					lstBill.Add(billDetails);
					item.Product.Amount -= billDetails.Quantity;
					_context.Products.Update(item.Product);
				}
				total += pvc;
				Console.WriteLine(pvc);
				Console.WriteLine(total);
				_context.BillDetails.AddRange(lstBill);
				bills.Money = total;
				_context.CartDetails.RemoveRange(data);

				_context.Bills.Add(bills); _context.SaveChanges();

			}
			return Ok();
		}

	}
}
