using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NLog;
using RegionalShop.App_Start;
using RegionalShop.DAL;
using RegionalShop.Infrastructure;
using RegionalShop.Models;
using RegionalShop.ViewModels;

namespace RegionalShop.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {

        private ProduktyContext db;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IMailService mailService;
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }
        public ManageController(ProduktyContext context, IMailService mailService)
        {
            this.db = context;
            this.mailService = mailService;
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Manage
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            var name = User.Identity.Name;
            logger.Info("Admin główna | " + name);

            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }

            if (User.IsInRole("Admin"))
                ViewBag.UserIsAdmin = true;
            else
                ViewBag.UserIsAdmin = false;

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }

            var model = new ManageCredentialsViewModel
            {
                Message = message,
                DaneUzytkownika = user
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile([Bind(Prefix = "DaneUzytkownika")] DaneUzytkownika daneUzytkownika)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.DaneUzytkownika = daneUzytkownika;
                var result = await UserManager.UpdateAsync(user);

                AddErrors(result);
            }

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Prefix = "ChangePasswordViewModel")] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var message = ManageMessageId.ChangePasswordSuccess;
            return RedirectToAction("Index", new { Message = message });
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("password-error", error);
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        public ActionResult ListaZamowien(string searching)
        {

            bool isAdmin = User.IsInRole("Admin");
            ViewBag.UserIsAdmin = isAdmin;

            IEnumerable<Zamowienie> zamowieniaUzytkownika;

            // Dla administratora zwracamy wszystkie zamowienia
            if (isAdmin)
            {
                if (searching == null)
                {
                    zamowieniaUzytkownika = db.Zamowienia.Include("PozycjeZamowienia").OrderByDescending(o => o.DataDodania).ToArray();
                }
                else
                {
                    searching = Regex.Match(searching, @"\d+").Value;
                    zamowieniaUzytkownika = db.Zamowienia.Where(o =>  o.ZamowienieId.ToString().Contains(searching)).Include("PozycjeZamowienia").OrderByDescending(o => o.DataDodania).ToArray();
                }
            }
            else
            {
                var userId = User.Identity.GetUserId();
                if (searching == null) {
                    zamowieniaUzytkownika = db.Zamowienia.Where(o => o.UserId == userId).Include("PozycjeZamowienia").OrderByDescending(o => o.DataDodania).ToArray(); 
                }
                else
                {
                    searching = Regex.Match(searching, @"\d+").Value;
                    zamowieniaUzytkownika = db.Zamowienia.Where(o => o.UserId == userId && o.ZamowienieId.ToString().Contains(searching)).Include("PozycjeZamowienia").OrderByDescending(o => o.DataDodania).ToArray();
                }
               
            }
            

            return View(zamowieniaUzytkownika);
        }


        public ActionResult ZamowieniaPodpowiedzi(string term)
        {        
        var zamowienia = db.Zamowienia.Where(
        a => a.Imie.ToLower().Contains(term.ToLower()) || a.Nazwisko.ToLower().Contains(term.ToLower()) || 
        a.ZamowienieId.ToString().Contains(term))
        .Take(10).Select(a => new { label =  a.ZamowienieId.ToString()+a.Imie.ToString()+a.Nazwisko.ToString() }) ;

            return Json(zamowienia, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zamowienie zamowienie = db.Zamowienia.Find(id);
            if (zamowienie == null)
            {
                return HttpNotFound();
            }
            return View(zamowienie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Zamowienie zamowienie = db.Zamowienia.Find(id);
            db.Zamowienia.Remove(zamowienie);
            db.SaveChanges();
            return RedirectToAction("ListaZamowien");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public StanZamowienia ZmianaStanuZamowienia(Zamowienie zamowienie)
        {
            Zamowienie zamowienieDoModyfikacji = db.Zamowienia.Find(zamowienie.ZamowienieId);
            zamowienieDoModyfikacji.StanZamowienia = zamowienie.StanZamowienia;
            db.SaveChanges();

            if (zamowienieDoModyfikacji.StanZamowienia == StanZamowienia.Zrealizowane)
            {
                this.mailService.WyslanieZamowienieZrealizowaneEmail(zamowienieDoModyfikacji);
            }        
            return zamowienie.StanZamowienia;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DodajProdukt(int? produktId, bool? potwierdzenie)
        {
            Produkt produkt;
            if (produktId.HasValue)
            {
                ViewBag.EditMode = true;
                produkt = db.Produkty.Find(produktId);
            }
            else
            {
                ViewBag.EditMode = false;
                produkt = new Produkt();
            }
            var result = new EditProduktViewModel();
            result.Kategorie = db.Kategorie.ToList();
            result.Produkt = produkt;
            result.Potwierdzenie = potwierdzenie;

            return View(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DodajProdukt(EditProduktViewModel model, HttpPostedFileBase file)
        {
            if (model.Produkt.ProduktId > 0)
            {
                // modyfikacja produktu
                db.Entry(model.Produkt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DodajProdukt", new { potwierdzenie = true });
            }
            else
            {
                // Sprawdzenie, czy użytkownik wybrał plik
                if (file != null && file.ContentLength > 0)
                {
                    if (ModelState.IsValid)
                    {
                        // Generowanie pliku
                        var fileExt = Path.GetExtension(file.FileName);
                        var filename = Guid.NewGuid() + fileExt;
                        var path = Path.Combine(Server.MapPath(AppConfig.ObrazkiFolderWzgledny), filename);
                        file.SaveAs(path);
                        model.Produkt.NazwaPlikuObrazka = filename;
                        model.Produkt.DataDodania = DateTime.Now;
                        db.Entry(model.Produkt).State = EntityState.Added;
                        db.SaveChanges();
                        return RedirectToAction("DodajProdukt", new { potwierdzenie = true });
                    }
                    else
                    {
                        var kategorie = db.Kategorie.ToList();
                        model.Kategorie = kategorie;
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nie wskazano pliku");
                    var kategorie = db.Kategorie.ToList();
                    model.Kategorie = kategorie;
                    return View(model);
                }
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult UkryjProdukt(int produktId)
        {
            var produkt = db.Produkty.Find(produktId);
            produkt.Ukryty = true;
            db.SaveChanges();

            return RedirectToAction("DodajProdukt", new { potwierdzenie = true });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PokazProdukt(int produktId)
        {
            var produkt = db.Produkty.Find(produktId);
            produkt.Ukryty = false;
            db.SaveChanges();

            return RedirectToAction("DodajProdukt", new { potwierdzenie = true });
        }

        [AllowAnonymous]
        public ActionResult WyslaniePotwierdzenieZamowieniaEmail(int zamowienieId, string nazwisko)
        {
            var zamowienie = db.Zamowienia.Include("PozycjeZamowienia").Include("PozycjeZamowienia.Produkt")
                               .SingleOrDefault(o => o.ZamowienieId == zamowienieId && o.Nazwisko == nazwisko);

            if (zamowienie == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            PotwierdzenieZamowieniaEmail email = new PotwierdzenieZamowieniaEmail();
            email.To = zamowienie.Email;
            email.From = "newregionalshop@gmail.com";
            email.Wartosc = zamowienie.WartoscZamowienia;
            email.NumerZamowienia = zamowienie.ZamowienieId;
            email.PozycjeZamowienia = zamowienie.PozycjeZamowienia;
            email.sciezkaObrazka = AppConfig.ObrazkiFolderWzgledny;
            email.Send();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [AllowAnonymous]
        public ActionResult WyslanieZamowienieZrealizowaneEmail(int zamowienieId, string nazwisko)
        {
            var zamowienie = db.Zamowienia.Include("PozycjeZamowienia").Include("PozycjeZamowienia.Produkt")
                                  .SingleOrDefault(o => o.ZamowienieId == zamowienieId && o.Nazwisko == nazwisko);

            if (zamowienie == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            ZamowienieZrealizowaneEmail email = new ZamowienieZrealizowaneEmail();
            email.To = zamowienie.Email;
            email.From = "newregionalshop@gmail.com";
            email.NumerZamowienia = zamowienie.ZamowienieId;
            email.Send();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult DodajKategorie(int? kategoriaId, bool? potwierdzenie)
        {
            Kategoria kategoria;
            if (kategoriaId.HasValue)
            {
                ViewBag.EditMode = true;
                kategoria = db.Kategorie.Find(kategoriaId);
            }
            else
            {
                ViewBag.EditMode = false;
                kategoria = new Kategoria();
            }
            var result = new EditKategoriaViewModel();
            result.Kategoria = kategoria;
            result.Potwierdzenie = potwierdzenie;

            return View(result);         
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DodajKategorie(EditKategoriaViewModel model, HttpPostedFileBase file)
        {
            if (model.Kategoria.KategoriaId > 0)
            {
                // modyfikacja kategorii
                db.Entry(model.Kategoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DodajKategorie", new { potwierdzenie = true });
            }
            else
            {
                // Sprawdzenie, czy użytkownik wybrał plik
                if (file != null && file.ContentLength > 0)
                {
                    if (ModelState.IsValid)
                    {
                        // Generowanie pliku
                        var fileExt = Path.GetExtension(file.FileName);
                        var filename = Guid.NewGuid() + fileExt;

                        var path = Path.Combine(Server.MapPath(AppConfig.IkonyKategoriFolderWzgledny), filename);
                        file.SaveAs(path);

                        model.Kategoria.NazwaPlikuIkony = filename;

                        db.Entry(model.Kategoria).State = EntityState.Added;
                        db.SaveChanges();

                        return RedirectToAction("DodajKategorie", new { potwierdzenie = true });
                    }
                    else
                    {
                        var kategorie = db.Kategorie.ToList();
                        model.Kategorie = kategorie;
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nie wskazano pliku");
                    var kategorie = db.Kategorie.ToList();
                    model.Kategorie = kategorie;
                    return View(model);
                }
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult UkryjKategorie(int kategoriaId)
        {
            var kategoria = db.Kategorie.Find(kategoriaId);
            kategoria.Ukryta = true;
            db.SaveChanges();

            return RedirectToAction("DodajKategorie", new { potwierdzenie = true });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PokazKategorie(int kategoriaId)
        {
            var kategoria = db.Kategorie.Find(kategoriaId);
            kategoria.Ukryta = false;
            db.SaveChanges();

            return RedirectToAction("DodajKategorie", new { potwierdzenie = true });
        }

    }
}