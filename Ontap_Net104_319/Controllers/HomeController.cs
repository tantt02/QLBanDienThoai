using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ontap_Net104_319.Models;
using System.Diagnostics;

namespace Ontap_Net104_319.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		HttpClient _client;
		AppDbContext _context;
		public HomeController(ILogger<HomeController> logger)
        {

			_client = new HttpClient();
			_context = new AppDbContext();
			_logger = logger;
        }

        public IActionResult Index()
        {
            var data = HttpContext.Session.GetString("username"); // Lấy data từ session
            if (data == null) ViewData["login"] = "Chưa đăng nhập";
            else ViewData["login"] = "Xin chào " + data;
            return View();
        }
		public IActionResult List()
		{
			string requesstURl = "https://localhost:7001/api/Product/get-all";
			var response = _client.GetStringAsync(requesstURl).Result;
			var data = JsonConvert.DeserializeObject<List<Product>>(response);
			return View(data);

		}
		public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}