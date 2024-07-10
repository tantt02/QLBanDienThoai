using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ontap_Net104_319.Models;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        AppDbContext _context;
        public AccountController()
        {
            _context = new AppDbContext();
        }
        [HttpPost("login")]
        public ActionResult Login(string username, string password)
        {
            if (username == null && password == null)
            {
                return BadRequest("username và password không để trống");
            }
            else
            {
                var data = _context.Accounts.FirstOrDefault(p => p.Username == username && p.Password == password);
                if (data == null) 
                {
                    return BadRequest("Đăng nhập thất bại mời kiểm tra lại");
                }
                else 
                {
                    //HttpContext.Session.SetString("username", username); 

                    return Ok();
                }
            }
        }

        [HttpPost("SignUp")]
        public ActionResult SignUp([FromBody] Account account)
        {
            try
            {
                _context.Accounts.Add(account);
                // Tạo mới đồng thời 1 giỏ hàng
                Cart cart = new Cart()
                {
                    Username = account.Username,
                    Status = 1
                };
                _context.Carts.Add(cart);
                _context.SaveChanges(); // Lưu thay đổi
                //TempData["Status"] = "Tạo tài khoản thành công";
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
