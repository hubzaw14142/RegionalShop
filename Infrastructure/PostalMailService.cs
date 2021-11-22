using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RegionalShop.Models;
using RegionalShop.ViewModels;

namespace RegionalShop.Infrastructure
{
    public class PostalMailService : IMailService
    {
        public void WyslaniePotwierdzenieZamowieniaEmail(Zamowienie zamowienie)
        {
            PotwierdzenieZamowieniaEmail email = new PotwierdzenieZamowieniaEmail();
            email.To = zamowienie.Email;
            email.From = "newregionalshop@gmail.com";
            email.Wartosc = zamowienie.WartoscZamowienia;
            email.NumerZamowienia = zamowienie.ZamowienieId;
            email.PozycjeZamowienia = zamowienie.PozycjeZamowienia;
            email.Send();
        }

        public void WyslanieZamowienieZrealizowaneEmail(Zamowienie zamowienie)
        {
            ZamowienieZrealizowaneEmail email = new ZamowienieZrealizowaneEmail();
            email.To = zamowienie.Email;
            email.From = "NewRegionalShop@gmail.com";
            email.NumerZamowienia = zamowienie.ZamowienieId;
            email.Send();
        }
    }
}