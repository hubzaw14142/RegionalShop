using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RegionalShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ProduktyList",
                url: "Kategoria/{NazwaKategorii}",
                defaults: new { controller = "Home", action = "Lista" });

            routes.MapRoute(
                 name: "ProduktySzczegoly",
                 url: "produkt-{id}.html",
                 defaults: new { controller = "Produkty", action = "Szczegoly" });

            routes.MapRoute(
                name: "StronyStatyczne",
                url: "strona/{nazwa}.html",
                defaults: new { controller = "Produkty", action = "StronyStatyczne" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
