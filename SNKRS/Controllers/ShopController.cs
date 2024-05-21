using SNKRS.Models;
using SNKRS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace SNKRS.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext db;

        public ShopController()
        {
            db = new ApplicationDbContext();
        }
        public ActionResult Index(string search, string order, string categories, string sizes, int? page)
        {
            var products = db.Portfolios.Where(x => x.isVisible).OrderByDescending(x => x.Id).ToList();

            List<string> filterCategories;
            if (!string.IsNullOrEmpty(categories))
            {
                filterCategories = categories.Split(',').ToList();
                products = products.Where(x => x.Categories.Select(s => s.Name).ToList().Intersect(filterCategories).Any()).ToList();
                ViewBag.categories = categories;
            }
            List<string> filterSizes;
            if (!string.IsNullOrEmpty(sizes))
            {
                filterSizes = sizes.Split(',').ToList();
                
                ViewBag.sizes = sizes;
            }
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(x => x.Name.Contains($"{search}")).ToList();
                ViewBag.search = search;
            }
            
                if (page == null)
            {
                page = 1;
            }
            var pageSize = 16;
            int pageNum = page ?? 1;
            var viewModel = new ShopViewModel()
            {
                Products = products.ToPagedList(pageNum, pageSize),
                AllCategories = db.Categories.Select(x => x.Name).ToList(),
            };
            return View(viewModel);
        }
    }
}