using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using RegionalShop.DAL;

namespace RegionalShop.Controllers
{
    public class ProduktyController : Controller
    {
        private ProduktyContext db = new ProduktyContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        // GET: Produkty

        public ProduktyController(ProduktyContext context)
        {
            this.db = context;
        }

        public ActionResult Index()
        {
            var name = User.Identity.Name;
            logger.Info("Strona Produkty | " + name);
            return View();
        }

        public ActionResult Lista(string nazwaKategorii, string searchQuery = null)
        {
            var name = User.Identity.Name;
            logger.Info("Strona kategoria | " + nazwaKategorii + " | " + name);
            var kategoria=db.Kategorie.Include("Produkty").Where(k =>k.NazwaKategorii.ToUpper()==nazwaKategorii.ToUpper()).Single();
            var produkty=kategoria.Produkty.Where(a => ( !a.Ukryty));
            if (searchQuery != null)
            {
                produkty = db.Produkty.Where(a => (a.NazwaProduktu.ToLower().Contains(searchQuery.ToLower())) && !a.Ukryty);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProduktyList", produkty);
            }

            return View(produkty);
        }

        public ActionResult Szczegoly(int id)
        {
            var produkt = db.Produkty.Find(id);
            var name = User.Identity.Name;
            logger.Info("Strona szczególy | " + produkt.NazwaProduktu + " | " + name);
            return View(produkt);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 60000)]
        public ActionResult KategorieMenu()
        {
            var kategorie = db.Kategorie.ToList();
            return PartialView("_KategorieMenu", kategorie);
        }

        public ActionResult ProduktyPodpowiedzi(string term)
        {
            var produkty = db.Produkty.Where(a => !a.Ukryty && a.NazwaProduktu.ToLower().Contains(term.ToLower()))
                        .Take(5).Select(a => new { label = a.NazwaProduktu });

            return Json(produkty, JsonRequestBehavior.AllowGet);
        }


    }
}