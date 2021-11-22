using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using RegionalShop.App_Start;
using RegionalShop.DAL;
using RegionalShop.Infrastructure;
using RegionalShop.Models;
using RegionalShop.ViewModels;

namespace RegionalShop.Controllers
{
    public class KoszykController : Controller
    {
        private KoszykMenager koszykMenager;
        private ISessionMenager sessionMenager { get; set; }
        private ProduktyContext db;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IMailService maileService;

        public KoszykController(IMailService maileService)
        {
            this.maileService = maileService;
            db = new ProduktyContext();
            sessionMenager = new SessionMenager();
            koszykMenager = new KoszykMenager(sessionMenager, db);
        }
        // GET: Koszyk
        public ActionResult Index()
        {
            var name = User.Identity.Name;
            logger.Info("Strona koszyk | " + name);
            var pozycjeKoszyka = koszykMenager.PobierzKoszyk();
            var cenaCalkowita = koszykMenager.PobierzWartoscKoszyka();
            KoszykViewModel koszykVM = new KoszykViewModel()
            {
                PozycjeKoszyka = pozycjeKoszyka,
                CenaCalkowita = cenaCalkowita
            };

            return View(koszykVM);
        }

        public ActionResult DodajDoKoszyka(int id)
        {
            koszykMenager.DodajDoKoszyka(id);

            return RedirectToAction("Index");
        }

        public int PobierzIloscElementowKoszyka()
        {
            return koszykMenager.PobierzIloscPozycjiKoszyka();
        }

        public ActionResult UsunZKoszyka(int produktId)
        {
            int iloscPozycji = koszykMenager.UsunZKoszyka(produktId);
            int iloscPozycjiKoszyka = koszykMenager.PobierzIloscPozycjiKoszyka();
            decimal wartoscKoszyka = koszykMenager.PobierzWartoscKoszyka();

            var wynik = new KoszykUsuwanieViewModel
            {
                IdPozycjiUsuwanej = produktId,
                IloscPozycjiUsuwanej = iloscPozycji,
                KoszykCenaCalkowita = wartoscKoszyka,
                KoszykIloscPozycji = iloscPozycjiKoszyka,
            };
            return Json(wynik);
        }

        public async Task<ActionResult> Zaplac()
        {
            var name = User.Identity.Name;
            logger.Info("Strona koszyk | zaplac | " + name);
            if (Request.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                var zamowienie = new Zamowienie
                {
                    Imie = user.DaneUzytkownika.Imie,
                    Nazwisko = user.DaneUzytkownika.Nazwisko,
                    Adres = user.DaneUzytkownika.Adres,
                    Miejscowosc = user.DaneUzytkownika.Miejscowosc,
                    KodPocztowy = user.DaneUzytkownika.KodPocztowy,
                    Email = user.Email,
                    Telefon = user.DaneUzytkownika.Telefon
                };
                return View(zamowienie);
            }
            else
            { 
                return RedirectToAction("Login", "Account", 
                    new { returnUrl = Url.Action("Zaplac", "Koszyk") });
            }
        }

        
        [HttpPost]
        public async Task<ActionResult> Zaplac(Zamowienie zamowienieSzczegoly)
        {
            if (ModelState.IsValid)
            {
                // pobieramy id uzytkownika aktualnie zalogowanego
                var userId = User.Identity.GetUserId();

                // utworzenie obiektu zamowienia na podstawie tego co mamy w koszyku
                var newOrder = koszykMenager.UtworzZamowienie(zamowienieSzczegoly, userId);

                // szczegóły użytkownika - aktualizacja danych 
                var user = await UserManager.FindByIdAsync(userId);
                TryUpdateModel(user.DaneUzytkownika);
                await UserManager.UpdateAsync(user);

                // opróżnimy nasz koszyk zakupów
                koszykMenager.PustyKoszyk();

                maileService.WyslaniePotwierdzenieZamowieniaEmail(newOrder);

                return RedirectToAction("PotwierdzenieZamowienia");
            }
            else
            {
                return View(zamowienieSzczegoly);
            }
        }


        public ActionResult PotwierdzenieZamowienia()
        {
            var name = User.Identity.Name;
            logger.Info("Strona koszyk | potwierdzenie | " + name);
            return View();
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



    }
}