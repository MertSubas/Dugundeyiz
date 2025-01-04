using Dugundeyiz.Context;
using Dugundeyiz.Controllers;
using Dugundeyiz.Entity;
using Dugundeyiz.Identity;
using Dugundeyiz.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<HomeController> _logger;
    private readonly DugundeyizContext _context;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<HomeController> logger, DugundeyizContext context, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _context = context;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterMember([FromBody] UserSingUpViewModel model) // Use [FromBody] to bind the data
    {
        if (ModelState.IsValid)
        {
            var usernameCheck = await _userManager.FindByNameAsync(model.UserName);
            if (usernameCheck != null)
            {
                return Json(new { Type = 2, Message = "Bu Kullanıcı Adında Bir Üye Zaten Mevcut Lütfen Farklı Bir Kullanıcı Adı Belirleyin" });
            }
            var user = new AppUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var addRoleResult = await _userManager.AddToRoleAsync(user, "User");
                if (addRoleResult.Succeeded)
                {
                    var newUser = new User
                    {
                        //UserID = user.Id, // Identity UserId'yi alın
                        Name = model.Name, // Modelde bu bilgileri aldığınızı varsayıyorum
                                           //Surname = model.Surname,
                        Email = model.Email,
                        Phone = model.Phone,
                        Role = Roles.User,
                        Admin = false,
                        IsApproved = true,
                        CreatedDate = DateTime.Now,
                        IdentityID = user.Id
                        //CreatedAt = DateTime.Now
                    };

                    _context.Users.Add(newUser);  // Kendi Users tablonuza ekleyin
                    await _context.SaveChangesAsync();  // Veritabanına kaydedin

                    // Kullanıcıyı oturum açtır
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home"); // Redirect to home page after logging out
                }
                else
                {
                    // Rol ekleme başarısız oldu
                    // Hata mesajlarını işleyebilirsiniz
                }
                // AspNetUsers tablosuna ekleme işlemi yapıldı, şimdi kendi Users tablonuza ekleyin

            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }
    public async Task<IActionResult> RegisterPartner([FromBody] PartnerSignUpViewModel model) // Use [FromBody] to bind the data
    {

        if (ModelState.IsValid)
        {
            if (model.Username != null)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Kullanıcı adı kontrolü
                        var usernameCheck = await _userManager.FindByNameAsync(model.Username);
                        if (usernameCheck != null)
                        {
                            return Json(new { Type = 2, Message = "Bu Kullanıcı Adında Bir Üye Zaten Mevcut Lütfen Farklı Bir Kullanıcı Adı Belirleyin" });
                        }

                        // Identity kullanıcı oluşturma
                        var user = new AppUser { UserName = model.Username, Email = model.Email };
                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            var addRoleResult = await _userManager.AddToRoleAsync(user, "Partner");
                            if (addRoleResult.Succeeded)
                            {
                                // Kendi Users tablonuza ekleme
                                var newUser = new User
                                {
                                    Name = model.Name,
                                    Surname = model.Surname,
                                    Email = model.Email,
                                    Phone = model.Phone,
                                    Role = Roles.Partner,
                                    Admin = false,
                                    IsApproved = false,
                                    CreatedDate = DateTime.Now,
                                    IdentityID = user.Id
                                };

                                _context.Users.Add(newUser);
                                await _context.SaveChangesAsync();

                                // Şehir ve İlçe bilgileri
                                var cityId = await _context.Citys
                                    .Where(x => x.CityName == model.City)
                                    .Select(x => x.Id)
                                    .FirstOrDefaultAsync();

                                var townId = await _context.Towns
                                    .Where(x => x.TownName == model.District)
                                    .Select(x => x.ID)
                                    .FirstOrDefaultAsync();

                                // Partner bilgileri ekleme
                                var partner = new UserPartner
                                {
                                    Adress = model.Address,
                                    UserID = user.Id,
                                    CompanyName = model.CompanyName,
                                    City = cityId,
                                    District = townId,
                                };

                                _context.UserPartners.Add(partner);
                                await _context.SaveChangesAsync();

                                // Kullanıcı onayı bilgisi ekleme
                                var approvalToken = Guid.NewGuid().ToString();
                                var approval = new UserApproval
                                {
                                    UserId = user.Id,
                                    ApprovalToken = approvalToken,
                                    ApprovalRequestedAt = DateTime.Now
                                };

                                _context.UserApprovals.Add(approval);
                                await _context.SaveChangesAsync();

                                // E-posta onayı gönderimi
                                var approvalLink = Url.Action("PartnerOnay", "YonetimPaneli", new { token = approvalToken }, Request.Scheme);
                                await SendApprovalEmail(approvalLink, model.Username, model.CompanyName, model.Phone);

                                // Transaction başarılı, commit işlemi
                                await transaction.CommitAsync();

                                return Ok();
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                            }
                        }
                        else
                        {
                            // Identity oluşturma hatalarını ekrana dökün
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }

                            // Transaction rollback
                            await transaction.RollbackAsync();
                            return BadRequest(ModelState);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Herhangi bir hata olursa rollback yap
                        await transaction.RollbackAsync();
                        return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
                    }
                }
            }
        }

        return View(model);
    }
    public async Task<IActionResult> LoginPartner([FromBody] UserViewModel model)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(model.UserName) && !string.IsNullOrWhiteSpace(model.Password))
            {
                //gjhgjhghjghj
                // Kullanıcıyı veritabanından bul
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    var userAccount = await _context.Users.Where(x=>x.IdentityID==user.Id).FirstOrDefaultAsync();
                    if (userAccount != null)
                    {
                        if (userAccount.Role != Roles.User)
                        {
                            if (userAccount.IsApproved != true)
                            {
                                return Json(new { Type = 2, Message = "Üyeliğiniz henüz onaylanmadı. Lütfen yönetici onayını bekleyin." });
                            }
                            // Şifreyi kontrol et
                            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);


                            if (result.Succeeded)
                            {
                                return ViewComponent("LoginData");
                            }
                            else if (result.IsLockedOut)
                            {
                                // Kullanıcı kilitli
                                return Json(new { Type = 2, Message = "Hesabınız çok fazla yanlış giriş nedeniyle kilitlendi. Lütfen bir süre bekleyip tekrar deneyin." });
                            }
                            else
                            {
                                return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı veya şifre." });

                            }
                        }
                        else
                        {
                            return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı veya şifre." });
                        }
                    }
                    else
                    {
                        return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı veya şifre." });
                    }
                }
                else
                {
                    return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı veya şifre." });
                }
            }
            else
            {
                return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı veya şifre." });
            }
        }
        catch (Exception ex)
        {
            return Json(new { Type = 2, Message = "Bir Hata Oluştu, Lütfen Daha Sonra Tekrar Deneyiniz (Bu Durum Tekrarlanırsa Lütfen Yöneticinize Bildiriniz) " });
        }

    }
    public async Task<IActionResult> LoginUser([FromBody] UserViewModel model)
    {
        if (!string.IsNullOrWhiteSpace(model.UserName) && !string.IsNullOrWhiteSpace(model.Password))
        {
            // Kullanıcıyı veritabanından bul
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                if ((_context.Users.Where(x=>x.IdentityID==user.Id).FirstOrDefault().Role) == Roles.User)
                {
                    // Şifreyi kontrol et
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        return ViewComponent("LoginData");

                        //return Ok(); // Oturum başarılı, ana sayfaya yönlendir
                    }
                    else if (result.IsLockedOut)
                    {
                        // Kullanıcı kilitli
                        return Json(new { Type = 2, Message = "Hesabınız çok fazla yanlış giriş nedeniyle kilitlendi. Lütfen bir süre bekleyip tekrar deneyin." });
                    }
                    else
                    {
                        return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı veya şifre." });
                    }
                }
                else
                {
                    return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı veya şifre." });
                }
            }
            else
            {
                return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı veya şifre." });
            }
        }
        else
        {
            return Json(new { Type = 2, Message = "Geçersiz kullanıcı adı veya şifre." });
        }
        return ViewComponent("LoginData");

        //return View(model); // Hata varsa formu tekrar göster
    }
    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync(); // Sign the user out
        return RedirectToAction("Index", "Home"); // Redirect to home page after logging out
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            var user = _context.Users.Where(x => x.IdentityID == loggedUser.Id).FirstOrDefault();

            // Kullanıcı bilgilerini güncellemek için gerekli iş mantığı
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Sadece dolu olan alanları güncelle
            user.Name = string.IsNullOrEmpty(model.Name) ? user.Name : model.Name;
            user.Surname = string.IsNullOrEmpty(model.Surname) ? user.Surname : model.Surname;
            user.Phone = string.IsNullOrEmpty(model.Phone) ? user.Phone : model.Phone;
            user.Email = string.IsNullOrEmpty(model.Email) ? user.Email : model.Email;
            if (model.CompanyName != null)
            {
                var partnerInfo = _context.UserPartners.Where(x => x.UserID == loggedUser.Id).FirstOrDefault();
                partnerInfo.CompanyName = string.IsNullOrEmpty(model.CompanyName) ? partnerInfo.CompanyName : model.CompanyName;
                partnerInfo.Adress = string.IsNullOrEmpty(model.Address) ? partnerInfo.Adress : model.Address;
                //partnerInfo.AddressDesc = string.IsNullOrEmpty(model.AddressDesc) ? partnerInfo.AddressDesc : model.AddressDesc;

                var cityId = await _context.Citys.Where(x => x.CityName == model.City).Select(x => x.Id).FirstOrDefaultAsync();
                var TownID = await _context.Towns.Where(x => x.TownName == model.District).Select(x => x.ID).FirstOrDefaultAsync();
                partnerInfo.City = cityId;
                partnerInfo.District = TownID;

                await _context.SaveChangesAsync();
            }
            return Ok(new { Message = "Profile updated successfully" });
        }

        return BadRequest(new { Message = "Invalid data" });
    }

    public IActionResult AccessDenied()
    {
        return View();
    }


    public async Task<IActionResult> MyProfile()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var profile = _context.Users.Where(x => x.IdentityID == user.Id).FirstOrDefault();

        //mail kontrolü de ekle
        PartnerEditProfile profileToSend = new PartnerEditProfile();
        profileToSend.Surname = profile.Surname;
        profileToSend.Name = profile.Name;
        profileToSend.Email = profile.Email;
        profileToSend.Phone = profile.Phone;
        profileToSend.Address = profile.Name;

        ProfileDatatoSend profileData = new ProfileDatatoSend();
        var partnerInfo = _context.UserPartners.Where(x => x.UserID == user.Id).FirstOrDefault();
        if (partnerInfo != null)
        {
            if (partnerInfo.City != null)
            {
                var cityName = _context.Citys.Where(x => x.Id == partnerInfo.City).Select(x => x.CityName).FirstOrDefault();
                var townName = _context.Towns.Where(x => x.ID == partnerInfo.District).Select(x => x.TownName).FirstOrDefault();
                profileData.CityName = cityName;
                profileData.DistrictName = townName;
            }
        }

        profileData.UserpartnerInfo = partnerInfo;
        profileData.Profile = profileToSend;
        return View(profileData);
    }



    public async Task SendApprovalEmail(string approvalLink, string userName, string companyName, string phone)
    {

        string subject = "Yeni Üyelik Onayı";
        string body = $"Yeni bir partner üyelik isteği var.\n\n" +
            $"Kullanıcı Adı: {userName}\n" +
            $"Firma Adı: {companyName}\n" +
            $"Telefon Numarası: {phone}\n\n" +
            $"Onaylamak için aşağıdaki linke tıklayın:\n{approvalLink}";

        //message.To.Add(adminEmail);
        string smtpServer = _configuration["SmtpSettings:Server"];
        int smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
        string smtpUser = _configuration["SmtpSettings:User"];
        string smtpPass = _configuration["SmtpSettings:Pass"];

        using (var mailMessage = new MailMessage())
        {
            mailMessage.From = new MailAddress(smtpUser, "Dügünedeyiz Üyelik Onay Sistemi");
            mailMessage.To.Add("mertsubasm@gmail.com");
            mailMessage.To.Add("vildan@dugundeyiz.com.tr");
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = false;

            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtpClient.EnableSsl = true;

                try
                {
                    smtpClient.Send(mailMessage);

                }
                catch (Exception ex)
                {
                    throw; // Hatayı üst katmana ilet
                }
            }
        }
    }




}
