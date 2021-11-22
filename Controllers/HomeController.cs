using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using NLog;
using RegionalShop.DAL;
using RegionalShop.Infrastructure;
using RegionalShop.Models;
using RegionalShop.ViewModels;

namespace RegionalShop.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private ProduktyContext db = new ProduktyContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public HomeController(ProduktyContext context)
        {
            this.db = context;
        }
        public ActionResult Index()
        {
            logger.Info("Jesteś na stronie głównwej");
            ICacheProvider cache = new DefaultCacheProvider();
            List<Kategoria> kategorie;
            db.Database.Log = message => Trace.WriteLine(message);
            if (cache.IsSet(Consts.KategorieCacheKey))
            {
                kategorie = cache.Get(Consts.KategorieCacheKey) as List<Kategoria>;
            }
            else
            {
                kategorie = db.Kategorie.ToList();
                cache.Set(Consts.KategorieCacheKey, kategorie, 60);
            }
            List<Produkt> nowosci;
            if(cache.IsSet(Consts.NowosciCacheKey))
            {
                nowosci = cache.Get(Consts.NowosciCacheKey) as List<Produkt>;
            }
            else
            {
                nowosci = db.Produkty.Where(a => !a.Ukryty).OrderByDescending(a => a.DataDodania).Take(3).ToList();
                cache.Set(Consts.NowosciCacheKey,nowosci,60);
            }
            List<Produkt> bestseller;
            if (cache.IsSet(Consts.BestsellerCacheKey))
            {
                bestseller = cache.Get(Consts.BestsellerCacheKey) as List<Produkt>;
            }
            else
            {
                bestseller = db.Produkty.Where(a => !a.Ukryty && a.Bestseller).OrderBy(a => Guid.NewGuid()).Take(3).ToList();
                cache.Set(Consts.BestsellerCacheKey, bestseller, 60);
            }
            var vm = new HomeViewModel()
            {
                Kategorie = kategorie,
                Nowosci = nowosci,
                Bestsellery = bestseller,
            };
            return View(vm);
        }

        public ActionResult StronyStatyczne(string nazwa)
        {
            var name = User.Identity.Name;
            logger.Info("Strona " + nazwa + " | " + name);
            return View(nazwa);
        }

   

    }
}