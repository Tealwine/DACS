using SNKRS.Models;
using SNKRS.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace SNKRS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            List<Portfolio> upcoming = db.Portfolios.Where(x => x.isVisible).OrderBy(p => p.Id).OrderByDescending(p => p.Id).Take(4).ToList();
        
            HomeViewModel model = new HomeViewModel();
            model.Upcoming = upcoming;
            return View(model);
        }
    }
}