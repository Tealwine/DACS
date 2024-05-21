using SNKRS.Models;
using SNKRS.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace SNKRS.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly ApplicationDbContext db;

        public PortfolioController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Details(int? Id)
        {
            if (Id == null) return HttpNotFound();
            var portfolio = db.Portfolios.FirstOrDefault(p => p.Id == Id && p.isVisible);
            if (portfolio == null) return HttpNotFound();
           
            var relatedProducts = db.Portfolios.SqlQuery($"select * from Portfolios P where P.Id in ( select ProductId from ProductCategory PC where PC.CategoryId in ( select PC.CategoryId from ProductCategory PC where PC.ProductId = {portfolio.Id} ) and PC.ProductId <> {portfolio.Id} )").Take(4).ToList();
            if (!relatedProducts.Any())
            {
                relatedProducts = db.Portfolios.Take(4).ToList();
            }
            var model = new ProductViewModel();
            model.RelatedProducts = relatedProducts;
            model.Reviews = db.Reviews.Where(x => x.ProductId == Id).OrderByDescending(x => x.Id).Take(5).ToList();
            return View(model);
        }

        public void AddReview(int Id, string Content, int Rating)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                var review = new Review()
                {
                    ProductId = Id,
                    Content = Content,
                    Rating = Rating,
                    DateTime = System.DateTime.Now,
                };
                db.Reviews.Add(review);
                db.SaveChanges();
            }
        }
    }
}