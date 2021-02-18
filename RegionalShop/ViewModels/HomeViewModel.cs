using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RegionalShop.Models;

namespace RegionalShop.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Kategoria> Kategorie { get; set; }
        public IEnumerable<Produkt> Nowosci { get; set; }
        public IEnumerable<Produkt> Bestsellery { get; set; }
    }
}