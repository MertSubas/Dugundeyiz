using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using Dugundeyiz.Models;
using Dugundeyiz.ViewModels;
using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Dugundeyiz.Identity;

namespace Dugundeyiz.Components
{
    public class LoginDataViewComponent : ViewComponent
    {
        private readonly ILogger<LoginDataViewComponent> _logger;
        private readonly DugundeyizContext _context;
        private readonly UserManager<AppUser> _userManager;
        public LoginDataViewComponent(ILogger<LoginDataViewComponent> logger, DugundeyizContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            HeaderUserViewModel userData = new HeaderUserViewModel() { };
            userData.username = "deneme";
            userData.name = "dasdasdasdaseneme";
            if (user != null) { userData.userID = user.Id; }

            return View(userData);
        }
    }
}
