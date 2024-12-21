using Dugundeyiz.Context;
using Dugundeyiz.Entity;
using Dugundeyiz.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dugundeyiz.Areas.Admin.Controllers
{

	[Area("Admin")]
	public class AccessController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ILogger<HomeController> _logger;
		private readonly DugundeyizContext _context;
		private readonly IConfiguration _configuration;

		public AccessController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<HomeController> logger, DugundeyizContext context, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_context = context;
			_configuration = configuration;
		}
		public IActionResult Index()		
		{
			
			return View();
		}

		public class LoginForm
		{
            public string username { get; set; }
            public string pass { get; set; }
        }
		public async Task<IActionResult> LoginAdmin([FromBody] LoginForm formdata)
		{
			if (formdata == null)
			{
				return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı ve şifre denemesi sisteme kullanılan cihaz ve adresler ile kaydedildi." });
			}
			var user = await _userManager.FindByNameAsync(formdata.username);
			if(user == null)
			{
				return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı ve şifre denemesi sisteme kullanılan cihaz ve adresler ile kaydedildi." });

			}
			var userAccount = await _context.Users.FindAsync(user.Id);
			if (userAccount.Role == Roles.Admin && userAccount.Admin==true)
			{
				var result = await _signInManager.PasswordSignInAsync(user, formdata.pass, isPersistent: false, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					return Json(new { Type = 1, Message = "Giris basarili" });
				}
				else
				{
					return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı ve şifre denemesi sisteme kullanılan cihaz ve adresler ile kaydedildi." });

				}
			}
			else
			{
				return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı ve şifre denemesi sisteme kullanılan cihaz ve adresler ile kaydedildi." });

			}


		}
	}
}
