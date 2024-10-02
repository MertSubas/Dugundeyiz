using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using Dugundeyiz.Models;
using Dugundeyiz.ViewModels;
using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Dugundeyiz.Components
{
    public class LayoutDataViewComponent : ViewComponent
    {
        private readonly ILogger<LayoutDataViewComponent> _logger;
        private readonly DugundeyizContext _context;
        public LayoutDataViewComponent(ILogger<LayoutDataViewComponent> logger, DugundeyizContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var subCategories = _context.Categories.Where(x=>x.Deleted!=true&&x.MainCategoryID!=null).ToList();
            LayoutviewModel dataForLayout = new LayoutviewModel();
            dataForLayout.LayoutAltKategoriler = subCategories;
            return View(dataForLayout);
        }
    }
}
