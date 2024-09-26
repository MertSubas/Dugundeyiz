using Dugundeyiz.Context;
using Dugundeyiz.Controllers;
using Dugundeyiz.Entity;
using Dugundeyiz.Identity;
using Dugundeyiz.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<HomeController> _logger;
    private readonly DugundeyizContext _context;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<HomeController> logger, DugundeyizContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserViewModel model) // Use [FromBody] to bind the data
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // AspNetUsers tablosuna ekleme işlemi yapıldı, şimdi kendi Users tablonuza ekleyin
                var newUser = new User
                {
                    //UserID = user.Id, // Identity UserId'yi alın
                    Name = model.Name, // Modelde bu bilgileri aldığınızı varsayıyorum
                    Surname = model.Surname,
                    Email = model.Email,
                    Phone = model.Phone,
                    Role = (Roles)model.Role,
                    Admin = model.Admin,
                    //CreatedAt = DateTime.Now
                };

                _context.Users.Add(newUser);  // Kendi Users tablonuza ekleyin
                await _context.SaveChangesAsync();  // Veritabanına kaydedin

                // Kullanıcıyı oturum açtır
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home"); // Redirect to home page after logging out
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }
    public async Task<IActionResult> Login([FromBody] UserViewModel model)
    {
        if (!string.IsNullOrWhiteSpace(model.UserName) && !string.IsNullOrWhiteSpace(model.Password))
        {
            // Kullanıcıyı veritabanından bul
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                // Şifreyi kontrol et
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok(); // Oturum başarılı, ana sayfaya yönlendir
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre."); // Hatalı giriş
                }
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
            }
        }
        else
        {
            ModelState.AddModelError("", "Kullanıcı adı ve şifre gerekli.");
        }

        return View(model); // Hata varsa formu tekrar göster
    }
    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync(); // Sign the user out
        return RedirectToAction("Index", "Home"); // Redirect to home page after logging out
    }

}
