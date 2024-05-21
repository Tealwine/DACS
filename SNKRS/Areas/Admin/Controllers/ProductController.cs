using SNKRS.Areas.Admin.DTOs;
using SNKRS.Areas.Admin.ViewModels;
using SNKRS.Models;
using System.Linq;
using System.Web.Mvc;

namespace SNKRS.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : AdminController
    {
        private readonly ApplicationDbContext db;

        public ProductController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var portfolios = db.Portfolios.ToList();
            return View(portfolios);
        }

        public ActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel
            {
                Categories = db.Categories.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = db.Categories.ToList();
                return View("Create", viewModel);
            }
            var product = new Portfolio()
            {
                Name = viewModel.Name,              
                Image = viewModel.Image,
                Description = viewModel.Description,
                isVisible = viewModel.isVisible
            };
            if (viewModel.ProductCategories != null)
            {
                foreach (var item in viewModel.ProductCategories)
                {
                    product.Categories.Add(db.Categories.Single(x => x.Id == item));
                }
            }
            db.Portfolios.Add(product);
            db.SaveChanges();
            return RedirectToAction("Edit", new { Id = product.Id });
        }

        public void Delete(int Id)
        {
            var product = db.Portfolios.SingleOrDefault(p => p.Id == Id);
            db.Portfolios.Remove(product);
            db.SaveChanges();
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null) return HttpNotFound();
            var product = db.Portfolios.SingleOrDefault(p => p.Id == Id);
            if (product == null) return HttpNotFound();
            var viewmodel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                ProductCategories = product.Categories.Select(x => x.Id).ToList(),
                Categories = db.Categories.ToList(),
                ProductGalleries = product.ProductGalleries,
                isVisible = product.isVisible,
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel viewModel)
        {
            var product = db.Portfolios.First(p => p.Id == viewModel.Id);
            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.Image = viewModel.Image;
            product.isVisible = viewModel.isVisible;
            product.Categories.Clear();
            if (viewModel.ProductCategories != null)
            {
                foreach (var item in viewModel.ProductCategories)
                {
                    product.Categories.Add(db.Categories.Single(x => x.Id == item));
                }
            }
            db.SaveChanges();
            return RedirectToAction("Edit", new { Id = viewModel.Id });
        }

        [HttpPost]
       

      

        public void AddGallery(int Id, string Src)
        {
            var product = db.Portfolios.FirstOrDefault(x => x.Id == Id);
            if (product != null)
            {
                var productGallery = new ProductGallery()
                {
                    ProductId = product.Id,
                    Src = Src,
                };
                db.ProductGalleries.Add(productGallery);
                db.SaveChanges();
            }
        }
    }
}