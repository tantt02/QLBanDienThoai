using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Ontap_Net104_319.Models;
using System.Xml.Schema;

namespace Ontap_Net104_319.Controllers
{
	public class BillController : Controller
	{
		HttpClient _client;

		AppDbContext _context;
		public BillController()
		{
			_client = new HttpClient();
			_context = new AppDbContext();
		}
		// GET: BillCOntroller
		public IActionResult Index()
		{
			if (HttpContext.Session.GetString("username") != null)
			{
				string requesstURl = $"https://localhost:7001/api/Bill/get-bill?username={HttpContext.Session.GetString("username")}";
				var response = _client.GetStringAsync(requesstURl).Result;
				var data = JsonConvert.DeserializeObject<List<Bill>>(response);
				return View(data);
			}
			return RedirectToAction("Login", "Account");
		}

		public IActionResult CancelBill(string username, string id)
		{
			if (HttpContext.Session.GetString("username") != null)
			{
				string requesstURl = $"https://localhost:7001/api/Bill/Cancel-bill?username={HttpContext.Session.GetString("username")}&id={id}";
				var response = _client.PutAsJsonAsync(requesstURl, requesstURl).Result;
				return RedirectToAction("Index", "Bill");
			}
			return BadRequest("Lỗi");
		}

		public IActionResult RepurchaseBill(string username, string id)
		{
			var bill = _context.Bills.Include(b => b.Details).FirstOrDefault(b => b.Id == id && b.Username == HttpContext.Session.GetString("username"));
			if (HttpContext.Session.GetString("username") != null)
			{
				foreach (var item in bill.Details)
				{
					var billDetails = _context.BillDetails.Include(a => a.Product).FirstOrDefault(a => a.Id == item.Id);
					if (billDetails.Product.Amount < billDetails.Quantity || billDetails.Product.Amount == 0)
					{
						TempData["Invalid"] = $"{billDetails.Product.Name}";
						return RedirectToAction("Index", "Bill");
					}
				}
				string requesstURl = $"https://localhost:7001/api/Bill/repurchase-bill?username={HttpContext.Session.GetString("username")}&id={id}";
				var response = _client.PutAsJsonAsync(requesstURl, requesstURl).Result;
				return RedirectToAction("Index", "cart");
			}
			return BadRequest("Lỗi");
		}
	}

}
