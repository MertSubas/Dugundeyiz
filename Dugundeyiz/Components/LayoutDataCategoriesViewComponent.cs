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
    public class LayoutDataCategoriesViewComponent : ViewComponent
    {
        private readonly ILogger<LayoutDataCategoriesViewComponent> _logger;
        private readonly DugundeyizContext _context;
        public LayoutDataCategoriesViewComponent(ILogger<LayoutDataCategoriesViewComponent> logger, DugundeyizContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var subCategories = _context.Categories.Where(x=>x.Deleted!=true&&x.MainCategoryID==null).OrderBy(x=>x.Sorting).ToList();
            LayoutviewModel dataForLayout = new LayoutviewModel();
            dataForLayout.LayoutKategoriler = subCategories;
            return View(dataForLayout);
        }
    }
}
