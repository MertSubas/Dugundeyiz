using Dugundeyiz.Context;
using Dugundeyiz.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Dugundeyiz.Entity;


namespace Dugundeyiz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        private readonly ILogger<ServicesController> _logger;
        private readonly DugundeyizContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public ServicesController(ILogger<ServicesController> logger, DugundeyizContext context, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;


        }
        [HttpGet("cities")]
        public IActionResult GetCities()
        {
            var cities = _context.Citys
                .Select(c => new { c.Id, c.CityName }).OrderBy(x => x.CityName)
                .ToList();

            return Ok(cities);
        }
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var cities = _context.Categories.Where(x => x.Deleted != true)
                .Select(c => new { c.CategoryID, c.CategoryName })
                .ToList();

            return Ok(cities);
        }

        [HttpGet("getCategories")]
        public IActionResult GetRegularCategories()
        {
            var categories = _context.Categories.Where(x => x.Deleted != true && x.MainCategoryID == null)
                .Select(c => new { c.CategoryID, c.CategoryName })
                .ToList();

            return Ok(categories);
        }


        [HttpGet("getSpecialCategories")]
        public IActionResult GetSpecialCategories()
        {
            var categories = _context.Categories.Where(x => x.Deleted != true && x.MainCategoryID != 0 && x.MainCategoryID != null)
                .Select(c => new { c.CategoryID, c.CategoryName })
                .ToList();

            return Ok(categories);
        }

        [HttpGet("getCategoriesProductAdd")]
        public IActionResult getCategoriesProductAdd()
        {
            var categories = _context.Categories.Where(x => x.Deleted != true && x.MainCategoryID !=0)
                .Select(c => new { c.CategoryID, c.CategoryName })
                .ToList();

            return Ok(categories);
        }

        // İlçeleri getir
        [HttpGet("districts/{cityId}")]
        public IActionResult GetDistricts(int cityId)
        {
            var districts = _context.Towns
                .Where(d => d.CityID == cityId)
                .Select(d => new { d.ID, d.TownName }).OrderBy(x => x.TownName)
                .ToList();

            return Ok(districts);
        }


        public class FormModel
        {
            [Required(ErrorMessage = "Ad soyad zorunludur.")]
            [StringLength(100, ErrorMessage = "Ad soyad 100 karakterden uzun olamaz.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "E-posta zorunludur.")]
            [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Telefon numarası zorunludur.")]
            [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefon numarası 10 haneli olmalıdır.")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Konu zorunludur.")]
            [StringLength(150, ErrorMessage = "Konu 150 karakterden uzun olamaz.")]
            public string Subject { get; set; }

            [Required(ErrorMessage = "Mesaj zorunludur.")]
            [StringLength(1000, ErrorMessage = "Mesaj 1000 karakterden uzun olamaz.")]
            public string Message { get; set; }
        }



        [HttpPost]
        [Route("submit-form")]
        public IActionResult SubmitForm([FromBody] FormModel model)
        {
            if (ModelState.IsValid)
            {
                // Form verilerini işleme
                string smtpServer = _configuration["SmtpSettings:Server"];
                int smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
                string smtpUser = _configuration["SmtpSettings:User"];
                string smtpPass = _configuration["SmtpSettings:Pass"];

                // E-posta içeriği
                var dateTimeNow = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                string toEmail = "mertsubasm@gmail.com";
                string subject = $"Yeni İletişim Formu: {model.Subject}";
                string body = $@"
            <h1>Yeni İletişim Formu Gönderimi</h1>
            <p><strong>Ad Soyad:</strong> {model.Name}</p>
            <p><strong>E-posta:</strong> {model.Email}</p>
            <p><strong>Telefon:</strong> {model.Phone}</p>
            <p><strong>Konu:</strong> {model.Subject}</p>
            <p><strong>Mesaj:</strong><br>{model.Message}</p>
            <p><strong>Mesaj:</strong><br>{model.Message}</p>
<p><strong>Tarih:</strong><br>{dateTimeNow}</p>
        ";

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(smtpUser, "Dügünedeyiz İletişim");
                    mailMessage.To.Add(toEmail);
                    mailMessage.To.Add("vildan@dugundeyiz.com.tr");
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                        smtpClient.EnableSsl = true;

                        try
                        {
                            smtpClient.Send(mailMessage);
                            Contact newContact = new Contact();
                            newContact.Subject = model.Subject;
                            newContact.Email = model.Email;
                            newContact.Message = model.Message;
                            newContact.Name = model.Name;
                            newContact.Phone = model.Phone;
                            newContact.CreatedDate = DateTime.Now;
                            _context.Contacts.Add(newContact);
                            _context.SaveChanges();
                            return Ok(new { success = true });

                        }
                        catch (Exception ex)
                        {
                            return Ok(new { success = false });

                            throw; // Hatayı üst katmana ilet
                        }
                    }
                }
            }
            return BadRequest(new { success = false, message = "Geçersiz veri." });
        }

    }
}
